using InstagramCaller.Models.Media;
using InstagramCaller.Models.User;
using InstagramCaller.ViewModels;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstagramCaller.Endpoints
{
    /// <summary>
    /// This Class Calls Instagram's Users API
    /// </summary>
    public class UsersEndPoint
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
        /// Create an instance of users End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public UsersEndPoint(string clientid, string clientsecret, string accesstoken, long userid,HttpClient httpclient,HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
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
        /// get current user info
        /// </summary>
        /// <returns>current user info</returns>
        public async Task<Models.User.User> Self_GetInfo(string accesstoken)
        {
            Models.User.User basicinfo = new Models.User.User();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/self/?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    basicinfo = JsonConvert.DeserializeObject<Models.User.User>(responsestring);
                }
                else
                {
                    basicinfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return basicinfo;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                basicinfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return basicinfo;
        }

        /// <summary>
        /// get other user info
        /// </summary>
        /// <param name="userid">other user id</param>
        /// <param name="accesstoken">access token of other user</param>
        /// <returns>other user info</returns>
        public async Task<Models.User.User> User_GetInfo(long userid, string accesstoken)
        {
            Models.User.User basicinfo = new Models.User.User();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/" + userid.ToString() + "/?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    basicinfo = JsonConvert.DeserializeObject<Models.User.User>(responsestring);
                }
                else
                {
                    basicinfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return basicinfo;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                basicinfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return basicinfo;
        }

        /// <summary>
        /// get media of current user
        /// </summary>
        /// <param name="count">media count to retrive</param>
        /// <param name="max_id">max media id</param>
        /// <param name="min_id">min media id</param>
        /// <returns>user's media</returns>
        public async Task<UserMedia> Self_GetMedia(string accesstoken,int? count = 0, string max_id = "", string min_id = "")
        {
            UserMedia media = new UserMedia();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "users/self/media/recent/?access_token=" + accesstoken;
            if (max_id != "")
            {
                URL += "&max_id=" + max_id;
            }
            if (min_id != "")
            {
                URL += "&min_id=" + min_id;
            }
            if (count != 0)
            {
                URL += "&count=" + count;
            }
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(URL);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<UserMedia>(responsestring);
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
        /// get media of other user
        /// </summary>
        /// <param name="userid">other user's id</param>
        /// <param name="accesstoken">other user's access token</param>
        /// <param name="count">media count to retrive</param>
        /// <param name="max_id">max media id</param>
        /// <param name="min_id">min media id</param>
        /// <returns>other user's media</returns>
        public async Task<UserMedia> User_GetMedia(long userid, string accesstoken, int? count = 0, string max_id = "", string min_id = "")
        {
            UserMedia media = new UserMedia();
            string URL = _HttpClient.BaseAddress.AbsoluteUri + "users/" + userid.ToString() + "/media/recent/?access_token=" + accesstoken;
            if (max_id != "")
            {
                URL += "&max_id=" + max_id;
            }
            if (min_id != "")
            {
                URL += "&min_id=" + min_id;
            }
            if (count != 0)
            {
                URL += "&count=" + count;
            }
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(URL);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<UserMedia>(responsestring);
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
        /// get user info and media of current user
        /// </summary>
        /// <returns>user info and media for current user</returns>
        public async Task<UserInfo_Media> Self_GetInfoandMedia(string accesstoken)
        {
            UserInfo_Media userwithmedia = new UserInfo_Media();
            try
            {
                userwithmedia.UserInfo = await Self_GetInfo(accesstoken);
                userwithmedia.Media = await Self_GetMedia(accesstoken);
                return userwithmedia;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                userwithmedia.Media.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                userwithmedia.UserInfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return userwithmedia;
        }

        /// <summary>
        /// get user info and media of other user
        /// </summary>
        /// <param name="userid">other user's id</param>
        /// <param name="accesstoken">other user's access token</param>
        /// <returns>user info and media of other user</returns>
        public async Task<UserInfo_Media> User_GetUserInfoandMedia(long userid, string accesstoken)
        {
            UserInfo_Media userwithmedia = new UserInfo_Media();
            if (userid > 0 && !string.IsNullOrEmpty(accesstoken))
            {
                try
                {
                    MediaSearch media;
                    _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/" + userid.ToString() + "/media/recent/?access_token=" + accesstoken);
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    media = JsonConvert.DeserializeObject<MediaSearch>(responsestring);
                    userwithmedia.UserInfo = await User_GetInfo(userid, accesstoken);
                    userwithmedia.Media = await User_GetMedia(userid, accesstoken);
                    return userwithmedia;
                }
                catch (Exception ex)
                {
                    if (_Telemetryclient != null)
                        _Telemetryclient.TrackException(ex);
                    userwithmedia.Media.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                    userwithmedia.UserInfo.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
            }
            return userwithmedia;
        }

        /// <summary>
        /// get liked media by current user
        /// </summary>
        /// <returns>liked media by current user</returns>
        public async Task<MediaSearch> Self_GetLikedMedia(string accesstoken)
        {
            MediaSearch media = new MediaSearch();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/self/media/liked?access_token=" + accesstoken);
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
        /// search for a user
        /// </summary>
        /// <param name="q">query</param>
        /// <returns>users match the query</returns>
        public async Task<UserSearch> UserSearch(string q)
        {
            UserSearch users = new UserSearch();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/search?q=" + q + "&access_token=" + _AccessToken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<UserSearch>(responsestring);
                }
                else
                {
                    users.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return users;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                users.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return users;
        }
    }

}
