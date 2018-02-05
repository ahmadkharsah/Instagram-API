using InstagramCaller.Models.Relation;
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
    /// relation actions
    /// </summary>
    public enum RelationAction
    {
        follow,
        unfollow,
        approve,
        ignore
    }

    /// <summary>
    /// This Class Calls Instagram's Relations API
    /// </summary>
    public class RelationsEndPoint
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
        /// Create an instance of relations End Point
        /// </summary>
        /// <param name="clientid">Client ID</param>
        /// <param name="clientsecret">Client Secret</param>
        /// <param name="accesstoken">Access Token of Current User</param>
        /// <param name="userid">Current User ID</param>
        /// <param name="httpclient">Http Client</param>
        /// <param name="responsemessage">Response Message</param>
        /// <param name="telemetryclient">Azure Telemetery Client</param>
        public RelationsEndPoint(string clientid, string clientsecret, string accesstoken, long userid, HttpClient httpclient, HttpResponseMessage responsemessage, TelemetryClient telemetryclient = null)
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
        /// get follows of current user
        /// </summary>
        /// <returns>all follows</returns>
        public async Task<Relation> Self_GetFollows(string accesstoken)
        {
            Relation users = new Relation();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/self/follows?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<Relation>(responsestring);
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

        /// <summary>
        /// get folllowed-by users of current user
        /// </summary>
        /// <returns>followed-by users</returns>
        public async Task<Relation> Self_GetFollowedBy(string accesstoken)
        {
            Relation users = new Relation();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/self/followed-by?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<Relation>(responsestring);
                }
                else
                {
                    users.meta.code = (int) _ResponseMessage.StatusCode;
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

        /// <summary>
        /// get all users who send follow request
        /// </summary>
        /// <returns>all users who send follow request</returns>
        public async Task<Relation> Self_GetRequestedBy(string accesstoken)
        {
            Relation users = new Relation();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/self/requested-by?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<Relation>(responsestring);
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

        /// <summary>
        /// get relation's status between current user and other user
        /// </summary>
        /// <param name="userid">other user id</param>
        /// <returns>relation's status between current user and other user</returns>
        public async Task<RelationStatus> Self_GetRelationStatus(long userid,string accesstoken)
        {
            RelationStatus status = new RelationStatus();
            try
            {
                _ResponseMessage = await _HttpClient.GetAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/" + userid.ToString() + "/relationship?access_token=" + accesstoken);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<RelationStatus>(responsestring);
                }
                else
                {
                    status.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return status;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                status.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return status;
        }

        /// <summary>
        /// modify the relation between current user and other user
        /// </summary>
        /// <param name="userid">other user id</param>
        /// <param name="action">action to perform on the relatioin</param>
        /// <returns>relation's status</returns>
        public async Task<RelationStatus> Self_ModifyRelation(string accesstoken,long userid, RelationAction action)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("action",action.ToString()) });
            RelationStatus status = new RelationStatus();
            try
            {
                _ResponseMessage = await _HttpClient.PostAsync(_HttpClient.BaseAddress.AbsoluteUri + "users/" + userid.ToString() + "/relationship?access_token=" + accesstoken , content);
                if (_ResponseMessage.IsSuccessStatusCode)
                {
                    string responsestring = await _ResponseMessage.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<RelationStatus>(responsestring);
                }
                else
                {
                    status.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
                }
                return status;
            }
            catch (Exception ex)
            {
                if (_Telemetryclient != null)
                    _Telemetryclient.TrackException(ex);
                status.meta.code = int.Parse(_ResponseMessage.StatusCode.ToString());
            }
            return status;
        }
    }
}
