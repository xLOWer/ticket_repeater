using System;
using System.Collections.Generic;
using System.Windows;

namespace Отправить_билеты
{
    public class WebService
    {
        private string _loginToken;
        private HttpSession _session;

        public WebService()
        {
            if (string.IsNullOrEmpty(ConfigManager.Parameters.Password) ||
                string.IsNullOrEmpty(ConfigManager.Parameters.User)     ||
                string.IsNullOrEmpty(ConfigManager.Parameters.AdminUrl) ||
                string.IsNullOrEmpty(ConfigManager.Parameters.MainUrl)  ||
                string.IsNullOrEmpty(ConfigManager.Parameters.SendUrl)  ||
                string.IsNullOrEmpty(ConfigManager.Parameters.ListUrl))
            {
                ConfigManager.Read();
            }
            _session = new HttpSession();
        }
        ///////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Отправляет заказ на почту
        /// </summary>
        /// <param name="IdOrder">Идентификатор заказа</param>
        /// <returns></returns>
        public bool Send(string IdOrder)
        {
            Auth();
            try
            {
                _session.Post(ConfigManager.Parameters.SendUrl, $"Id={IdOrder}&UserName={Uri.EscapeDataString(ConfigManager.Parameters.SendMail)}");
            }
            catch(Exception ex)
            {
                Logger.Log("EXCEPTION_Message: " + ex.Message);
                Logger.Log("EXCEPTION_StackTrace: " + ex.StackTrace);
                Logger.Log("EXCEPTION_Source: " + ex.Source);
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        ///////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Получить список искомых заказов
        /// </summary>
        /// <param name="search">параметры поиска</param>
        /// <returns></returns>
        public List<SearchResult> GetList(SearchData search)
        {
            string searchString = GetLookupString(search);
            Auth();
            // получаем список по поисковому запросу
            _session.Post(ConfigManager.Parameters.ListUrl, searchString, true);
            return HtmlParser.ProcessList(_session.ResponseData);
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private void Auth()
        {
            try
            {
                // делаем первый запрос
                _session.Get(ConfigManager.Parameters.AdminUrl);
                // по сути без авторизации он только ради редиректа в нём
                // запомним куда этот редирект, там важные параметры
                // необходимые для авторизации
                string resp1url = _session.GetLocationValue();
                // если отстрельнуло без редиректа, то значит сессия ещё не истекла
                // поэтому в таком случае покинем метод
                if (resp1url == null) return;
                // делаем второй запрос
                _session.Get(resp1url);
                // он также отстрельнит редирект с ещё одним редиректом
                // запомним и этот редирект
                string resp2url = _session.GetLocationValue();
                // сделаем во второй редирект запрос
                _session.Get(resp2url);
                // он отстрельнит страницу авторизации из которой необходимо вытащить
                // переменную(1) которую распарсим их html'ки из запроса(2) из которой сделаем строку для атворизации(3)
                // и затем закинем ещё запрос на авторизацию на второй редирект(4)
                _session.Post/*4*/(resp2url, /*3*/GetAuthString(/*2*/HtmlParser.HtmlTo_idsrv_xsrf_String(/*1*/_session.ResponseData)));
                // затем закидываем post на первый редирект
                _session.Get(resp1url);
                // оттуда распарсим строку с тоекнами авторизации
                _loginToken = HtmlParser.HtmlToTokenString(_session.ResponseData);
                // и просто закидываем токен чтобы наша сессии регнулась в системе moipass
                _session.Post(ConfigManager.Parameters.MainUrl, _loginToken);
            }
            catch (Exception ex)
            {
                Logger.Log("EXCEPTION_Message: " + ex.Message);
                Logger.Log("EXCEPTION_StackTrace: " + ex.StackTrace);
                Logger.Log("EXCEPTION_Source: " + ex.Source);
                MessageBox.Show(ex.Message);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private string GetLookupString(SearchData data)
     => $@"PageCount=100&Page=1&OrderCode={data.OrderCode}&OrderId={data.IdOrder}&Phone={data.Phone}&Email={data.Email}&DateStart={data.DateStart}&DateEnd={data.DateEnd}&TimeStart=&TimeEnd=&OrderSum=";
        ///////////////////////////////////////////////////////////////////////////////////
        private string GetAuthString(string idsrv_xsrf)
             => $@"SiteId=&OrganizationId=&idsrv.xsrf={idsrv_xsrf}&username={Uri.EscapeDataString(ConfigManager.Parameters.User)}&password={Uri.EscapeDataString(ConfigManager.Parameters.Password)}";
        ///////////////////////////////////////////////////////////////////////////////////
    }
}
