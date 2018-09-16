using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Report;

namespace Utilities
{
    /// <summary>
    /// Вложение в письмо.
    /// </summary>
    public class Attachment : ViewModelBase
    {

        /// <summary>
        /// Файл с отчетом о ребенке.
        /// </summary>
        private string _reportFilePath;
        /// <summary>
        /// Файл с отчетом о ребенке.
        /// </summary>
        public string ReportFilePath
        {
            get { return _reportFilePath; }
            set { _reportFilePath = value; OnPropertyChanged();  }
        }
        /// <summary>
        /// Отчет о ребенке.
        /// </summary>
        private ChildReport _childReport;
        /// <summary>
        /// Отчет о ребенке.
        /// </summary>
        public ChildReport ChildReport
        {
            get { return _childReport; }
            set { _childReport = value; OnPropertyChanged(); }
        }

    }
}
