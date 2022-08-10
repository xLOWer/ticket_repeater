using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Отправить_билеты
{
    public static class ConfigManager
    {
        private static string _directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\ticket_repeater";
        private static string _confFileName = "conf";
        private static string _confFile = Path.GetFullPath(_directory + "\\" + _confFileName);

        /// <summary>
        /// Указывает был ли считан фал с настройками
        /// </summary>
        public static bool IsReadConfig = false;

        /// <summary>
        /// Считывает конфигурацию приложения из файла настроек
        /// </summary>
        public static void Read()
        {
            try
            {
                if (File.Exists(_confFile))
                {
                    string confJsonText = File.ReadAllText(_confFile);
                    if (string.IsNullOrEmpty(confJsonText))
                        throw new Exception("Файл с параметрами пуст");
                    Parameters parameters = JsonConvert.DeserializeObject<Parameters>(confJsonText);
                    Parameters = parameters;
                    return;
                }
                else
                {
                    var result = MessageBox.Show("Создать стандартную конфигурацию?", "Конфигурация не найдена", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SetDefaultParameters();
                        Write();
                        Read();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            IsReadConfig = true;
        }

        /// <summary>
        /// Создаёт настройки по-умолчанию
        /// </summary>
        public static void SetDefaultParameters()
        {
            Parameters = new Parameters
            {
                User = "",
                Password = "",
                AdminUrl = "https://admin.moipass.ru/Sellers",
                ListUrl = "https://admin.moipass.ru/sellers/orders/list",
                SendUrl = "https://admin.moipass.ru/Sellers/orders/SendOnAnotherEmail",
                SendMail = "primorskij.okeanarium@yandex.ru",
                MainUrl = "https://admin.moipass.ru/",
                IsDebugging = false
            };
        }

        /// <summary>
        /// Записывает изменения в файл настроек приложения
        /// </summary>
        public static void Write()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(Parameters, Formatting.Indented);
                File.WriteAllText(_confFile, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Хранилище параметров приложения
        /// </summary>
        public static Parameters Parameters;

        static ConfigManager()
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            Parameters = new Parameters()
            {
                User = "",
                Password = "",
                AdminUrl = "",
                ListUrl = "",
                SendUrl = "",
                SendMail = "",
                MainUrl = "",
                IsDebugging = false
            };
        }

    }

}
