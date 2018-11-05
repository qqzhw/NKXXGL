
using System;
using System.Diagnostics;
using System.Reflection;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"Log4Net\log4Net.config", Watch = true)]

namespace ICIMS.Core.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// SError
        /// </summary>
        private static string mSError = "ERROR";

        /// <summary>
        /// SDebug
        /// </summary>
        private static string mSDebug = "DEBUG";

        /// <summary>
        /// SInfo
        /// </summary>
        private static string mSInfo = "INFO";

        /// <summary>
        /// SWarn
        /// </summary>
        private static string mSWarn = "WARN";

        /// <summary>
        /// SFatal
        /// </summary>
        private static string mSFatal = "FATAL";

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Debug(object message)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.Debug(message);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public static void Debug(object message, Exception exception)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.Debug(message, exception);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg">消息</param>
        public static void DebugFormat(string format, object arg)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.DebugFormat(format, arg);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void DebugFormat(string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.DebugFormat(format, args);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="provider">格式提供者</param>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.DebugFormat(format, args);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">信息1</param>
        /// <param name="arg1">信息2</param>
        public static void DebugFormat(string format, object arg0, object arg1)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.DebugFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">信息1</param>
        /// <param name="arg1">信息2</param>
        /// <param name="arg2">信息3</param>
        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            log4net.ILog log = GetLogger(mSDebug);
            log.DebugFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Error(object message)
        {
            log4net.ILog log = GetLogger(mSError);
            log.Error(message);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="exception">异常</param>
        public static void Error(object message, Exception exception)
        {
            log4net.ILog log = GetLogger(mSError);
            log.Error(message, exception);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息</param>
        public static void ErrorFormat(string format, object arg0)
        {
            log4net.ILog log = GetLogger(mSError);
            log.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSError);
            log.ErrorFormat(format, args);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="provider">提供者</param>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSError);
            log.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            log4net.ILog log = GetLogger(mSError);
            log.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        /// <param name="arg2">消息3</param>
        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            log4net.ILog log = GetLogger(mSError);
            log.ErrorFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Fatal(object message)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.Fatal(message);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public static void Fatal(object message, Exception exception)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.Fatal(message, exception);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息</param>
        public static void FatalFormat(string format, object arg0)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.FatalFormat(format, arg0);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">信息</param>
        public static void FatalFormat(string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.FatalFormat(format, args);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="provider">提供者</param>
        /// <param name="format">格式</param>
        /// <param name="args">信息</param>
        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        public static void FatalFormat(string format, object arg0, object arg1)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.FatalFormat(format, format, arg0, arg1);
        }

        /// <summary>
        /// 记录严重错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        /// <param name="arg2">消息3</param>
        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            log4net.ILog log = GetLogger(mSFatal);
            log.FatalFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="message">信息</param>
        public static void Info(object message)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.Info(message);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public static void Info(object message, Exception exception)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.Info(message, exception);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息</param>
        public static void InfoFormat(string format, object arg0)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.InfoFormat(format, arg0);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void InfoFormat(string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.InfoFormat(format, args);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="provider">提供者</param>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        public static void InfoFormat(string format, object arg0, object arg1)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        /// <param name="arg2">消息3</param>
        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            log4net.ILog log = GetLogger(mSInfo);
            log.InfoFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="message">消息</param>
        public static void Warn(object message)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.Warn(message);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public static void Warn(object message, Exception exception)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.Warn(message, exception);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息</param>
        public static void WarnFormat(string format, object arg0)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.WarnFormat(format, arg0);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void WarnFormat(string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.WarnFormat(format, args);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="provider">提供者</param>
        /// <param name="format">格式</param>
        /// <param name="args">消息</param>
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.WarnFormat(provider, format, args);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        public static void WarnFormat(string format, object arg0, object arg1)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">消息1</param>
        /// <param name="arg1">消息2</param>
        /// <param name="arg2">消息3</param>
        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            log4net.ILog log = GetLogger(mSWarn);
            log.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>Log对象</returns>
        private static log4net.ILog GetLogger(string name)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(name);
            return log;
        }
    }
}
