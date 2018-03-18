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
            //This is a note to those looking either in the source code or decompiled. Please do not use my secret :( This is a client-only
            //application so i have to leave my secret in here.
            var disclaimer = "This is a note to those looking either in the source code or decompiled. Please do not use my secret :( This is a client-only" +
                             "application so i have to leave my secret in here.";
            _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "34430044113-jqdis25snltgl3bu1m0ek9i6nsope81j.apps.googleusercontent.com",
                    ClientSecret = "kkyYKBCrwSWX_rxnb4vuGZ5v"
                },
                new[] { YouTubeService.Scope.Youtube },
                "user",
                CancellationToken.None,
                new FileDataStore("Youtube.ServiceTools"));

            Service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "ServiceTools"
            });

            await MainLoop();
        }

        public async Task<IEnumerable<SuperChatEvent>> MainLoop()
        {
            var asyc = await Service.SuperChatEvents.List("id").ExecuteAsync();
            return asyc.Items;
        }
    }
}
