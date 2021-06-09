using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Playprism.Services.TeamService.API.Interface.Repositories;
using Playprism.Services.TeamService.API.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Repositories
{
    public class Auth0Repository: IAuth0Repository
    {
        private readonly AuthConfig _authConfig;
        private readonly IRestClient _restClient;

        public Auth0Repository(IOptionsMonitor<AuthConfig> authConfig, IRestClient restClient)
        {
            _authConfig = authConfig.CurrentValue;
            _restClient = restClient;
            _restClient.BaseUrl = new Uri("https://dev-e821827o.eu.auth0.com/api/v2/users");
        }

        public async Task<UserInfo> SearchUserByNameAsync(string username)
        {
            var request = new RestRequest(Method.GET);
            request
                .AddParameter("q", $"username:{username}")
                .AddParameter("search_engine", "v3")
                .AddHeader("authorization", $"Bearer {_authConfig.Token}");
            IRestResponse response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Got unauthorized response from Auth0");
                }
                //TODO: log error
                return null;
            }
            var userInfo = JsonConvert.DeserializeObject<IEnumerable<UserInfo>>(response.Content);
            return userInfo.FirstOrDefault();
        }

        public async Task<UserInfo> GetUserByUserId(string userId)
        {
            var request = new RestRequest(userId, Method.GET)
                .AddHeader("authorization", $"Bearer {_authConfig.Token}");
            IRestResponse response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Got unauthorized response from Auth0");
                }
                //TODO: log error
                return null;
            }
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(response.Content);
            return userInfo;
        }
    }
}
