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
                LoadResult();
            }
        }

        public async void LoadResult()
        {
            vm.Status = "正在加载中";
            int logCount = await LoadAsync();
            //System.Windows.Forms.MessageBox.Show("一共加载了"+logCount+"条日志");
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

                                temps.Add(log);
                                icount++;
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
