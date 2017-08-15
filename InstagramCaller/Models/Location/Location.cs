using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Location
{
    public class Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Location
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

    public class LocationSearch
    {
        public List<Data> data { get; set; }
        public Meta meta { get; set; }
    }
}
