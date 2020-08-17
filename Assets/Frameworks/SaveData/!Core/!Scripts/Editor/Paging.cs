namespace HandyPackage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Paging<T>
    {
        private int pageIndex;
        private int amountToShowPerPage;
        private ICollection<T> collections;

        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                if (value < 0) pageIndex = 0;
                else
                {
                    int maxPage = MaxPage();
                    if (value >= maxPage - 1) pageIndex = maxPage - 1;
                    else pageIndex = value;
                }
            }
        }

        public int AmountToShowPerPage
        {
            get { return amountToShowPerPage; }
            set { amountToShowPerPage = (value <= 0) ? 1 : value; }
        }

        public ICollection<T> Collections
        {
            get { return collections; }
            set
            {
                if (collections != value)
                {
                    collections = value;
                    PageIndex = 0;
                }
            }
        }

        public int MaxPage()
        {
            if (collections == null) return 0;
            if (collections.Count == 0) return 0;
            return (int)Math.Ceiling((float)collections.Count / (float)amountToShowPerPage);
        }

        public Paging(int amountToShowPerPage, ICollection<T> collections)
        {
            this.Collections = collections;
            this.PageIndex = 0;
            this.AmountToShowPerPage = amountToShowPerPage;
        }

        public ICollection<T> GetItems()
        {
            if (collections == null) return null;
            if (collections.Count == 0) return collections;

            return collections.Skip(amountToShowPerPage * pageIndex)
                                .Take(amountToShowPerPage).ToList();
        }

    }

}
