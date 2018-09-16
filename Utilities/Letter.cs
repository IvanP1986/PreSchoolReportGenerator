using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Письмо.
    /// </summary>
    public class Letter : ViewModelBase
    {
        /// <summary>
        /// Заголовок письма.
        /// </summary>
        private string _header;
        /// <summary>
        /// Заголовок письма.
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Текст письма.
        /// </summary>
        private string _body;
        /// <summary>
        /// Текст письма.
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Вложения.
        /// </summary>
        private ObservableCollection<Attachment> attachments;
        /// <summary>
        /// Вложения.
        /// </summary>
        public ObservableCollection<Attachment> Attachments
        {
            get { return attachments; }
            set { attachments = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Скрытый список адресов электронной почты.
        /// </summary>
        private ObservableCollection<string> _hiddenAdresses;
        /// <summary>
        /// Скрытый список адресов электронной почты.
        /// </summary>
        public ObservableCollection<string> HiddenAdresses
        {
            get { return _hiddenAdresses; }
            set { _hiddenAdresses = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Список адресов электронной почты для обычной отправки.
        /// </summary>
        private ObservableCollection<string> _adresses;
        /// <summary>
        /// Список адресов электронной почты для обычной отправки.
        /// </summary>
        public ObservableCollection<string> Adresses
        {
            get { return _adresses; }
            set { _adresses = value; OnPropertyChanged(); }
        }

    }
}
