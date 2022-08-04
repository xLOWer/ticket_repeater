using System.Collections.Generic;
using System.IO;

namespace Отправить_билеты
{
    public static class Logger
    {
        private static List<string> logVar = new List<string>();
        private const string _logFileName = "log.txt";

        static Logger()
        {
            if (File.Exists(_logFileName))            
                File.Delete(_logFileName);            
        }

        /// <summary>
        /// Логирует событие в файл
        /// </summary>
        /// <param name="data"></param>
        public static void Log(string data)
        {
            logVar.Add(data);
            File.WriteAllLines(_logFileName, logVar);
        }
    }
}
