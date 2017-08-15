using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Tag
{
    public class Data
    {
        public string name { get; set; }
        public int media_count { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Tag
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

    public class TagSearch
    {
        public List<Data> data { get; set; }
        public Meta meta { get; set; }
    }
}
