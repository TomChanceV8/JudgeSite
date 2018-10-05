using System.Collections.Generic;

namespace V8Media.Framework.Models.ViewModels
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ActiveClass { get; set; }
        public IEnumerable<MenuItem> ChildMenuItems { get; set; } 
    }
}