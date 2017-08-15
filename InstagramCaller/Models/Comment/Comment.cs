using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Comment
{
    public class From
    {
        public string id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public From from { get; set; }
        public string text { get; set; }
        public string created_time { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Comment
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }
}
