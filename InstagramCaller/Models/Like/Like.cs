using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Like
{

    public class Datum
    {
        public string id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Like
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }
}
