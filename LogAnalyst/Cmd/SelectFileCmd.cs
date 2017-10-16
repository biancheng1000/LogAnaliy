using InkeverCommon;
using LogAnalyst.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
namespace LogAnalyst.Cmd
{
    /// <summary>
    /// 选择指定的文件
    /// </summary>
    public class SelectFileCmd:baseCmd
    {
        LogAnalystVm vm;
        public SelectFileCmd(LogAnalystVm _vm)
        {
            vm = _vm;
        }

        public override void Execute(object parameter)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\anghua\Documents\Tencent Files\469477947\FileRecv";
                openFileDialog.Filter = "text files (*.txt)|*.txt";
                openFileDialog.RestoreDirectory = true;

                System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    vm.LogFileName = openFileDialog.FileName;
                }
            }
        }
    }
}
