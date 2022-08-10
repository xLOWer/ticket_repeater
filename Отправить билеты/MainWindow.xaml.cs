using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            search_button.Command = SearchCommand;
            save_button.Command = SaveConfigCommand;
            read_button.Command = ReadConfigCommand;
            send_button.Command = SendCommand;

            if (!ConfigManager.IsReadConfig)
                ConfigManager.Read();

            _ws = new WebService();
            datefrom_textbox.SelectedDate = DateTime.Now.AddDays(-7);
            dateto_textbox.SelectedDate = DateTime.Now.AddDays(7);
            UpdateToUi_Parameters();
        }

        ///////////////////////////////////////////////////////////////////////////////////

        ICommand SendCommand => new RelayCommand((e) =>
        {
            SafeRunCommand(() =>
            {
                List<SearchResult> selectedItems = new List<SearchResult>();
                string id = "0";
                bool isSended = false;
                string errorMessage = "";

                Dispatcher.Invoke(() =>
                {
                    selectedItems = data_grid.SelectedItems.Cast<SearchResult>().ToList();
                });

                foreach(SearchResult item in selectedItems)
                {
                    id = item.Id;
                    isSended = _ws.Send(id);
                    if (!isSended)
                        errorMessage += $"Билет не отправлен с кодом заказа {item.OrderCode} ({item.User})\r\n";
                }
                if (string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show($"Все билеты помечены на отправку. В течение пары минут они придут в почту {ConfigManager.Parameters.SendMail}", 
                        "Успешно", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
                else
                    MessageBox.Show(errorMessage, 
                        "Ошибка", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);

            });
        });

        ICommand SearchCommand => new RelayCommand((e) =>
        {
            SafeRunCommand(() =>
            {
                SearchData data = new SearchData();
                List<SearchResult> list = new List<SearchResult>();
                Dispatcher.Invoke(() =>
                {
                    data = new SearchData()
                    {
                        IdOrder = idorder_textbox.Text,
                        OrderCode = ordercode_textbox.Text,
                        Phone = phone_textbox.Text,
                        Email = email_textbox.Text,
                        DateStart = datefrom_textbox.SelectedDate.Value.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd"),
                        DateEnd = dateto_textbox.SelectedDate.Value.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd"),
                    };
                });
                list = _ws.GetList(data);
                if (list == null)
                    return;

                Dispatcher.Invoke(() =>
                {
                    data_grid.ItemsSource = list;
                    data_grid.UpdateLayout();
                });
            });
        });

        ICommand ReadConfigCommand => new RelayCommand((e) =>
        {
            SafeRunCommand(() =>
            {
                ConfigManager.Read();
                UpdateToUi_Parameters();
            });
        });

        ICommand SaveConfigCommand => new RelayCommand((e) =>
        {
            SafeRunCommand(() =>
            {
                UpdateFromUi_Parameters();
                ConfigManager.Write();
                ConfigManager.Read();
                UpdateToUi_Parameters();
            });
        });

        private async void SafeRunCommand(Action action)
        {
            try
            {
                BlockUi();
                await Task.Run(action);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UnblocUi();
            }
        }

        private void UpdateToUi_Parameters()
        {
            Dispatcher.Invoke(() =>
            {
                user_textbox.Text = ConfigManager.Parameters.User;
                password_textbox.Password = ConfigManager.Parameters.Password;
                admin_url_textbox.Text = ConfigManager.Parameters.AdminUrl;
                list_url_textbox.Text = ConfigManager.Parameters.ListUrl;
                send_url_textbox.Text = ConfigManager.Parameters.SendUrl;
                main_url_textbox.Text = ConfigManager.Parameters.MainUrl;
                sendmail_textbox.Text = ConfigManager.Parameters.SendMail;
            });
        }

        private void UpdateFromUi_Parameters()
        {
            Dispatcher.Invoke(() =>
            {
                ConfigManager.Parameters.User = user_textbox.Text;
                ConfigManager.Parameters.Password = password_textbox.Password;
                ConfigManager.Parameters.AdminUrl = admin_url_textbox.Text;
                ConfigManager.Parameters.ListUrl = list_url_textbox.Text;
                ConfigManager.Parameters.SendUrl = send_url_textbox.Text;
                ConfigManager.Parameters.MainUrl = main_url_textbox.Text;
                ConfigManager.Parameters.SendMail = sendmail_textbox.Text;
            });
        }

        private void BlockUi()
        {
            Dispatcher.Invoke(() =>
            {
                read_button.IsEnabled = false;
                save_button.IsEnabled = false;
                user_textbox.IsEnabled = false;
                password_textbox.IsEnabled = false;
                admin_url_textbox.IsEnabled = false;
                list_url_textbox.IsEnabled = false;
                send_url_textbox.IsEnabled = false;
                main_url_textbox.IsEnabled = false;
                send_button.IsEnabled = false;
                search_button.IsEnabled = false;
                sendmail_textbox.IsEnabled = false;
                email_textbox.IsEnabled = false;
                ordercode_textbox.IsEnabled = false;
                idorder_textbox.IsEnabled = false;
                phone_textbox.IsEnabled = false;
                datefrom_textbox.IsEnabled = false;
                dateto_textbox.IsEnabled = false;
            });
        }

        private void UnblocUi()
        {
            Dispatcher.Invoke(() =>
            {
                read_button.IsEnabled = true;
                save_button.IsEnabled = true;
                user_textbox.IsEnabled = true;
                password_textbox.IsEnabled = true;
                admin_url_textbox.IsEnabled = true;
                list_url_textbox.IsEnabled = true;
                send_url_textbox.IsEnabled = true;
                main_url_textbox.IsEnabled = true;
                send_button.IsEnabled = true;
                search_button.IsEnabled = true;
                sendmail_textbox.IsEnabled = true;
                email_textbox.IsEnabled = true;
                ordercode_textbox.IsEnabled = true;
                idorder_textbox.IsEnabled = true;
                phone_textbox.IsEnabled = true;
                datefrom_textbox.IsEnabled = true;
                dateto_textbox.IsEnabled = true;
            });
        }


    }
}
