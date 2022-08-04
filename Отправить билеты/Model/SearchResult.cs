namespace Отправить_билеты
{
    /// <summary>
    /// Модель данных по которым происходит отображение в таблице
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Код заказа
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// Адрес эл. почты заказа
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Откуда был сделан заказа
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Дата регистрации заказа
        /// </summary>
        public string RegestrationDate { get; set; }
        /// <summary>
        /// Статус заказа
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// На какую сумму заказ (по всем билетам)
        /// </summary>
        public string Summa { get; set; }

        public SearchResult()
        {
            Id = "";
            OrderCode = "";
            User = "";
            Site = "";
            RegestrationDate = "";
            Status = "";
            Summa = "";
        }


    }
}