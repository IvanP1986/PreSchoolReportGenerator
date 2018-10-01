using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilities;
using Utilities.Calendar;
using Utilities.Report;

namespace WpfApplication
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Вложения.
        /// </summary>
        private ObservableCollection<Utilities.Attachment> attachments;
        /// <summary>
        /// Вложения.
        /// </summary>
        public ObservableCollection<Utilities.Attachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Список возрастных групп.
        /// </summary>
        private ObservableCollection<AgeGroupType> _ageGroups;
        /// <summary>
        /// Список детей.
        /// </summary>
        private ObservableCollection<string> _children;
        /// <summary>
        /// Список отчетов.
        /// </summary>
        private ObservableCollection<ChildReport> _childReports;
        /// <summary>
        /// Список отчетов.
        /// </summary>
        public ObservableCollection<ChildReport> ChildReports
        {
            get { return _childReports; }
            set {
                _childReports = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Список детей.
        /// </summary>
        public ObservableCollection<string> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Список возрастных групп.
        /// </summary>        
        public ObservableCollection<AgeGroupType> AgeGroups
        {
            get { return _ageGroups; }
            set
            {
                _ageGroups = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Список писем.
        /// </summary>
        private ObservableCollection<Letter> letters;
        /// <summary>
        /// Список писем.
        /// </summary>
        public ObservableCollection<Letter> Letters
        {
            get { return letters; }
            set { letters = value; }
        }

        private Letter _selectedLetter;

        public Letter SelectedLetter
        {
            get { return _selectedLetter; }
            set { _selectedLetter = value; OnPropertyChanged(); }
        }

        public string SelectedTeacherName { get; set; }

        private ICalendarProvider _calendarProvider;

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }


        public MainWindowViewModel(Options options, ICalendarProvider calendarProvider)
        {
            _ageGroups = options.AgeGroups.ToObservableCollection();
            _children = options.Children.ToObservableCollection();
            _childReports = options.Reports.ToObservableCollection();
            SelectedTeacherName = "Пустобаева И.Б.";
            _calendarProvider = calendarProvider;

            ChildReport report = _CreateNewSampleReport();
            var calendarManager = new Utilities.Calendar.CalendarManager(new Utilities.Calendar.HHCalendarProvider());

            ReportCreator reportCreator = new ReportCreator(report, calendarManager);
            string reportFile = reportCreator.GenerateReport();
            
            Letters = new ObservableCollection<Letter>();

            Letter letter = _CreateNewSampleLetter();
            letter.Attachments.Clear();
            letter.Attachments.Add(new Utilities.Attachment() { ReportFilePath = reportFile });

            Letters.Add(letter);
            SelectedLetter = Letters.First();
            _InitializeCommand();
        }
        private void CreateSampleReports()
        {

        }
        private ChildReport _CreateNewSampleReport()
        {
            ChildReport report = new ChildReport()
            {
                AgeGroup = _ageGroups.First(),
                Children = _children.Take(3).ToObservableCollection(),
                Period = new Period() { Year = DateTime.Now.Year, Month = 2 }
                ,TeacherName = this.SelectedTeacherName
            };

            return report;
        }

        /// <summary>
        /// Инициализирует комманды.
        /// </summary>
        private void _InitializeCommand()
        {
            SendSelectedLetterCommand = new RelayCommand(_SendSelectedLetter, o => SelectedLetter != null);
        }

        private void _SendSelectedLetter(object obj)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ivan_pust@mail.ru");
            mail.To.Add(SelectedLetter.Adresses[0]);
            mail.Bcc.Add(SelectedLetter.HiddenAdresses[0]);
            mail.Subject = SelectedLetter.Header;
            mail.Body = SelectedLetter.Body;
            foreach(var attachement in SelectedLetter.Attachments)
            {
                mail.Attachments.Add(new System.Net.Mail.Attachment(attachement.ReportFilePath));
            }
            

            SmtpClient SmtpServer = new SmtpClient("smtp.mail.ru");
            SmtpServer.Port = 25;
            
            SmtpServer.Credentials = new NetworkCredential("ivan_pust@mail.ru", Password);
            SmtpServer.EnableSsl = true;

            try
            {
               // SmtpServer.Send(mail);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            MessageBox.Show("Письмо успешно отправлено!");
        }

        public ICommand SendSelectedLetterCommand { get; set; }

        private Letter _CreateNewSampleLetter()
        {
            Letter letter = new Letter()
            {
                Adresses = new ObservableCollection<string>()
                {
                    "ivan_pust@mail.ru"
                },
                HiddenAdresses = new ObservableCollection<string>()
                {
                    "ivapus@yandex.ru"
                },
                Attachments = new ObservableCollection<Utilities.Attachment>()
                {
                    new Utilities.Attachment() { ReportFilePath = @"c:\Work\!Docs\My\Табели\2018_08\example_5-7.xls" }
                    ,new Utilities.Attachment() { ReportFilePath = @"c:\Work\!Docs\My\Табели\2018_08\example_до 3-х.xls" }
                    ,new Utilities.Attachment() { ReportFilePath = @"c:\Work\!Docs\My\Табели\2018_08\example_от 3-х до 5-и.xls" }
                },
                Header = "Отчет по Пустобаевым за <месяц>",
                Body = @"Здравствуйте, в прикрепленных файлах отчеты о посещаемости детей. 
    
    Пустобаева Ирина."

            };


            return letter;
        }
    }
}
