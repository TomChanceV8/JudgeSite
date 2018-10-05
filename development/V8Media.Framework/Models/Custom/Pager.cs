using System.Collections.Generic;
using System.Linq;

namespace V8Media.Framework.Models.Custom
{
    public class Pager
    {
        public int NumberOfItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<int> Pages { get; set; }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < Pages.Count();
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
    }
}