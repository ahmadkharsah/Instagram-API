using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Relation
{
    public class Pagination
    {
    }

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

    public class Relation
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Data
    {
        public string outgoing_status { get; set; }
        public string incoming_status { get; set; }
        public bool target_user_is_private { get; set; }
    }

    public class RelationStatus
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

}
