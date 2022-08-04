using System;
using System.Windows;
using System.Windows.Input;

namespace Отправить_билеты
{
    public partial class MainWindow : Window
    {        
        private WebService _ws;

        public MainWindow()
        {
            InitializeComponent();

            // навешиваем команды на кнопки
            search_button.Command = SearchCommand;
            save_button.Command = SaveConfigCommand;
            read_button.Command = ReadConfigCommand;
            send_button.Command = SendCommand;

            if (!ConfigManager.IsReadConfig)            
                ConfigManager.Read();    
            
            _ws = new WebService();
            // задаём границы дат поиска
            datefrom_textbox.SelectedDate = DateTime.Now.AddDays(-7);
            dateto_textbox.SelectedDate = DateTime.Now.AddDays(7);
            // обновим ui
            UpdateToUi_Parameters();
        }
        
        /// <summary>
        /// Команда для кнопки "Отправить"
        /// </summary>
        ICommand SendCommand => new RelayCommand(new Action<object>((e) => {
            // 1 смотрим в выделенную строку таблицы
            // 2 нам нужен только id заказа котрый надо отправить
            // 3 отправляем
            if (_ws.Send/*3*/(((SearchResult)data_grid.SelectedItem/*1*/).Id/*2*/))            
                MessageBox.Show($"В ближайшее время будет выслано в почту {ConfigManager.Parameters.SendMail}", "Успешно");            
        }));

        /// <summary>
        /// Команда для кнопки "Поиск"
        /// </summary>
        ICommand SearchCommand => new RelayCommand(new Action<object>((e) => {
            // 1 формируем поисковый запрос
            // 2 отправлчем его искать данные
            // 3 засовываем в таблицу
            // 4 обновляем таблицу в ui
            data_grid.ItemsSource/*3*/ = _ws.GetList/*2*/(/*1*/new SearchData()
            {
                IdOrder = idorder_textbox.Text,
                OrderCode = ordercode_textbox.Text,
                Phone = phone_textbox.Text,
                Email = email_textbox.Text,
                DateStart = datefrom_textbox.SelectedDate.Value.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd"),
                DateEnd = dateto_textbox.SelectedDate.Value.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd"),
            });
            data_grid.UpdateLayout(); // 4
        }));

        /// <summary>
        /// Команда для кнопки "Загрузить настройки"
        /// </summary>
        ICommand ReadConfigCommand => new RelayCommand(new Action<object>((e) => {
            ConfigManager.Read();
            UpdateToUi_Parameters();
        }));

        /// <summary>
        /// Команда для кнопки "Сохранить настройки"
        /// </summary>
        ICommand SaveConfigCommand => new RelayCommand(new Action<object>((e) => {
            UpdateFromUi_Parameters();
            ConfigManager.Write();
            ConfigManager.Read();
            UpdateToUi_Parameters();
        }));

        private void UpdateToUi_Parameters()
        {
            user_textbox.Text = ConfigManager.Parameters.User;
            password_textbox.Password= ConfigManager.Parameters.Password;
            admin_url_textbox.Text = ConfigManager.Parameters.AdminUrl;
            list_url_textbox.Text = ConfigManager.Parameters.ListUrl;
            send_url_textbox.Text = ConfigManager.Parameters.SendUrl;
            main_url_textbox.Text = ConfigManager.Parameters.MainUrl;
            sendmail_textbox.Text = ConfigManager.Parameters.SendMail;
        }
        // для экономии времени решил отказаться от любых паттернов 
        // и сделал тупую процедурную отбрабоку вот этими двумя методами
        private void UpdateFromUi_Parameters()
        {
            ConfigManager.Parameters.User = user_textbox.Text;
            ConfigManager.Parameters.Password = password_textbox.Password;
            ConfigManager.Parameters.AdminUrl = admin_url_textbox.Text;
            ConfigManager.Parameters.ListUrl = list_url_textbox.Text;
            ConfigManager.Parameters.SendUrl = send_url_textbox.Text;
            ConfigManager.Parameters.MainUrl = main_url_textbox.Text;
            ConfigManager.Parameters.SendMail = sendmail_textbox.Text;
        }
    }
}
