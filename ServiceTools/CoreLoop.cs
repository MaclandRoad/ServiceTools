using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Google.Apis.YouTube.v3.Data;

namespace ServiceTools
{
    public delegate void NewSuperChatHandler(SuperChatEventSnippet snippet);
    class CoreLoop
    {
        private static CoreLoop _instance;

        public static CoreLoop Instance => _instance ?? (_instance = new CoreLoop());

        public DispatcherTimer Timer = new DispatcherTimer();

        public event NewSuperChatHandler NewSuperChat;
        private IEnumerable<SuperChatEvent> _lastChats;

        public CoreLoop()
        {
            Timer.Tick += TimerOnTick;
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.IsEnabled = true;
            Timer.Start();
        }

        private async void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var newChats = await CoreService.Instance.MainLoop();
        }
    }
}
