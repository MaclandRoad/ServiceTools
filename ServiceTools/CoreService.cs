using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace ServiceTools
{
    public class CoreService
    {
        private static CoreService _instance;

        public static CoreService Instance => _instance ?? (_instance = new CoreService());

        public YouTubeService Service;

        private UserCredential _credential;

        public async Task Startup()
        {
            var disclaimer = "This is a note to those looking either in the source code or decompiled. Please do not use my secret :( This is a client-only" +
                             "application so i have to leave my secret in here.";
            _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "632529535771-jfhmk689ql38k5cc4bg7clplvhorfi4i.apps.googleusercontent.com",
                    ClientSecret = "DawckKBXVnuC32bCqL_ZIQkY"
                },
                new[] { YouTubeService.Scope.Youtube },
                "user",
                CancellationToken.None,
                new FileDataStore("Youtube.SuperChatter"));

            Service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "SuperChatter"
            });

            await GetSuperChats();
        }

        public async Task<IEnumerable<SuperChatEvent>> GetSuperChats()
        {
            var asyc = await Service.SuperChatEvents.List("id").ExecuteAsync();
            return asyc.Items;
        }
    }
}
