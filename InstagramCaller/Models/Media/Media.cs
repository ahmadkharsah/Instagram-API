using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Models.Media
{

    public class User
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public string username { get; set; }
    }

    public class Thumbnail
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class LowResolution
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class StandardResolution
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Images
    {
        public Thumbnail thumbnail { get; set; }
        public LowResolution low_resolution { get; set; }
        public StandardResolution standard_resolution { get; set; }
    }

    public class Likes
    {
        public int count { get; set; }
    }

    public class Comments
    {
        public int count { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public long id { get; set; }
    }

    public class StandardResolution2
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class LowBandwidth
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class LowResolution2
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Videos
    {
        public StandardResolution2 standard_resolution { get; set; }
        public LowBandwidth low_bandwidth { get; set; }
        public LowResolution2 low_resolution { get; set; }
    }
    public class From
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public string username { get; set; }
    }

    public class Caption
    {
        public string id { get; set; }
        public string text { get; set; }
        public string created_time { get; set; }
        public From from { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public User user { get; set; }
        public Images images { get; set; }
        public string created_time { get; set; }
        public object caption { get; set; }
        public bool user_has_liked { get; set; }
        public Likes likes { get; set; }
        public List<object> tags { get; set; }
        public string filter { get; set; }
        public Comments comments { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public Location location { get; set; }
        public object attribution { get; set; }
        public List<object> users_in_photo { get; set; }
        public Videos videos { get; set; }
    }
    public class Datum
    {
        public string id { get; set; }
        public User user { get; set; }
        public Images images { get; set; }
        public string created_time { get; set; }
        public Caption caption { get; set; }
        public bool user_has_liked { get; set; }
        public Likes likes { get; set; }
        public List<object> tags { get; set; }
        public string filter { get; set; }
        public Comments comments { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public Location location { get; set; }
        public object attribution { get; set; }
        public List<object> users_in_photo { get; set; }
        public Videos videos { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Media
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
    }

    public class MediaSearch
    {
        public List<Data> data { get; set; }
        public Meta meta { get; set; }
    }

    public class UserMedia
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }
}
