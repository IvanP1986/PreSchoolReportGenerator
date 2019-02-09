using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Utilities.AgeGroup;
using Utilities.Calendar;

namespace Utilities.Report
{
    public partial class ReportCreator
    {
        private class ExcelClient
        {
            private string _fileName;

            private ChildReport _childReport;

            private Application _application;

            private Workbook _workBook;

            private Worksheet _workSheet;

            private CalendarManager _calendarManager;

            public ExcelClient(string fileName, ChildReport childReport, CalendarManager calendarManager)
            {
                _fileName = fileName;
                _childReport = childReport;
                _calendarManager = calendarManager;
            }

            public void WriteData()
            {
                _OpenFile();

                _ReplaceValue("{Month}", _GetMonthDate(_childReport.Period));
                _ReplaceValue("{DigitDate}", _GetDigitDate(_childReport.Period));
                _ReplaceValue("{LongDate}", _GetLongDate(_childReport.Period));
                _ReplaceValue("{AgeGroup}", _childReport.AgeGroup.GetDescription());
                _ReplaceValue("{TeacherName}", _childReport.TeacherName);

                //Размножение строк для детей
                Range line = (Range)_workSheet.Rows[14];
                for (int i = 0; i < _childReport.Children.Count() - 1; i++)
                {
                    line.Insert();
                    int row = 14 + i;
                    _workSheet.get_Range($"b{row}", $"an{row}").Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                }

                //Убираем лишние дни (изначально считаем, что 31 день в месяце)
                var days = _calendarManager.GetDayList(_childReport.Period.Year, _childReport.Period.Month).ToArray();
                for (int i = 36; i > days.Length + 5; i--)
                {
                    ((Range)_workSheet.Columns[i]).Delete();
                }
                
                //Заполняем отчет
                for (int childIndex = 0; childIndex < _childReport.Children.Count; childIndex++)
                {
                    int row = 13 + childIndex;
                    _workSheet.Cells[row, 2] = childIndex + 1;
                    _workSheet.Cells[row, 3] = _childReport.Children[childIndex];
                    _workSheet.Cells[row, 5] = "0";

                    for (int day = 0; day < days.Length; day++)
                    {
                        _workSheet.Cells[row, 6 + day] = (days[day].IsWorkDay) ? "" : "В";
                    }

                    _workSheet.Cells[row, 6 + days.Length + 2] = days.Count(d => d.IsWorkDay);
                }

                for (int day = 0; day < days.Length; day++)
                {
                    _workSheet.Cells[13 + _childReport.Children.Count, 6 + day] = (days[day].IsWorkDay) ?
                        _childReport.Children.Count.ToString() : "";

                    _workSheet.Cells[14 + _childReport.Children.Count, 6 + day] = (days[day].IsWorkDay) ?
                        "0" : "";

                    if (day == days.Length - 1)
                    {
                        _workSheet.Cells[13 + _childReport.Children.Count, 6 + day + 3] =
                            days.Count(d => d.IsWorkDay) * _childReport.Children.Count;

                        _workSheet.Cells[13 + _childReport.Children.Count, 6 + day + 4] =
                            days.Count(d => d.IsWorkDay) * _childReport.Children.Count;

                        _workSheet.Cells[14 + _childReport.Children.Count, 6 + day + 4] = "0";
                    }
                }

                _workBook.Save();
                _application.Visible = true;

                //_SaveAndCloseFile();
            }
            private string _GetLongDate(Period period)
            {
                var days = _calendarManager.GetDayList(period.Year, period.Month);

                var cultureInfo = CultureInfo.CreateSpecificCulture("ru");
                int lastWorkDay = days.Last(d => d.IsWorkDay).Number;
                string result = (new DateTime(period.Year, period.Month, lastWorkDay)).ToString("dd MMMM yyyyг.", cultureInfo);

                return result;
            }
            /// <summary>
            /// Возвращает дату в числовом формате. Пример, "30 09 2018"
            /// </summary>
            /// <param name="period"></param>
            /// <returns></returns>
            private string _GetDigitDate(Period period)
            {
                var days = _calendarManager.GetDayList(period.Year, period.Month);

                int lastWorkDay = days.Last(d => d.IsWorkDay).Number;
                string result = (new DateTime(period.Year, period.Month, lastWorkDay)).ToString("dd MM yyyy");

                return result;
            }
            /// <summary>
            /// Возвращает дату месяц и год. Пример, "Февраль 2018 г."
            /// </summary>
            /// <param name="period"></param>
            /// <returns></returns>
            private string _GetMonthDate(Period period)
            {
                var cultureInfo = CultureInfo.CreateSpecificCulture("ru");
                string result = (new DateTime(period.Year, period.Month, 1)).ToString("MMMM yyyy г.", cultureInfo);

                return result;
            } 
             
            private void _ReplaceValue(string replace, string replacement)
            {
                object m = Type.Missing;
                // get the used range. 
                Range r = (Range)_workSheet.UsedRange;

                bool success = (bool)r.Replace(
                    replace, replacement,
                    XlLookAt.xlPart,
                    XlSearchOrder.xlByRows,
                    m, m, m, m);
            }

            private void _SaveAndCloseFile()
            {
                _workBook.Save();
                _application.Quit();

                Marshal.ReleaseComObject(_workSheet);
                Marshal.ReleaseComObject(_workBook);
                Marshal.ReleaseComObject(_application);

                _workSheet = null;
                _workBook = null;
                _application = null;
                GC.Collect();
            }

            private void _OpenFile()
            {
                _application = new Application();
                _workBook = _application.Workbooks.Open(_fileName);
                _workSheet = (Microsoft.Office.Interop.Excel.Worksheet)_workBook.Sheets.get_Item(1);
            }

        }
    }
}
