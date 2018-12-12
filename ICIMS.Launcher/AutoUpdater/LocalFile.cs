using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ICIMS.Launcher.AutoUpdater
{
    public class LocalFile
    {
        #region 本地文件
        private string path = "";
        private string lastver = "";
        private int size = 0;
        #endregion

       
        [XmlAttribute("path")]
        public string Path { get { return path; } set { path = value; } }
        [XmlAttribute("lastver")]
        public string LastVer { get { return lastver; } set { lastver = value; } }
        [XmlAttribute("size")]
        public int Size { get { return size; } set { size = value; } }
        
         
        public LocalFile(string path, string ver, int size)
        {
            this.path = path;
            this.lastver = ver;
            this.size = size;
        }

        public LocalFile()
        {
        }
    

    }
}
