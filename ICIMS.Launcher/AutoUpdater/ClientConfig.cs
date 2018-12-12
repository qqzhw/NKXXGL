using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ICIMS.Launcher.AutoUpdater
{
    public class ClientConfig
    {
        #region 字段声明
        private bool enabled = true;
        private string serverUrl = string.Empty;
        private string serverAddress = string.Empty;
        private UpdateFileList updateFileList = new UpdateFileList();
        #endregion

        #region 属性
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public string ServerAddress
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }
        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }
       
        public UpdateFileList UpdateFileList
        {
            get { return updateFileList; }
            set { updateFileList = value; }
        }
        #endregion

        #region 方法
        public static ClientConfig LoadConfig(string file)
        {
            try
            { 
                XmlSerializer xs = new XmlSerializer(typeof(ClientConfig));
                StreamReader sr = new StreamReader(file);
                ClientConfig config = xs.Deserialize(sr) as ClientConfig;
                sr.Close();

                return config;
            }
            catch (Exception ex)
            { 
                return null;
            }
          
        }

        public void SaveConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ClientConfig));
            StreamWriter sw = new StreamWriter(file);
            xs.Serialize(sw, this);
            sw.Close();
        }
        #endregion
    }

}
