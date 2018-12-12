using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ICIMS.Launcher.AutoUpdater
{ 
    public class RemoteFile
    {
        #region  服务端xml文件配置字段
        private string path = "";
        private string url = "";
        private string lastver = "";
        private int size = 0;
        private bool needRestart = false;
        #endregion

        
        /// <summary>
        /// 程序名称/dll名称
        /// </summary>
        public string Path { get { return path; } }
        
        /// <summary>
        /// 详细url
        /// </summary>
        public string Url { get { return url; } }
        
        /// <summary>
        /// 最新版本号
        /// </summary>
        public string LastVer { get { return lastver; } }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int Size { get { return size; } }
        
        /// <summary>
        /// 是否需要重新启动
        /// </summary>
        public bool NeedRestart { get { return needRestart; } }
       

     
        public RemoteFile(XmlNode node)
        {
            this.path = node.Attributes["path"].Value;
            this.url = node.Attributes["url"].Value;
            this.lastver = node.Attributes["lastver"].Value;
            this.size = Convert.ToInt32(node.Attributes["size"].Value);
            this.needRestart = Convert.ToBoolean(node.Attributes["needRestart"].Value);
        }
        
    }
}
