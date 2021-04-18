using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Xml;

namespace Core
{
    public class Logger
    {
        public static readonly Logger Instance = new Logger();
        private readonly ILog _log;

        private Logger()
        {
            XmlDocument log4NetConfig = new XmlDocument();
            log4NetConfig.Load(File.OpenRead("log4net.config"));
            ILoggerRepository repo = LogManager.CreateRepository(typeof(Logger).Assembly, typeof(Hierarchy));
            XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);
            _log = LogManager.GetLogger(GetType());
        }

        public void Info(string message)
        {
            _log.Info("[INFO] " + message);
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Error(string message)
        {
            _log.Error("[ERROR] " + message);
        }

        public void Error(Exception e)
        {
            _log.Error("[ERROR] " + $"{e.Message}. StackTrace:\n{e.StackTrace}");
        }

        public void Warn(Exception e)
        {
            _log.Warn("[WARN] " + $"{e.Message}. StackTrace:\n{e.StackTrace}");
        }

        public void Warn(string message)
        {
            _log.Error("[WARN] " + message);
        }
    }
}