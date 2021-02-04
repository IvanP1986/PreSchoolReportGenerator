﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Calendar
{
    /// <summary>
    /// Реализация интерфейса на основе  производственного календаря HeadHunter.
    /// </summary>
    public class HHCalendarProvider : ICalendarProvider
    {
        /// <summary>
        /// Базовый адрес для календаря. 
        /// </summary>
        private const string HH_CALENDAR_BASE_URL = @"https://hh.ru/article/";

        private const string CALENDAR_FILE_NAME = @"hh_calendar.xml";
        /// <summary>
        /// Список календарей.
        /// </summary>
        private List<Year> _years;

        /// <summary>
        /// Возвращает список дней месяца.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Порядковый номер месяца.</param>
        /// <returns>Результат выполнения операции.</returns>
        public IEnumerable<Day> GetDayList(int year, int month)
        {
            //load from file
            _LoadFromFile();

            if (_years?.FirstOrDefault(y => y.Number == year) == null)
            {
                IEnumerable<Month> months = null;
                months = _GetMonth2021(year);
                //if (year < 2020)
                //{
                //    var task = _GetCalendarDocumentAsync(year);
                //    task.Wait();

                //    XmlDocument xdoc = task.Result;

                //    months = _GetMonth(xdoc).ToArray();
                //}
                //else if (year < 2021)
                //{
                //    months = _GetMonth2020(year);
                //}
                //else
                //{
                //    months = _GetMonth2021(year);
                //}

                if (_years == null) _years = new List<Year>();
                _years.Add(new Year() { Number = year, Months = months.ToArray() });
                _SaveToFile();
            }

            return _years.First(y => y.Number==year).Months.First(m => m.Number == month).Days;
        }

        private IEnumerable<Month> _GetMonth2020(int year)
        {
            string url = @"https://hh.ru/calendar";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            var body = htmlDoc.DocumentNode.ChildNodes
                .First(x => x.Name == "html")
                .SelectSingleNode("body");

            var calendarNode = body
                .SelectNodes("div")
                .First(x => x.GetClasses().Any(c => c == "HH-MainContent")).FirstChild.FirstChild
                .SelectNodes("div")
                .First(x => x.GetClasses().Any(c => c == "bloko-columns-wrapper"))
                .FirstChild.FirstChild.FirstChild
                .SelectSingleNode("div")
                .SelectNodes("div")
                .First(x => x.GetClasses().Any(c => c == "calendar-content"))
                .SelectNodes("ul");

            var months = calendarNode
                .SelectMany(x => x.SelectNodes("li"))
                .Select(m => m.SelectSingleNode("div"))
                .Select(x => new
                {
                    Month = x.SelectSingleNode("div").InnerText,
                    Days = x
                        .SelectSingleNode(@"ul[2]")
                        .SelectNodes("li")
                        .Where(q => q.InnerText.Trim() != "")
                        .Select(q => new
                            {
                                Number = Int32.Parse(Regex.Match(q.InnerText, @"\d+").Value),
                                IsDayOff = q.GetClasses().Any(c=>c.Contains("item_day-off"))
                            })
                        .ToList()
                })
                .ToList();

            CultureInfo ruCulture = new CultureInfo("ru-RU");

            var result = months
                .Select(x => new Month()
                {
                    Number = DateTime.ParseExact(x.Month, "MMMM", ruCulture).Month,
                    Days = x.Days
                        .Select(d => new Day() { Number = d.Number, IsWorkDay = !d.IsDayOff })
                        .ToArray()
                })
                .ToList();

            return result;
        }

        private bool _IsWorkDay(int year, int month, int day)
        {
            var freeDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var dayOfWeek = (new DateTime(year, month, day)).DayOfWeek;

            return !freeDays.Contains(dayOfWeek);
        }

        private IEnumerable<Month> _GetMonth2021(int year)
        {
            var months = GenerateMonths(year).ToList();

            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync($"http://xmlcalendar.ru/data/ru/{year}/calendar.xml").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            var xml = new XmlDocument();
            xml.LoadXml(content);
            var days = xml.SelectSingleNode("//days").ChildNodes
                .Cast<XmlElement>()
                .Select(s =>
                {
                    var d = s.GetAttribute("d");
                    var dd = DateTime.ParseExact($"{year}.{d}", "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    var month = dd.Month;
                    var day = dd.Day;
                    var t = s.GetAttribute("t");
                    var isWorkDay = t == "2" || t == "3";
                    var h = s.GetAttribute("h");

                    return new { month, day, isWorkDay, s};
                })
                .ToList();

            days.ForEach(i =>
            {
                var day = months.First(m => m.Number == i.month).Days
                    .First(d => d.Number == i.day);

                day.IsWorkDay = i.isWorkDay;
            });

            return months;
        }

        private IEnumerable<Month> GenerateMonths(int year)
        {
            return Enumerable.Range(1, 12)
                .Select(m => new Month()
                {
                    Number = m,
                    Days = Enumerable.Range(1, DateTime.DaysInMonth(year, m))
                        .Select(d => new Day()
                        {
                            Number = d,
                            IsWorkDay = _IsWorkDay(year, m, d)
                        })
                        .ToArray()
                });
        }

        /// <summary>
        /// Получение документа xml календаря.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <returns>Документ.</returns>
        private async Task<XmlDocument> _GetCalendarDocumentAsync(int year)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HH_CALENDAR_BASE_URL);

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress, $"calendar{year.ToString()}")
            };

            var content = await httpClient.GetStringAsync(httpRequest.RequestUri).ConfigureAwait(false);
            //var response = await httpClient.SendAsync(httpRequest);

            //var content = await response.Content.ReadAsStringAsync();

            content = (new Regex(@"(?=.ul\sclass =.calendar-list.).+(?=<div\sclass=.calendar-info.>)", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace)).Match(content).Value;

            string document = $"<root>{content}</root>";

            document = _CorrectOpenTags(document);
            

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(document);

            return xdoc;
        }
        /// <summary>
        /// Возвращает текстовый html с закрытыми тегами.
        /// </summary>
        /// <param name="document">Текстовый html.</param>
        /// <returns>Исправленный текстовый html.</returns>
        private string _CorrectOpenTags(string document)
        {
            string result = document.Clone() as string;
            _CloseLiTag(ref result);
            _CloseBrTag(ref result);

            return result;
        }
        /// <summary>
        /// Закрывает открые теги li.
        /// </summary>
        /// <param name="document"></param>
        private void _CloseLiTag(ref string document)
        {
            string openLiTagPattern = @"(<li[^<>]*?[^/][>][^<>]*?)(?=(?=\<li|</ul>))";
            string closeLiTagReplacementPattern = @"$1</li>";

            string result = Regex.Replace(document, openLiTagPattern, closeLiTagReplacementPattern);
            document = result;
        }
        /// <summary>
        /// Закрывает открытый тег br.
        /// </summary>
        /// <param name="document"></param>
        private void _CloseBrTag(ref string document)
        {
            string openBrTagPattern = @"(<br(?:[^<>]*?[^/])?[>])";
            string closeBrTagReplacementPattern = String.Empty;//@"$1</br>";

            string result = Regex.Replace(document, openBrTagPattern, closeBrTagReplacementPattern);
            document = result;
        }

        /// <summary>
        /// Получение списка месяцев производственного календаря с днями.
        /// </summary>
        /// <param name="xdoc">Документ календаря.</param>
        /// <returns></returns>
        private IEnumerable<Month> _GetMonth(XmlDocument xdoc)
        {
            List<Month> months = new List<Month>();

            for (int i = 1; i <= 12; i++)
            {
                Month month = new Month()
                {
                    Number = i
                    ,Days = _GetDays(xdoc, i).ToArray()
                };
                months.Add(month);
            }

            return months;
        }
        /// <summary>
        /// Получение списка дней месяца.
        /// </summary>
        /// <param name="xdoc">Документ календаря.</param>
        /// <param name="monthNumber">Порядковый номер месяца.</param>
        /// <returns>Список дней.</returns>
        private IEnumerable<Day> _GetDays(XmlDocument xdoc, int monthNumber)
        {
            XmlNode monthNode = xdoc.FirstChild.FirstChild.ChildNodes[monthNumber - 1];
            var days = monthNode.FirstChild.LastChild.ChildNodes
                .Cast<XmlElement>()
                .Where(x => x.FirstChild != null)
                .ToArray()
                .Select(x => new {
                    Number = x.FirstChild.Value.ToString().TrimEnd(),
                    IsWorkDay = !x.GetAttribute("class").EndsWith("day-off")
                })
                .Where(x => !String.IsNullOrEmpty(x.Number))
                .Select(x => new Day { Number = Int32.Parse(x.Number), IsWorkDay = x.IsWorkDay })
                .SkipWhile(d => d.Number > 1)
                .ToArray();

            var distinctDays = days.Distinct(new DayIEqualityComparer()).ToArray();

            return distinctDays;
        }
        /// <summary>
        /// Сохраняет имеющиеся календари в файл.
        /// </summary>
        private void _SaveToFile()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Year[]));

            using (FileStream fs = new FileStream(CALENDAR_FILE_NAME, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _years.ToArray());
            }
        }
        /// <summary>
        /// Загружает сохраненные календари из файла.
        /// </summary>
        private void _LoadFromFile()
        {
            if (_years!=null || !System.IO.File.Exists(CALENDAR_FILE_NAME)) 
                return;

            XmlSerializer formatter = new XmlSerializer(typeof(Year[]));

            using (FileStream fs = new FileStream(CALENDAR_FILE_NAME, FileMode.Open))
            {
                _years = ((Year[])formatter.Deserialize(fs)).ToList();
            }

            if (_years == null)
                _years = new List<Year>();
        }
    }
}
