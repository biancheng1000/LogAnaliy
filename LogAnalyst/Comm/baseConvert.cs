using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InkeverCommon
{
    /// <summary>
    /// 进行数据绑定转换的基类，所有的转换类都从这里继承
    /// 可以根据需要重载转换和反向转换
    /// </summary>
    public class baseConvert : IValueConverter
    {

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    /// <summary>
    /// 多绑定的基类
    /// </summary>
    public class baseMuiltConvert : IMultiValueConverter
    {
        public virtual  object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
