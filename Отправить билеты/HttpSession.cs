using System;
using System.IO;
using System.Net;

namespace Отправить_билеты
{
    public class HttpSession
    {
        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private string _requestData;
        private string _responseData;

        public string ResponseData
        {
            get => _responseData;
            private set {
                if (value != null) 
                    _responseData = value;
            }
        }
        private CookieContainer _cookieContainer;

        public HttpSession()
        {
            _cookieContainer = new CookieContainer();
            _request = HttpWebRequest.CreateHttp("http://leningrad.ru");
        }

        /// <summary>
        /// Получает адрес редиректа из последнего успешного запроса
        /// </summary>
        /// <returns></returns>
        public string GetLocationValue() => _response?.Headers[HttpResponseHeader.Location];

        /// <summary>
        /// Отправляет HTTP POST запрос на указанный адрес
        /// </summary>
        /// <param name="URL">Адрес запроса</param>
        public void Post(string URL)
        {
            CreateRequest(URL, WebRequestMethods.Http.Post, true, false);
            _request.ContentLength = 0;
            _response = (HttpWebResponse)_request.GetResponse();
            _responseData = GetResponseContent(_response);
            if(ConfigManager.Parameters.IsDebugging) 
                LogRequest();
        }

        /// <summary>
        /// Отправляет HTTP POST запрос на указанный адрес с данными
        /// </summary>
        /// <param name="URL">Адрес запроса</param>
        /// <param name="data">Отправляемые данные</param>
        /// <param name="redirect">использовать ли редиректы</param>
        public void Post(string URL, string data, bool redirect = false)
        {
            CreateRequest(URL, WebRequestMethods.Http.Post, true, redirect);
            _request.ContentType = "application/x-www-form-urlencoded";
            _requestData = data;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            _request.ContentLength = byteArray.Length;
            using (Stream dataStream = _request.GetRequestStream())            
                dataStream.Write(byteArray, 0, byteArray.Length);            
            _response = (HttpWebResponse)_request.GetResponse();
            _responseData = GetResponseContent(_response);
            if (ConfigManager.Parameters.IsDebugging) LogRequest();
        }

        /// <summary>
        /// Отправляет HTTP GET запрос на указанный адрес
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="redirect"></param>
        public void Get(string URL, bool redirect = false)
        {
            CreateRequest(URL, WebRequestMethods.Http.Get, true, redirect);
            _response = (HttpWebResponse)_request.GetResponse();
            _responseData = GetResponseContent(_response);
            if (ConfigManager.Parameters.IsDebugging) 
                LogRequest();
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private void CreateRequest(string URL, string method, bool alive, bool redirect)
        {
            _request = HttpWebRequest.CreateHttp(new Uri(URL));
            _request.Method = method;
            _request.KeepAlive = alive;
            _request.AllowAutoRedirect = redirect;
            _request.CookieContainer = _cookieContainer;
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private string GetResponseContent(HttpWebResponse response)
        {
            string content = "";
            try
            {
                using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        content = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return content;
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private void LogError(Exception ex)
        {
            Logger.Log($@"##############################################################
GetResponseContent Exception: {ex.Message}
{(ex?.InnerException)}
{ex.StackTrace}
##############################################################");
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private void LogRequest()
        {
            string method = _request?.Method ?? "",
                requrl = _request?.RequestUri.ToString() ?? "",
                reqheaders = GetHeaders(_request.Headers),
                reqcontent = !string.IsNullOrEmpty(_requestData) ? _requestData : "",

                code = _response?.StatusCode.ToString() ?? "",
                resurl = _response?.ResponseUri.ToString() ?? "",
                resheaders = GetHeaders(_response.Headers) ?? "",
                rescontent = _responseData ?? "";

            Logger.Log($@"### request
{method} {requrl}
{reqheaders}
{reqcontent}
### response
{code} {resurl}
{resheaders}
{rescontent}");
            _requestData = "";
        }
        ///////////////////////////////////////////////////////////////////////////////////
        private string GetHeaders(WebHeaderCollection Headers)
        {
            string values = "";
            foreach (var header in Headers.AllKeys)
                values += header + " : " + Headers[header] + "\r\n";
            return values;
        }
    }
}
