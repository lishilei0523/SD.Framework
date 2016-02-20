using System.Collections.Generic;

namespace ShSoft.Framework2015.Common.Web
{
    public class PageList<T> : List<T> where T : class
    {

        int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        } int _pageIndex;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        } int _total;

        public int Total
        {
            get { return _total; }
            set { _total = value; }
        }
    }


    public class NewPageList<T> where T : class
    {
        public NewPageList()
        {
            this.Data = new List<T>();
        }

        int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        } int _pageIndex;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        } int _total;

        public int Total
        {
            get { return _total; }
            set { _total = value; }
        }
        public List<T> Data { get; set; }
    }
}