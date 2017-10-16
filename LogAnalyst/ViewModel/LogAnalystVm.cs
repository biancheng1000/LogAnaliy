using InkeverCommon;
using LogAnalyst.Cmd;
using LogAnalyst.Comm;
using LogAnalyst.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace LogAnalyst.ViewModel
{
    /// <summary>
    /// 包含说有功能的VM
    /// </summary>
    public class LogAnalystVm:BindableBase
    {

        #region 普通属性


        string ignorKey;
        /// <summary>
        /// 模糊忽略
        /// </summary>
        public string IgnorKey
        {
            get { return ignorKey; }
            set { ignorKey = value; }
        }

        string searchKey;
        /// <summary>
        /// 需要查找的关键字
        /// </summary>
        public string SearchKey
        {
            get { return searchKey; }
            set { searchKey = value; }
        }

        MemLog selectLog;
        /// <summary>
        /// 当前选中的日志
        /// </summary>
        public MemLog SelectLog
        {
            get { return selectLog; }
            set { selectLog = value; }
        }

        string logFileName;
        /// <summary>
        /// 日志文件所在的目录
        /// </summary>
        public string LogFileName
        {
            get { return logFileName; }
            set 
            { 
                logFileName = value;
                OnPropertyChanged("LogFileName");
            }
        }
        string ignorFunctionList;
        /// <summary>
        /// 忽略函数的名称
        /// </summary>
        public string IgnorFunctionList
        {
            get { return ignorFunctionList; }
            set 
            {
                ignorFunctionList = value;
                OnPropertyChanged("IgnorFunctionList");
            }
        }


        string searchFunctionList;
        /// <summary>
        /// 查找指定的函数
        /// </summary>
        public string SearchFunctionList
        {
            get { return searchFunctionList; }
            set 
            { 
                searchFunctionList = value;
                OnPropertyChanged("SearchFunctionList");
            }
        }

        private ViewableCollection<MemLog> logs = null;
        /// <summary>
        /// 加载的日志内容
        /// </summary>
        public ViewableCollection<MemLog> Logs
        {
            get 
            {
                if (logs == null)
                {
                    logs = new ViewableCollection<MemLog>();
                    logs.View.Filter = Filter.Filter;
                }
                return logs; 
            }
            set 
            {
                logs = value;
                logs.View.Filter = Filter.Filter;
                OnPropertyChanged("Logs");
            }

        }

        string status;
        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        #endregion

        #region 过滤

        GroupFilter filter = new GroupFilter();
        /// <summary>
        /// 综合过滤器
        /// </summary>
        public GroupFilter Filter
        {
            get { return filter; }
            set { filter = value; }
        }


        #region 函数名称忽略顾虑
        bool isCheckIgnorFunction = false;
        /// <summary>
        /// 是否启用函数名称忽略
        /// </summary>
        public bool IsCheckIgnorFunction
        {
            get { return isCheckIgnorFunction; }
            set 
            { 
                isCheckIgnorFunction = value;
                if (isCheckIgnorFunction)
                {
                    Filter.AddFilter(IgnorFunction);
                }
                else
                {
                    Filter.RemoveFilter(IgnorFunction);
                }
                OnPropertyChanged("IsCheckIgnorFunction");
            }
        }

        bool isCheckSerachFunction = false;
        /// <summary>
        /// 是否查找指定的函数
        /// </summary>
        public bool IsCheckSerachFunction
        {
            get { return isCheckSerachFunction; }
            set
            { 
                isCheckSerachFunction = value;
                if (isCheckSerachFunction)
                {
                    Filter.AddFilter(SerchrFunction);
                }
                else
                {
                    Filter.RemoveFilter(SerchrFunction);
                }
                OnPropertyChanged("IsCheckSerachFunction");
            }
        }

        bool isSearchVague;
        /// <summary>
        /// 是否模糊匹配
        /// </summary>
        public bool IsSearchVague
        {
            get { return isSearchVague; }
            set 
            { 
                isSearchVague = value;
                if (isSearchVague)
                {
                    Filter.AddFilter(VagueSearch);
                }
                else
                {
                    Filter.RemoveFilter(VagueSearch);
                }
            }
        }

        bool isIgnorVague;
        /// <summary>
        /// 模糊忽略
        /// </summary>
        public bool IsIgnorVague
        {
            get { return isIgnorVague; }
            set 
            { 
                isIgnorVague = value;
                if (isIgnorVague)
                {
                    Filter.AddFilter(VagueIgnor);
                }
                else
                {
                    Filter.RemoveFilter(VagueIgnor);
                }
            }
        }
    
        //单个的过虑器
        //函数过滤

        private bool FilterFunction(object logobj,Func<MemLog,bool> func)
        {
            MemLog log = logobj as MemLog;
            if (log != null)
            {
                return func(log);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 模糊查找
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool VagueSearch(object logobj)
        {
            return FilterFunction(logobj, (o) =>
            {
                if (!string.IsNullOrEmpty(searchKey))
                {
                    return o.LogDesc.Contains(searchKey);
                }
                else
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 过滤开始时间
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool FilterStartTime(object logobj)
        {
            return FilterFunction(logobj, (o) =>
            {
                DateTime logStartTime = DateTime.Parse(o.LogTime);
                return logStartTime >= StartTime;
            });
        }

        /// <summary>
        /// 过滤开始时间
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool FilterEndTime(object logobj)
        {
            return FilterFunction(logobj, (o) =>
            {
                DateTime logStartTime = DateTime.Parse(o.LogTime);
                return logStartTime <= EndTime;
            });
        }


        /// <summary>
        /// 模糊忽略
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool VagueIgnor(object logobj)
        {
            return FilterFunction(logobj, (o) =>
            {
                if (!string.IsNullOrEmpty(IgnorKey))
                {
                    return !o.LogDesc.Contains(IgnorKey);
                }
                else
                {
                    return false;
                }
            });
        }


        /// <summary>
        /// 查找指定函数的过滤器
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool SerchrFunction(object logobj)
        {
            return FilterFunction(logobj,
                (o) => 
                {
                    return SearchFunctionList.Split(',').Where(n => o.LogSrcName.Contains(n)).Count() > 0;
                   // return SearchFunctionList==null?false:SearchFunctionList.Contains(o.LogSrcName);
                }
             );
        }

        /// <summary>
        /// 忽略指定的函数
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
        private bool IgnorFunction(object logobj)
        {
            return FilterFunction(logobj, (o) => { return !IgnorFunctionList.Contains(o.LogSrcName); });
        }

        #endregion

        #region 日志级别过滤
        bool isCheckLogLevel;
        /// <summary>
        /// 是否启动日志级别顾虑
        /// </summary>
        public bool IsCheckLogLevel
        {
            get { return isCheckLogLevel; }
            set 
            { 
                isCheckLogLevel = value;
                if (isCheckLogLevel)
                {
                    Filter.AddFilter(IsTargetLogLever);
                }
                else
                {
                    Filter.RemoveFilter(IsTargetLogLever);
                }
            }
        }

        bool isFilterBeginTime;
        bool isFilterEndTime;

        LogTextLevel selectedLevel = LogTextLevel.Error;
        /// <summary>
        /// 当前选中的日志界别
        /// </summary>
        public LogTextLevel SelectedLevel
        {
            get { return selectedLevel; }
            set { selectedLevel = value; }
        }

        //日志类型过滤
        private bool IsTargetLogLever(object logobj)
        {

            return FilterFunction(logobj, (o) => { return o.LogLevel.Equals(SelectedLevel); });

        }
        #endregion

        #region 时间过滤
        //时间过滤

        bool isFilterTime;
        /// <summary>
        /// 是否启动时间过滤
        /// </summary>
        public bool IsFilterTime
        {
            get { return isFilterTime; }
            set
            { 
                isFilterTime = value;
                if (isFilterTime)
                {
                    Filter.AddFilter(IsInTargetTime);
                }
                else
                {
                    Filter.RemoveFilter(IsInTargetTime);
                }
            }
        }


        /// <summary>
        /// 比较指定的日志是否在指定的区间内
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private bool IsInTargetTime(object logobj)
        {
            return FilterFunction(logobj, (o) => 
            {
                DateTime starttime = DateTime.Parse(string.Format("00", SYear) + "-" + string.Format("00", Smonth) + "-" + string.Format("00", SDay));
                DateTime endtime = DateTime.Parse(string.Format("0000", EYear) + "-" + string.Format("00", EMonth) + "-" + string.Format("00", EDay));
                //return o.LogTime <= endtime && o.LogTime >= starttime;
                return true;
            });
        }

        int sYear;
        /// <summary>
        /// 开始小时
        /// </summary>
        public int SYear
        {            
            get { return sYear; }
            set { sYear = value; }
        }
        int smonth;
        /// <summary>
        /// 分钟
        /// </summary>
        public int Smonth
        {
            get { return smonth; }
            set { smonth = value; }
        }
        int sDay;
        /// <summary>
        /// 秒
        /// </summary>
        public int SDay
        {
            get { return sDay; }
            set { sDay = value; }
        }
        int eYear;
        /// <summary>
        /// 结小时
        /// </summary>
        public int EYear
        {
            get { return eYear; }
            set { eYear = value; }
        }
        int eMonth;
        /// <summary>
        /// 结分钟
        /// </summary>
        public int EMonth
        {
            get { return eMonth; }
            set { eMonth = value; }
        }
        int eDay;
        /// <summary>
        /// 结秒
        /// </summary>
        public int EDay
        {
            get { return eDay; }
            set { eDay = value; }
        }

        #endregion

        #endregion

        #region 命令
        /// <summary>
        /// 选择文件目录
        /// </summary>
        public baseCmd SelectLogFileCmd
        {
            get { return new SelectFileCmd(this); }
        }

        /// <summary>
        /// 加载日志内容
        /// </summary>
        public baseCmd LoadLogCmd
        {
            get { return new LoadLogCmd(this); }
        }

        public baseCmd RefreshCmd
        {
            get{return new baseCmd(()=>
            {

                this.Logs.View.Filter = Filter.Filter;
                this.Logs.View.Refresh();
                
                this.Logs.OnpropertyChange("View");

                this.Status = "过滤的结果为："+Logs.View.Count;
            });}
        }
        #endregion

        #region 右键菜单
        ContextMenu menu;
        /// <summary>
        /// 右键菜单：将指定函数添加到忽略函数列表或是查找函数列表
        /// </summary>
        public ContextMenu Menu
        {
            get 
            {
                if (menu == null)
                {
                    menu = new ContextMenu();

                    MenuItem addIgnorCmd = new MenuItem();
                    addIgnorCmd.Header = "添加到忽略函数列表";
                    addIgnorCmd.Command = new baseCmd(() => 
                    {
                        if(SelectLog!=null)
                        {
                            if (string.IsNullOrEmpty(IgnorFunctionList))
                            {
                                IgnorFunctionList = SelectLog.LogSrcName;
                            }
                            else
                            {
                                if (!IgnorFunctionList.Contains(SelectLog.LogSrcName))
                                {
                                    IgnorFunctionList = IgnorFunctionList + "," + SelectLog.LogSrcName;
                                }
                            }
                           
                        }

                        IsCheckIgnorFunction = true;
                    });
                    menu.Items.Add(addIgnorCmd);

                    MenuItem addSearchCmd = new MenuItem();
                    addSearchCmd.Header = "添加到查找函数列表";
                    addSearchCmd.Command = new baseCmd(() =>
                    {
                        if (SelectLog != null)
                        {
                            if (string.IsNullOrEmpty(SearchFunctionList))
                            {
                                SearchFunctionList =  SelectLog.LogSrcName;
                            }
                            else
                            {
                                if (!SearchFunctionList.Contains(SelectLog.LogSrcName))
                                {
                                    SearchFunctionList = SearchFunctionList + "," + SelectLog.LogSrcName;
                                }
                              
                            }
                           IsCheckSerachFunction=true;
                        }
                    });

                    menu.Items.Add(addSearchCmd);

                    MenuItem setStartTime = new MenuItem();
                    setStartTime.Header = "设为开始时间";
                    setStartTime.Command = new baseCmd(() =>
                    {
                        if (SelectLog != null)
                        {
                            DateTime st = DateTime.Parse(selectLog.LogTime);
                            IsFilterBeginTime = true;
                            StartTime = st;
                        }
                    });
                    menu.Items.Add(setStartTime);

                    MenuItem setEndTime = new MenuItem();
                    setEndTime.Header = "设为结束时间";
                    setEndTime.Command = new baseCmd(() =>
                    {
                        if (SelectLog != null)
                        {
                            DateTime st = DateTime.Parse(selectLog.LogTime);
                            IsFilterEndTime = true;
                            EndTime = st;
                        }
                    });
                    menu.Items.Add(setEndTime);

                }
                return menu; 
            }
        
        }

        DateTime startTime;
        DateTime endTime;

        /// <summary>
        /// 是否添加开始时间过滤
        /// </summary>
        public bool IsFilterBeginTime { get => isFilterBeginTime;
            set
            {
                SetProperty<bool>(ref isFilterBeginTime, value);
                if (isFilterBeginTime)
                {
                    Filter.AddFilter(FilterStartTime);
                }
                else
                {
                    Filter.RemoveFilter(FilterStartTime);
                }
            }
        }

        /// <summary>
        /// 是否添加结束时间过滤
        /// </summary>
        public bool IsFilterEndTime { get => isFilterEndTime;
            set {
                SetProperty<bool>(ref isFilterEndTime, value);
                if (isFilterEndTime)
                {
                    Filter.AddFilter(FilterEndTime);
                }
                else
                {
                    Filter.RemoveFilter(FilterEndTime);
                }
            } }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get => startTime; set =>SetProperty<DateTime>(ref startTime ,value); }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get => endTime; set =>SetProperty<DateTime>(ref endTime , value); }

        #endregion

    }
}
