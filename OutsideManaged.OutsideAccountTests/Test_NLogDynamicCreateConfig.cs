using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OutsideManaged.OutsideAccountTests
{
    /// <summary>
    /// 測試 NLog 動態產生 NLog.Config 的內容與相關資訊
    /// </summary>
    [TestClass]
    public class Test_NLogDynamicCreateConfig
    {
        [TestMethod]
        public void Test_WriteExceptionLog()
        {
            // Arrange
            string errMessage = "這是一個測試的錯誤訊息........";
            DateTime _startTime;
            long _startMilliSeconds;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Reset();
            sw.Start();

            _startTime = DateTime.Now;
            _startMilliSeconds = sw.ElapsedMilliseconds;

            string baseDirectory = AppContext.BaseDirectory;
            var config = new NLog.Config.LoggingConfiguration();

            var errLogfile = new NLog.Targets.FileTarget("errLogfile") 
            { 
                FileName = $"{baseDirectory}/logs/{DateTime.Now.ToString("yyyy-MM-dd")}_OUTSIDE_ERROR.log" 
            };

            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
          
            config.AddRule(LogLevel.Error, LogLevel.Fatal, errLogfile);
            config.AddRule(LogLevel.Debug, LogLevel.Debug, logconsole);

            // 套用 NLog configuration
            NLog.LogManager.Configuration = config;
            Logger _logger = LogManager.GetCurrentClassLogger();

            // Act
            _logger.Error($" UnitTest [來源:file://{baseDirectory} 執行 {nameof(Test_NLogDynamicCreateConfig)} [錯誤訊息:{errMessage}]");

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_WriteLogInfo()
        {
            // Arrange
            DateTime _startTime;
            long _startMilliSeconds;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Reset();
            sw.Start();

            _startTime = DateTime.Now;
            _startMilliSeconds = sw.ElapsedMilliseconds;

            string baseDirectory = AppContext.BaseDirectory;
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") 
            { 
                FileName = $"{baseDirectory}/logs/{DateTime.Now.ToString("yyyy-MM-dd")}_OUTSIDE.log" 
            };

            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Info, logfile);
            config.AddRule(LogLevel.Debug, LogLevel.Debug, logconsole);

            // 套用 NLog configuration
            NLog.LogManager.Configuration = config;
            Logger _logger = LogManager.GetCurrentClassLogger();

            // Act
            _logger.Info($" UnitTest [來源:file://{baseDirectory} 執行 {nameof(Test_NLogDynamicCreateConfig)} [開始時間:{_startTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}] [結束時間:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] [花費時間:{sw.ElapsedMilliseconds - _startMilliSeconds}]");

            // Assert
            Assert.IsTrue(true);
        }
    }
}
