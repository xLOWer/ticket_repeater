using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Отправить_билеты
{
    public static class HtmlParser
    {
        /// <summary>
        /// Парсит переменную idsrv_xsrf из указанного html-текста
        /// </summary>
        /// <param name="html">html-текст</param>
        /// <returns></returns>
        public static string HtmlTo_idsrv_xsrf_String(string html)
        {
            HtmlDocument _doc1 = new HtmlDocument();
            _doc1.LoadHtml(html);
            return _doc1?.GetElementbyId("login-form")
                ?.ChildNodes?.Single(x => x.Name == "input" && string.IsNullOrEmpty(x.Id))
                ?.Attributes?.Single(x => x.Name == "value")
                ?.Value ?? "";
        }

        /// <summary>
        /// Парсит данные для токена авторизации из указанного html-текста
        /// </summary>
        /// <param name="html">html-текст</param>
        /// <returns></returns>
        public static string HtmlToTokenString(string html)
        {
            string
                code = "",
                id_token = "",
                scope = "",
                state = "",
                session_state = "";
            HtmlDocument _doc1 = new HtmlDocument();
            _doc1.LoadHtml(html);

            HtmlNodeCollection inputs = _doc1.DocumentNode.SelectNodes("//input"); // дёргаем все теги input
            // и имеем горячий секс с каждым по очереди:
            // (1) ищем конкретную ноду: чтобы был атрибут с именем "name" и он был определённого значения
            // (2) из конкретной ноды дёргаем атрибут с именем "value"
            // (3) берём из этого атрибута значение (оно и нужно)
            // (4) а если чтото пошло не так на одном из шагов, то значение вернётся как пустая строка
            code = (inputs?.Single(x => x.Attributes.Single(a => a.Name == "name").Value == nameof(code)/*(1)*/)
                ?.Attributes?.Single(x => x.Name == "value")/*(2)*/
                ?.Value/*(3)*/) ?? ""/*(4)*/;
            // далее по аналогии (** хотя можно было бы ебануть и метод, но лень)
            id_token = (inputs?.Single(x => x.Attributes.Single(a => a.Name == "name").Value == nameof(id_token))
                ?.Attributes?.Single(x => x.Name == "value")
                ?.Value) ?? "";
            scope = (inputs?.Single(x => x.Attributes.Single(a => a.Name == "name").Value == nameof(scope))
                ?.Attributes?.Single(x => x.Name == "value")
                ?.Value) ?? "";
            state = (inputs?.Single(x => x.Attributes.Single(a => a.Name == "name").Value == nameof(state))
                ?.Attributes?.Single(x => x.Name == "value")
                ?.Value) ?? "";
            session_state = (inputs?.Single(x => x.Attributes.Single(a => a.Name == "name").Value == nameof(session_state))
                ?.Attributes?.Single(x => x.Name == "value")
                ?.Value) ?? "";
            // компонуем всё в ответ и выкидываем в мир
            return $@"code={code}&id_token={id_token}&scope={scope}&state={state}&session_state={session_state}";
        }

        /// <summary>
        /// Парсит список данных о билетах посетителей
        /// </summary>
        /// <param name="html">html-текст</param>
        /// <returns></returns>
        public static List<SearchResult> ProcessList(string html)
        {
            List<SearchResult> result = new List<SearchResult>();
            HtmlDocument _doc1 = new HtmlDocument();
            _doc1.LoadHtml(html);
            HtmlNodeCollection table_rows = _doc1.DocumentNode.SelectNodes("//table[@id='orders_table']/tbody/tr");
            if (table_rows.Count > 0)
                foreach (HtmlNode row in table_rows)
                    result.Add(new SearchResult()
                    {
                        Id = HtmlEntity.DeEntitize(row?.SelectNodes("./td[1]")?.SingleOrDefault()?.InnerText ?? ""),
                        OrderCode = HtmlEntity.DeEntitize(row?.SelectNodes("./td[2]")?.SingleOrDefault()?.InnerText ?? ""),
                        User = HtmlEntity.DeEntitize(row?.SelectNodes("./td[3]")?.SingleOrDefault()?.InnerText ?? ""),
                        Site = HtmlEntity.DeEntitize(row?.SelectNodes("./td[4]")?.SingleOrDefault()?.InnerText ?? ""),
                        RegestrationDate = HtmlEntity.DeEntitize(row?.SelectNodes("./td[5]")?.SingleOrDefault()?.InnerText ?? ""),
                        Status = HtmlEntity.DeEntitize(row?.SelectNodes("./td[6]/span")?.SingleOrDefault()?.InnerText ?? ""),
                        Summa = HtmlEntity.DeEntitize(row?.SelectNodes("./td[7]")?.SingleOrDefault()?.InnerText ?? "")
                    });
            return result;
        }
    }
}
