using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities;
using Utilities.Calendar;
using Utilities.Report;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            const string path = "Configuration.xml";
            Options options= new Options();
            try
            {
                throw new Exception();
                //options.LoadOptionsFromFile(path);
            }
            catch
            {
                options.CreateNewConfiguration();
                options.SaveOptionsToFile(path);
            }

            ICalendarProvider calendarProvider = new Utilities.Calendar.HHCalendarProvider();
            var calendarManager = new Utilities.Calendar.CalendarManager(calendarProvider);
            ReportCreator reportCreator = new ReportCreator(calendarManager);

            MainWindowViewModel vm = new MainWindowViewModel(options, reportCreator);
            this.DataContext = vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Password = ((PasswordBox)e.OriginalSource).Password;
        }
    }
}
