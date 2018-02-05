using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InstagramCaller.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using InstagramCaller.Models.User;
using InstagramCaller.ViewModels;
using InstagramCaller.Models.Relation;
using InstagramCaller.Models.Media;
using InstagramCaller.Models.Comment;
using InstagramCaller.Models.Like;
using InstagramCaller.Models.Tag;
using InstagramCaller.Endpoints;

namespace InstagramCaller
{
    public class InstagramCaller
    {
        //Azure Insights Telemetery Client
        private TelemetryClient _Telemetryclient;

        //Properties
        private string _ClientID { get; set; }
        private string _ClientSecret { get; set; }
        public string _AccessToken { get; set; }
        public long _UserID { get; set; }

        public UsersEndPoint UsersEndPoint { get; }
        public RelationsEndPoint RelationsEndPoint { get; }
        public MediaEndPoint MediaEndPoint { get; }
        public CommentsEndPoint CommentsEndPoint { get; }
        public LikesEndPoint LikesEndPoint { get; }
        public TagsEndPoint TagsEndPoint { get; }
        public LocationsEndPoint LocationsEndPoint { get; }

        //Helpers
        private HttpClient _HttpClient;
        private HttpResponseMessage _ResponseMessage;

        /// <summary>
        /// create new instance of Instagram Caller to call Instagram API
        /// </summary>
        /// <param name="clientid">App's Client ID</param>
            /// <param name="clientsecret">App's Client Secret</param>
        /// <param name="userid">Current User ID who has the token</param>
        /// <param name="accesstoken">Access Token of current user</param>
        public InstagramCaller(string clientid,string clientsecret,long userid,string accesstoken,string telemetryclient ="")
        {
            //assign properties
            _ClientID = clientid;
            _ClientSecret = clientsecret;
            _UserID = userid;
            _AccessToken = accesstoken;

            //create telemetry client
            if (telemetryclient != "")
            {
                _Telemetryclient = new TelemetryClient(new TelemetryConfiguration(telemetryclient));
            }

            //prepare Http Client
            _HttpClient = new HttpClient();
            _HttpClient.BaseAddress = new Uri("https://api.instagram.com/v1/");
            _HttpClient.DefaultRequestHeaders.Clear();
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //initiate response message
            _ResponseMessage = new HttpResponseMessage();

            //initiate the End Points
            UsersEndPoint = new UsersEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            RelationsEndPoint = new RelationsEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            MediaEndPoint = new MediaEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            CommentsEndPoint = new CommentsEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            LikesEndPoint = new LikesEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            TagsEndPoint = new TagsEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
            LocationsEndPoint = new LocationsEndPoint(_ClientID, _ClientSecret, _AccessToken, _UserID, _HttpClient, _ResponseMessage, _Telemetryclient);
        }
    }
}
