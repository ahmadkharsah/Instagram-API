using InstagramCaller.Models.Media;
using InstagramCaller.Models.Tag;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.Endpoints
{
    /// <summary>
    /// This Class Calls Instagram's Tags API
    /// </summary>
    public class TagsEndPoint
    {
        //Azure Insights Telemetery Client
        private TelemetryClient _Telemetryclient;

        //Properties
        private string _ClientID { get; set; }
        private string _ClientSecret { get; set; }
        private string _AccessToken { get; set; }
        private long _UserID { get; set; }

        //Helpers
        private HttpClient _HttpClient;
        private HttpResponseMessage _ResponseMessage;

        /// <summary>
        /// Create an instance of tags End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public TagsEndPoint(string clientid, string clientsecret, string accesstoken, long userid, HttpClient httpclient, HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
        {
            _ClientID = clientid;
            _ClientSecret = clientsecret;
            _AccessToken = accesstoken;
            _UserID = userid;
            _Telemetryclient = telemetryclient;
            _HttpClient = httpclient;
            _ResponseMessage = responsemessage;
        }
        /// <summary>
        /// get tags count for a #hashtag
        /// </summary>
        /// <param name="tagname">tag name without #</param>
        /// <returns>count of #hashtag</returns>
        public async Task<Tag> GetTagsCountForName(string tagname)
        {
            Tag tag = new Tag();
            StringContent content = new StringContent("");
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "tags/" + tagname + "?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    tag = JsonConvert.DeserializeObject<Tag>(responsestring);
                }
                else
                {
                    tag.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return tag;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                tag.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return tag;
        }

        /// <summary>
        /// get recent media tagged with a tag name
        /// </summary>
        /// <param name="tagname">tag name without #</param>
        /// <param name="max_tag_id">max tag id</param>
        /// <param name="min_tag_id">min tag id</param>
        /// <param name="count">count of retrived media</param>
        /// <returns>recent media tagged by tag name</returns>
        public async Task<MediaSearch> GetRecentTaggedMediaForTagName(string tagname, string max_tag_id = "", string min_tag_id = "", int count = 0)
        {
            MediaSearch media = new MediaSearch();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "tags/" + tagname + "/media/recent?access_token=" + _AccessToken;
            if (max_tag_id != "")
            {
                URL += "&max_tag_id=" + max_tag_id;
            }
            if (min_tag_id != "")
            {
                URL += "&min_tag_id=" + min_tag_id;
            }
            if (count > 0)
            {
                URL += "&count=" + count.ToString();
            }
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(URL);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<MediaSearch>(responsestring);
                }
                else
                {
                    media.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return media;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                media.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return media;
        }

        /// <summary>
        /// get all tags possibilties for a tag name
        /// </summary>
        /// <param name="tagname">tag name without #</param>
        /// <returns>all tags possibilities for a tag name</returns>
        public async Task<TagSearch> GetTagSearchForTagName(string tagname)
        {
            TagSearch tagsearch = new TagSearch();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "tags/search?q=" + tagname + "&access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    tagsearch = JsonConvert.DeserializeObject<TagSearch>(responsestring);
                }
                else
                {
                    tagsearch.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return tagsearch;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                tagsearch.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return tagsearch;
        }

    }
}
