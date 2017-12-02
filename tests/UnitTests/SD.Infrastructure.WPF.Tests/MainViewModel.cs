using SD.Infrastructure.WPF.Interfaces;
using SD.Infrastructure.WPF.Tests.Others;
using SD.Infrastructure.WPF.Toolkits;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Tests
{
    /// <summary>
    /// 主窗体ViewModel
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged, IPageable
    {
        #region # 构造器

        private readonly List<FakeData> _source;

        public MainViewModel()
        {
            //初始化属性
            this.FakeSource = new ObservableCollection<FakeData>();

            //初始化数据
            FakeData fake = new FakeData();
            this._source = fake.GenerateFakeSource();

            //初始化页码、页容量
            this.PageIndex = 1;
            this.PageSize = 20;
            this.RowCount = this._source.Count;

            this.RefreshData();
        }

        #endregion

        #region # 属性

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }

            set
            {
                if (this._pageIndex != value)
                {
                    this._pageIndex = value;
                    this.OnPropertyChanged("PageIndex");
                }
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                if (this._pageSize != value)
                {
                    this._pageSize = value;
                    this.OnPropertyChanged("PageSize");
                }
            }
        }

        private int _rowCount;
        public int RowCount
        {
            get
            {
                return this._rowCount;
            }

            set
            {
                if (this._rowCount != value)
                {
                    this._rowCount = value;
                    this.OnPropertyChanged("RowCount");
                }
            }
        }

        private ObservableCollection<FakeData> _fakeSource;
        public ObservableCollection<FakeData> FakeSource
        {
            get
            {
                return this._fakeSource;
            }
            set
            {
                if (this._fakeSource != value)
                {
                    this._fakeSource = value;
                    this.OnPropertyChanged("FakeSource");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(this.RefreshData);
            }
        }

        #endregion

        #region # 方法


        public void RefreshData()
        {
            int rowCount, pageCount;
            IEnumerable<FakeData> result = this._source.ToPage(this.PageIndex, this._pageSize, out rowCount, out pageCount);
            this.RowCount = rowCount;

            this.FakeSource.Clear();

            foreach (FakeData fakeData in result)
            {
                this.FakeSource.Add(fakeData);
            }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
