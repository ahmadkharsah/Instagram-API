using InstagramCaller.Models.Location;
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
    /// This Class Calls Instagram's Location API
    /// </summary>
    public class LocationsEndPoint
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
        /// Create an instance of locations End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public LocationsEndPoint(string clientid, string clientsecret, string accesstoken, long userid, HttpClient httpclient, HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
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
        /// get location info
        /// </summary>
        /// <param name="locationid">locaion id</param>
        /// <returns>location info</returns>
        public async Task<Models.Location.Location> GetLocationInforation(string locationid)
        {
            Models.Location.Location location = new Models.Location.Location();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "locations/" + locationid + "?access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    location = JsonConvert.DeserializeObject<Models.Location.Location>(responsestring);
                }
                else
                {
                    location.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return location;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                location.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return location;
        }

        /// <summary>
        /// get recent media in a location
        /// </summary>
        /// <param name="locationid">location id</param>
        /// <param name="max_id">max media id</param>
        /// <param name="min_id">min media id</param>
        /// <returns>recent media</returns>
        public async Task<MediaSearch> GetRecentMediaForLocation(string locationid, string max_id = "", string min_id = "")
        {
            MediaSearch media = new MediaSearch();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "locations/" + locationid + "/media/recent?" + _AccessToken;
            if (max_id != "")
            {
                URL += "&max_tag_id=" + max_id;
            }
            if (min_id != "")
            {
                URL += "&min_tag_id=" + min_id;
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
        /// get all locations in a lat,lng point
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longtude</param>
        /// <param name="distance">distance less than 750 meters</param>
        /// <param name="facebook_places_id">facebook places id</param>
        /// <returns>all locations</returns>
        public async Task<LocationSearch> GetLocationSearch(string lat, string lng, string distance = "", string facebook_places_id = "")
        {
            LocationSearch location = new LocationSearch();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "locations/search?lat=" + lat + "&lng=" + lng + "&access_token=" + _AccessToken;
            if (distance != "")
            {
                URL += "&distance=" + distance;
            }
            if (facebook_places_id != "")
            {
                URL += "&facebook_places_id=" + facebook_places_id;
            }

            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(URL);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    location = JsonConvert.DeserializeObject<Models.Location.LocationSearch>(responsestring);
                }
                else
                {
                    location.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return location;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                location.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return location;
        }
    }
}
