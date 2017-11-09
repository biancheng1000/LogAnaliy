using InkeverCommon;
using LogAnalyst.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using LogAnalyst.Model;
using System.Windows;

namespace LogAnalyst.Cmd
{
    /// <summary>
    /// 从文件中加载日志数据
    /// </summary>
    public class LoadLogCmd:baseCmd
    {
        LogAnalystVm vm;
        public LoadLogCmd(LogAnalystVm _vm)
        {
            vm = _vm;
        }

        public override void Execute(object parameter)
        {
            if (vm == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(vm.LogFileName))
            {
                return;
            }

            if (File.Exists(vm.LogFileName))
            {

                PreviewProcess();
    
                LoadResult();
            }
        }


        /// <summary>
        /// 对大的log日志进行预先处理，分割
        /// </summary>
        private bool PreviewProcess()
        {
            FileInfo f = new FileInfo(vm.LogFileName);
            string newfileName = vm.LogFileName;
            if (f.Length > 100 * 1024 * 1024)
            {
                if (MessageBox.Show("文件超过了100M，是否进行分割？", "提问", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //按照10M大小进行文件分割
                    FileStream fs = new FileStream(vm.LogFileName, FileMode.Open);
                    byte[] temp = new byte[100 * 1024 * 1024];
                    int index = 1;
                    while (fs.Position < fs.Length)
                    {
                        if (fs.Length - fs.Position < temp.Length)
                        {
                            temp = new byte[fs.Length - fs.Position];
                        }
                        else
                        {
                            fs.Read(temp, 0, temp.Length);
                        }

                        //fs.Position += temp.Length;

                        newfileName = vm.LogFileName.Replace(".txt", "_"+index + ".txt");
                        using (FileStream sfs = new FileStream(newfileName, FileMode.CreateNew))
                        {
                            sfs.Write(temp, 0, temp.Length);
                            sfs.Flush();
                            sfs.Close();
                        }
                        index++;
                    }
                    vm.LogFileName = newfileName;
                    return true;
                }
            }
            return false;
        }


        public async void LoadResult()
        {
            vm.Status = "正在加载中";
            int logCount = await LoadAsync();
            
            vm.Status = "一共加载了:"+logCount+"条日志";
            if (logCount > 0)
            {
                vm.Status = "数据加载完成";
            }
        }

        private  Task<int> LoadAsync()
        {
          return Task.Run<int>(() =>
            {
                int icount = 0;
                using (StreamReader reader = new StreamReader(vm.LogFileName, Encoding.UTF8))
                {
                    IList<MemLog> temps = new List<MemLog>();
                    while (!reader.EndOfStream)
                    {
                        string text = reader.ReadLine();
                        MemLog log = new MemLog();
                        Match matchResult = Regex.Match(text, "\\[([^\\]]+)\\]\\[([^\\]]+)\\]\\[([^\\]]+)\\](.*)");
                        if (matchResult.Success)
                        {
                            if (matchResult.Groups.Count == 5)
                            {
                                log.LogTime = matchResult.Groups[1].Value;
                                log.LogLevel = (LogTextLevel)Enum.Parse(typeof(LogTextLevel), matchResult.Groups[2].Value);
                                log.LogSrcName = matchResult.Groups[3].Value;
                                log.LogDesc = matchResult.Groups[4].Value;
                                if (vm.FilterDate.HasValue)
                                {
                                    if (DateTime.Parse(log.LogTime) >= new DateTime(2017, 11, 7))
                                    {
                                        temps.Add(log);
                                        icount++;
                                    }
                                }
                                else
                                {
                                    temps.Add(log);
                                    icount++;
                                }
                               
                            }
                            else
                            {
                                Console.WriteLine("异常数据："+text);
                            }
                        }

                    
                    }
                    App.Current.Dispatcher.Invoke(() => { vm.Logs = new Comm.ViewableCollection<MemLog>(temps); });
                }
                return icount;
            });
        }
    }
}
