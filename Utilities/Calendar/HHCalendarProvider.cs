using System;
using System.Collections.Generic;
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
                var task = _GetCalendarDocumentAsync(year);
                task.Wait();

                XmlDocument xdoc = task.Result;

                IEnumerable<Month> months = _GetMonth(xdoc).ToArray();

                if (_years == null) _years = new List<Year>();
                _years.Add(new Year() { Number = year, Months = months.ToArray() });
                _SaveToFile();
            }

            return _years.First(y => y.Number==year).Months.First(m => m.Number == month).Days;
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
