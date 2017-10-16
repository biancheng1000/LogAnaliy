using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogAnalyst.Comm
{
    /// <summary>
    /// 可以进行排序的view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewableCollection<T> : ObservableCollection<T>,INotifyPropertyChanged
    {
        public ViewableCollection(IEnumerable<T> data)
            : base(data)
        { 
            
        }

        public ViewableCollection(IList<T> data)
            : base(data)
        {

        }

        public ViewableCollection()
        { 
        
        }

        private ListCollectionView _view;
        /// <summary>
        /// A bindable view of this Observable Collection (of T) that supports filtering, sorting, and grouping.
        /// </summary>
        public ListCollectionView View
        {
            get
            {
                if (_view == null)
                {
                    _view = new ListCollectionView(this);
                }
                return _view;
            }
        }

       public event PropertyChangedEventHandler PropertyChanged;
       public void OnpropertyChange(string propertyName)
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
    }

}
