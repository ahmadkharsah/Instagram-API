using InstagramCaller.Models.Media;
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
    /// This Class Calls Instagram's Media API
    /// </summary>
    public class MediaEndPoint
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
        /// Create an instance of media End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public MediaEndPoint(string clientid, string clientsecret, string accesstoken, long userid, HttpClient httpclient, HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
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
        /// get media by id
        /// </summary>
        /// <param name="mediaid">media id</param>
        /// <returns>media</returns>
        public async Task<Media> GetMediaByID(string mediaid)
        {
            Media media = new Media();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/" + mediaid + "?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<Media>(responsestring);
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
        /// get media by shortcode
        /// </summary>
        /// <param name="shortcode">shortcode</param>
        /// <returns>media</returns>
        public async Task<Media> GetMediaByShortCode(string shortcode)
        {
            Media media = new Media();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "media/shortcode/" + shortcode + "?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<Media>(responsestring);
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
        /// get all media in a geogrhpic point
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longtude</param>
        /// <param name="distance">distance less than 5000 meters</param>
        /// <returns>all media</returns>
        public async Task<MediaSearch> MediaSearch(string lat, string lng, int? distance)
        {
            MediaSearch media = new MediaSearch();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "media/search?lat=" + lat + "&lng=" + lng + "&access_token=" + _AccessToken;
            if (distance != null && distance <= 5000)
            {
                URL += "&distance=" + distance.ToString();
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
    }
}
