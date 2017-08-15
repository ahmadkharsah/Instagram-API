using InstagramCaller.Models.Like;
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
    /// This Class Calls Instagram's Likes API.
    /// </summary>
    public class LikesEndPoint
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
        /// Create an instance of likes End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public LikesEndPoint(string clientid, string clientsecret, string accesstoken, long userid, HttpClient httpclient, HttpResponseMessage responsemessage,TelemetryClient telemetryclient = null) 
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
        /// get all likes on a media
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <returns>all likes on the media</returns>
        public async Task<Like> GetLikesForMedia(string mediaid)
        {
            Like like = new Like();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/likes?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    like = JsonConvert.DeserializeObject<Like>(responsestring);
                }
                else
                {
                    like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return like;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return like;
        }

        /// <summary>
        /// post a like on media
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <returns>media</returns>
        public async Task<Like> PostLikesOnMedia(string mediaid)
        {
            Like like = new Like();
            StringContent content = new StringContent("");
            try
            {
                _ResponseMessage = await _HttpClient.PostAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/likes?access_token=" + _AccessToken, content);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    like = JsonConvert.DeserializeObject<Like>(responsestring);
                }
                else
                {
                    like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return like;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return like;
        }

        /// <summary>
        /// delete a like on media (like made by current user)
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <returns>like</returns>
        public async Task<Like> DeleteLikesOnMedia(string mediaid)
        {
            Like like = new Like();
            StringContent content = new StringContent("");
            try
            {
                _ResponseMessage = await _HttpClient.DeleteAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "/likes?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    like = JsonConvert.DeserializeObject<Like>(responsestring);
                }
                else
                {
                    like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return like;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                like.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return like;
        }

    }
}
