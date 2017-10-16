using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyst.Comm
{
    /// <summary>
    /// 综合顾虑器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GroupFilter
    {
       List<Predicate<object>> _filters;
       public GroupFilter()
        {
            _filters = new List<Predicate<object>>();
          Filter = InternalFilter;
        }

        /// <summary>
        /// 顾虑器
        /// </summary>
       public Predicate<object> Filter
       {
           get;
           set;
       }

        /// <summary>
        /// 合并过滤器
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
       private bool InternalFilter(object o)
        {
          foreach(var filter in _filters)
          {
            if (!filter(o))
            {
              return false;
            }
          }

          return true;
        }
        
        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <param name="filter"></param>
       public void AddFilter(Predicate<object> filter)
        {
            if (!_filters.Contains(filter))
            {
                _filters.Add(filter);
            }
        }

        /// <summary>
        /// 移除过滤器
        /// </summary>
        /// <param name="filter"></param>
       public void RemoveFilter(Predicate<object> filter)
        {
          if (_filters.Contains(filter))
          {
            _filters.Remove(filter);
          }
        }    
  }
    
}
