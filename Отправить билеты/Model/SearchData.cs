namespace Отправить_билеты
{
    /// <summary>
    /// Модель данных по которым проходит поиск заказов
    /// </summary>
    public class SearchData
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public string IdOrder { get; set; }
        /// <summary>
        /// Код заказа
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// Телефон клиента сделавшего заказ
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Адрес эл. почты клиента сделавшего заказ
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// С какой даты искать
        /// </summary>
        public string DateStart { get; set; }
        /// <summary>
        /// По какую дату искать
        /// </summary>
        public string DateEnd { get; set; }
    }
}
