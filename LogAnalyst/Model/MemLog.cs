using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyst.Model
{
    /// <summary>
    /// 日志的内容
    /// </summary>
    public class MemLog
    {
        public string LogTime { get; set; }
        public LogTextLevel LogLevel { get; set; }
        public string LogSrcName { get; set; }
        public string LogDesc { get; set; }
    }

    /// <summary>
    /// 日志文本的级别
    /// </summary>
    public enum LogTextLevel
    {
        /// <summary>
        /// 最详细的调试信息
        /// </summary>
        DebugDetail = 1,

        /// <summary>
        /// 普通级别的调试信息
        /// </summary>
        Debug,

        /// <summary>
        /// 正常运行时输出的信息
        /// </summary>
        Info,

        /// <summary>
        /// 警告信息
        /// </summary>
        Warning,

        /// <summary>
        /// 错误信息
        /// </summary>
        Error,

        /// <summary>
        /// 严重错误信息
        /// </summary>
        Fatal,
    };
}
