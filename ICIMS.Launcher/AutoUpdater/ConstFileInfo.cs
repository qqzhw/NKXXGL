using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICIMS.Launcher.AutoUpdater
{
    public class ConstFileInfo
    {
        public const string TEMPFOLDERNAME = "TempFolder";
        public const string CONFIGFILEKEY = "config_";
        public const string FILENAME = "AutoUpdater.config";
        public const string ROOLBACKFILE = "Pvirtech.Commander.Launcher.exe";
        public const string MESSAGETITLE = "内控信息化管理系统";
        public const string CANCELORNOT = "客户端正在更新。你确定要取消吗?";
        public const string APPLYTHEUPDATE = "程序需要重新启动应用更新，请单击“确定”重新启动程序！";
        public const string NOTNETWORK = "客户端更新失败，即将重新启动。请尝试再次更新 。";
    }
}
