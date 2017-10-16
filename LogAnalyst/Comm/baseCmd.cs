using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InkeverCommon
{
    /// <summary>
    ///基础的命令，以后的业务中使用到的命令直接重载里面的Execute函数
    /// </summary>
    public class baseCmd : ICommand
    {
        public baseCmd()
        {
        }

        Action act;

        public baseCmd(Action _act)
        {
            act = _act;
        }

        /// <summary>
        /// 暂时不用
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 用来控制当前的命令是否可用，在重载函数中处理IsEnable是否可用
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool CanExecute(object parameter)
        {
            //if (CanExecuteChanged != null)
            //{
            //    CanExecuteChanged(this, new EventArgs());
            //}
            return true;
        }

        /// <summary>
        /// 命令的执行的具体过程
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute(object parameter)
        {
            if (act != null)
            {
                act();
            }
        }

        bool isEnable = true;

        /// <summary>
        /// 表示命令是否可用的属性标志，用来绑定控制按钮或是菜单中 对应的enable属性
        /// </summary>
        public virtual bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                isEnable = value;
            }
        }
    }
}