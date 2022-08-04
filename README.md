# ticket_repeater
Приложение для отправки билетов забывчивым посетителям океанариума
1.	Терминология
1.1.	Определения к используемым терминам можно узнать из источников:
1.1.1.	ГОСТ 27201-87 «Машины вычислительные электронные персональные»
1.1.2.	ГОСТ 19781-90 «Обеспечение систем обработки информации программное»
1.1.3.	ГОСТ 28397-89 «Языки программирования»
1.2.	Moipass – портал для работы с клиентами и билетами от ISD.
2.	Цели и задачи приложения
2.1.	Проблематика, – посетители покупающие билеты часто приходят без распечатанных электронных билетов, в связи с чем у администраторов отдела по работе с посетителями возникает необходимость тратить время на составление заявки в отдел управления информационными системами для отправки билета на почту сотрудников через портал moipass для дальнейшей распечатки билета и выдачи его посетителю. На это тратится время сотрудников, а посетителей складывается негативное впечатление о «Приморском океанариуме»
2.2.	Цель – обеспечить скорость и качество работы с посетителями сотрудникам отдела по работе с посетителями
2.3.	Задача – сократить число действий и сократить время получения электронных билетов при помощи автоматизации процесса отправки билета через приложение.
3.	Техническое описание
3.1.	Приложение, написанное с применением следующих технологий:
3.1.1.	Язык программирования – С#
3.1.2.	Тип проекта – приложение WPF
3.1.3.	Разметка графического интерфейса – XAML
3.1.4.	Visual Studio Community 2019
3.1.5.	Свободно распространяемые библиотеки (менеджер NuGet)
3.1.5.1.	HtmlAgilityPack – парсит html для выборки данных
3.1.5.2.	Newtonsoft.Json – кодирует и декодирует json
3.2.	Системные требования
3.2.1.	Microsoft Visual Redist 2017 и новее
3.2.2.	Microsoft .NET Framework 4.7.2 и новее
3.2.3.	Не менее 1 Гб ОЗУ
3.2.4.	Интернет-соединение
3.3.	Состав скомпилированного приложения
3.3.1.	Файл «Newtonsoft.Json.dll» – соответствующая названию библиотека
3.3.2.	Файл «HtmlAgilityPack.dll» – соответствующая названию библиотека
3.3.3.	Файл «Отправить билеты.exe» – исполняемый файл, приложение
3.3.4.	Файл «conf» – файл с настройками в формате JSON
4.	Установка приложения
4.1.	Приложение поставляется в архиве zip
4.2.	Файла «conf» может не быть в составе приложения, но он создаётся со стандартными настройками при первом обращении к конфигурации
4.3.	Порядок установки приложения:
4.3.1.	Распаковать файлы из архива с приложением в любую папку
4.3.2.	Создать ярлык на рабочем столе
4.3.3.	Файла «conf» 
4.3.4.	При первом запуске проверить конфигурацию во вкладке «Настройки»
4.3.5.	Ввести необходимые параметры во вкладке «Настройки»
5.	Функционал приложения
5.1.	Описание функционала вкладки «Главная»
5.1.1.	Вкладка содержит поля, кнопки и таблицу для поиска билетов
5.1.2.	Описание текстовых полей:
5.1.2.1.	«E-Mail» – содержащее любую часть электронной почты клиента. Например, если необходимо найти пользователя с адресом user123@mail.ru, то можнно вводить любые комбинации: user1, user123, user, 123@mail.ru итд 
5.1.2.2.	«Id Заказа» – содержит номер id заказа
5.1.2.3.	«Дата с» – содержащее информацию с какой даты начать поиск, ищет дату покупки билета. Можно вручную вводить цифры даты
5.1.2.4.	«Код заказа» – содержит номер кода заказа из билета
5.1.2.5.	«Номер тел.» – содержит телефон клиента при покупке билета
5.1.2.6.	«Дата по» – содержащее информацию по какую дату искать
5.1.3.	Описание кнопок управления:
5.1.3.1.	Кнопка «Найти» начинает поиск по введённым параметрам поиска формируя запросы для отправки на удалённый сервер (Москва), поэтому может занять некоторое время или даже не сработать, о чём выведет ошибку. После успешного поиска выведет в таблицу всю найденную информацию
5.1.3.2.	Кнопка «Отправить» после выбора в таблицу нужного билета отправляет на указанный в настройках адрес эл. почты письмо с данными билета клиента. Операция также удалённая (Москва) и может занять некоторое время (обычно от 10 сек до 2 минут), но при сбое на стороне сервера может выдать ошибку, при этом неизвестно отправится ли билет или нет. При успешном завершении появится всплывающее уведомление. Провалившуюся операцию необходимо повторить до появления уведомления об успехе.
5.2.	Описание вкладки «Настройки»
5.2.1.	Содержит настройки, необходимые для работы приложения
5.2.2.	Описание текстовых полей:
5.2.2.1.	«Пользователь» - содержит логин пользователя в системе moipass
5.2.2.2.	«Пароль» - содержит пароль пользователя
5.2.2.3.	«Куда слать» - содержит адрес эл. почты, на который отсылаются билеты
5.2.2.4.	Остальные поля с адресами отвечают за служебные функции, трогать их не требуется, они на случай, если когда-то произойдут изменения в адресах
5.2.3.	Описание кнопок управления:
5.2.3.1.	Кнопка «Сохранить настройки» выполняет запись введённых в текстовые поля на вкладке «Настройки» данных в файл из 
5.2.3.2.	Кнопка «Загрузить настройки» выполняет загрузку настроек в текстовые поля из файла из п.4.3.3. и если файл не обнаруживается, то создаётся файл с настройками по-умолчанию со всеми служебными адресами и без логина и пароля пользователя
6.	Использование приложения
6.1.	Запуск приложения происходит путём двойного нажатия на ярлык, если он был создан, или исполняемый файл
6.2.	Если запуск был первым
6.2.1.	Создастся первичная конфигурация
6.2.2.	Пройти на вкладку «Настройки»
6.2.3.	Ввести данные пользователя системы moipass
6.2.4.	Если данные не отображаются закрыть приложение и открыть снова
6.3.	Перейти во вкладку «Главная»
6.4.	При типичной эксплуатации приложения используется только поле «Дата с» которая установлена на 7 дней назад от текущего, поле «Дата по» которое установлено на 7 дней вперед от текущего, поле «E-mail» клиента, купившего билет
6.5.	В полях с выбором дат выпадает окно с календарём, а также дату можно вводить вручную
6.6.	Нажать кнопку «Поиск» и подождать согласно п.5.1.3.1.
6.7.	Выбрать билет, который необходимо отправить
6.8.	Нажать кнопку «Отправить» и ожидать согласно п.5.1.3.2.
