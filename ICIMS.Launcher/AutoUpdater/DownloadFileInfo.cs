using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ICIMS.Launcher.AutoUpdater
{
    public class DownloadFileInfo
    {
        #region 字段声明
        string downloadUrl = string.Empty;
        string fileName = string.Empty;
        string lastver = string.Empty;
        int size = 0;
       
        public string DownloadUrl { get { return downloadUrl; } }
        public string FileFullName { get { return fileName; } }
        public string FileName { get { return Path.GetFileName(FileFullName); } }
        public string LastVer { get { return lastver; } set { lastver = value; } }
        public int Size { get { return size; } }
        #endregion

      
        public DownloadFileInfo(string url, string name, string ver, int size)
        {
            this.downloadUrl = url;
            this.fileName = name;
            this.lastver = ver;
            this.size = size;
        }
       
    }
}
