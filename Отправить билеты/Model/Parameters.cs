namespace Отправить_билеты
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Имя пользователя системы moipass
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Пароль пользователя системы moipass
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Адрес эл. почты куда отсылать билеты
        /// </summary>
        public string SendMail { get; set; }
        public string AdminUrl { get; set; }
        public string ListUrl { get; set; }
        public string SendUrl { get; set; }
        public string MainUrl { get; set; }
        /// <summary>
        /// Переменная переключения режима отладки
        /// </summary>
        public bool IsDebugging { get; set; }
    }
}
