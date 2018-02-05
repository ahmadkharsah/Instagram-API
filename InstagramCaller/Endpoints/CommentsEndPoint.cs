using InstagramCaller.Models.Comment;
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
    /// This Class Calls Instagram Comments API.
    /// </summary>
    public class CommentsEndPoint
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
        /// Create an instance of comments End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public CommentsEndPoint(string clientid, string clientsecret, string accesstoken, long userid,HttpClient httpclient, HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
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
        /// get all comments of media by media id
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <returns>all comments for the media</returns>
        public async Task<Comment> GetCommentsForMedia(string mediaid,string accesstoken)
        {
            Comment comment = new Comment();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/comments?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    comment = JsonConvert.DeserializeObject<Comment>(responsestring);
                }
                else
                {
                    comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return comment;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return comment;
        }

        /// <summary>
        /// post a comment to media
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <param name="text">comment text</param>
        /// <returns>comment</returns>
        public async Task<Comment> POSTCommentsForMedia(string mediaid, string text,string accesstoken)
        {
            Comment comment = new Comment();
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>();
            p.Add(new KeyValuePair<string, string>("access_token", accesstoken));
            p.Add(new KeyValuePair<string, string>("text", text));
            HttpContent content = new FormUrlEncodedContent(p);

            try
            {
                _ResponseMessage = await _HttpClient.PostAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/comments", content);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    comment = JsonConvert.DeserializeObject<Comment>(responsestring);
                }
                else
                {
                    comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return comment;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return comment;
        }

        /// <summary>
        /// delete a comment of a media
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <param name="commentid">comment id</param>
        /// <returns>comment</returns>
        public async Task<Comment> DeleteCommentsForMedia(string mediaid, string commentid,string accesstoken)
        {
            Comment comment = new Comment();
            try
            {
                _ResponseMessage = await _HttpClient.DeleteAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/comments/" + commentid + "?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    comment = JsonConvert.DeserializeObject<Comment>(responsestring);
                }
                else
                {
                    comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return comment;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                comment.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return comment;
        }

    }
}
