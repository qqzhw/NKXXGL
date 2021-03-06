using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
 
using System.Windows;

namespace ICIMS.Launcher.AutoUpdater
{
   
    public delegate void ShowHandler();
    

    public class AutoUpdater : IAutoUpdater
    {
       
        private ClientConfig config = null;
        private bool bNeedRestart = false;
        private bool bDownload = false;
        List<DownloadFileInfo> downloadFileListTemp = null;        
         
        public AutoUpdater()
        {
           // Init();
            config = ClientConfig.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFileInfo.FILENAME));
        }

        private void Init()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdater.config.txt";
            try
            {
                if (File.Exists(fileName))
                {
                    string tmpFile = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdater.config";
                    if (File.Exists(tmpFile))
                        File.Delete(tmpFile);
                    File.Move(fileName, "AutoUpdater.config");
                    File.Delete(fileName);
                }
            }
            catch
            {
            }
            
        }
       

      
        public void Update()
        {
            if (config==null)
            {
                return;
            }
            Dictionary<string, RemoteFile> listRemotFile = ParseRemoteXml(config.ServerUrl);

            List<DownloadFileInfo> downloadList = new List<DownloadFileInfo>();

            foreach (LocalFile file in config.UpdateFileList)
            {
                if (listRemotFile.ContainsKey(file.Path))
                {
                    RemoteFile rf = listRemotFile[file.Path];
                    Version v1 = new Version(rf.LastVer);
                    Version v2 = new Version(file.LastVer);
                    if (v1 > v2)
                    {
                        downloadList.Add(new DownloadFileInfo(rf.Url, file.Path, rf.LastVer, rf.Size));
                        file.LastVer = rf.LastVer;
                        file.Size = rf.Size;

                        if (rf.NeedRestart)
                            bNeedRestart = true;

                        bDownload = true;
                    }

                    listRemotFile.Remove(file.Path);
                }
            }

            foreach (RemoteFile file in listRemotFile.Values)
            {
                downloadList.Add(new DownloadFileInfo(file.Url, file.Path, file.LastVer, file.Size));
                LocalFile localFile = new LocalFile();
                localFile.LastVer = file.LastVer;
                localFile.Path = file.Path;
                localFile.Size = file.Size;
                config.UpdateFileList.Add(localFile);
                if (file.NeedRestart)
                    bNeedRestart = true;
                bDownload = true;
            }

            downloadFileListTemp = downloadList;

            if (bDownload)
            {
                DownloadConfirm dc = new DownloadConfirm(downloadList); 
                if (dc.ShowDialog()==true)
                {
                    if (dc.chkupdateAll.IsChecked==true)
                    {
                        string url = config.ServerAddress + "setup.exe";
                        string filename = "setup.exe";
                       
                        DownloadFileInfo downloadinfo = new DownloadFileInfo(url, filename, string.Empty, 0);
                        StartDownload(downloadinfo);
                    }
                    else
                    { 
                        StartDownload(downloadList);
                       
                    }
                   
                }
                else
                {
                    App.StartMainProgram(); 
                }
            }
        }

        public void RollBack()
        {
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                string oldPath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                    }

                    if (oldPath.EndsWith("_"))
                        oldPath = oldPath.Substring(0, oldPath.Length - 1);

                    MoveFolderToOld(oldPath + ".old", oldPath);

                }
                catch
                {
                    
                }
            }
        }


       
        string newfilepath = string.Empty;
        private void MoveFolderToOld(string oldPath, string newPath)
        {
            if (File.Exists(oldPath) && File.Exists(newPath))
            {
                System.IO.File.Copy(oldPath, newPath, true);
            }
        }

        private void StartDownload(List<DownloadFileInfo> downloadList)
        {
            MainWindow dp = new MainWindow(downloadList);
            if (dp.ShowDialog()==true)
            { 
                //更新成功
                config.SaveConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFileInfo.FILENAME));

                if (bNeedRestart)
                {
                    //删除临时文件夹
                    Directory.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFileInfo.TEMPFOLDERNAME), true);

                   // MessageBox.Show(ConstFileInfo.APPLYTHEUPDATE, ConstFileInfo.MESSAGETITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                    CommonUnitity.RestartApplication();
                }
            }
        }
        private void StartDownload(DownloadFileInfo downloadfileinfo)
        {
            UpdateWindow dp = new UpdateWindow(downloadfileinfo);
            if (dp.ShowDialog() == true)
            {
                //更新成功
                string folder=Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFileInfo.TEMPFOLDERNAME);
                string filePath=Path.Combine(folder,"setup.exe");
                using (Process process = new Process())
                {
                    process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    process.StartInfo.FileName =filePath;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.Verb = "RunAs";
                    try
                    {
                        process.Start();
                       bool f= process.WaitForExit(1);
                      //  Directory.Delete(folder, true);
                    }
                    catch
                    {
                        Directory.Delete(folder, true);
                    }
                    finally
                    {
                    
                    }

                }
                    //删除临时文件夹
               //  Directory.Delete(folder, true);

                    // MessageBox.Show(ConstFileInfo.APPLYTHEUPDATE, ConstFileInfo.MESSAGETITLE, MessageBoxButton.OK, MessageBoxImage.Information);
               //     CommonUnitity.RestartApplication();
                 
            }
        }

       


        private Dictionary<string, RemoteFile> ParseRemoteXml(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xml);

            Dictionary<string, RemoteFile> list = new Dictionary<string, RemoteFile>();
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                list.Add(node.Attributes["path"].Value, new RemoteFile(node));
            }

            return list;
        }



        public bool IsHasUpdate()
        {
            if (config==null)
            {
                return false;
            }
            if (!config.Enabled)
                return false; 
            Dictionary<string, RemoteFile> lstRemotFile = ParseRemoteXml(config.ServerUrl);  
            foreach (LocalFile file in config.UpdateFileList)
            {
                if (lstRemotFile.ContainsKey(file.Path))
                {
                    RemoteFile rf = lstRemotFile[file.Path];
                    Version v1 = new Version(rf.LastVer);
                    Version v2 = new Version(file.LastVer);
                    if (v1 > v2)
                    {
                        return true;
                    }
                    lstRemotFile.Remove(file.Path);
                }
               
            }

            if (lstRemotFile.Count > 0)
                return true;
           
            return false;
            
        }
    }

}
