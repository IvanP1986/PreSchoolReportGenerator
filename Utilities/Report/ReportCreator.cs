using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities.AgeGroup;
using Utilities.Calendar;

namespace Utilities.Report
{
    /// <summary>
    /// Создает файл отчета о посещаемости.
    /// </summary>
    public partial class ReportCreator
    {
        /// <summary>
        /// Папка для отчетов.
        /// </summary>
        const string REPORT_FOLDER = "ChildReports";
        /// <summary>
        /// Папка с шаблонами.
        /// </summary>
        const string TEMPLATE_FOLDER = "Templates";
        /// <summary>
        /// Отчет о посещаемости ребенка.
        /// </summary>
        public ChildReport Report { get; set; }

        private CalendarManager _calendarManager;
        /// <summary>
        /// Конструтор с параметрами.
        /// </summary>
        public ReportCreator(ChildReport report, CalendarManager calendarManager)
        {
            Report = report;
            _calendarManager = calendarManager;
        }
        
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="calendarManager"></param>
        public ReportCreator(CalendarManager calendarManager) : this(null, calendarManager)
        {
        }
        public string GenerateReport()
        {
            string reportFile = _CopyTemplateReport();

            ExcelClient excelClient = new ExcelClient(reportFile, Report, _calendarManager);
            excelClient.WriteData();
            Report.FilePath = reportFile;

            return reportFile;
        }
        /// <summary>
        /// Возвращает полный путь к папке, где будет храниться отчет.
        /// </summary>
        /// <returns>Путь к папке.</returns>
        private string _GetFolderName()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                REPORT_FOLDER,
                $"{Report.Period.Year}_{Report.Period.Month}");
        }
        /// <summary>
        /// Получает суффикс для имени файла отчета по возрастной группе.
        /// </summary>
        /// <param name="ageGroup">Возрастная группа.</param>
        /// <returns>Суффикс.</returns>
        private string _GetSuffixByAgeGroup(string ageGroup)
        {
            string suffix = "5-7";
            if (ageGroup.Contains("3") && ageGroup.Contains("5"))
            {
                suffix = "3-5";
            }
            else if (ageGroup.Contains("3"))
            {
                suffix = "3";
            }
            return suffix;
        }
        /// <summary>
        /// Возвращает путь к шаблону эталонного отчета.
        /// </summary>
        /// <returns></returns>
        private string _GetTemplateReportFilePath()
        {
            string fileName = $"template01.xls";

            //string fileName = $"Template_{_GetSuffixByAgeGroup(Report.AgeGroup)}.xls";


            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                TEMPLATE_FOLDER,
                fileName);
        }
        /// <summary>
        /// Копирует файл шаблона возрастной группы.
        /// </summary>
        /// <returns>Полный путь нового файла.</returns>
        private string _CopyTemplateReport()
        {
            string reportFolder = _GetFolderName();
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);

            string dateStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string newFileName = $"Report_{_GetSuffixByAgeGroup(Report.AgeGroup.GetDescription())}_{dateStamp}.xls";

            string reportFilePath = Path.Combine(reportFolder, newFileName);

            string sourceFilePath = _GetTemplateReportFilePath();
            File.Copy(sourceFilePath, reportFilePath, true);
            return reportFilePath;
        }
    }
}
