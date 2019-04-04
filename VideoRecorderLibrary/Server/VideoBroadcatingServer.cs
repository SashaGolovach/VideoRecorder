using Ozeki.Camera;
using Ozeki.Common;
using Ozeki.Media;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRecorderLibrary.Server
{
    public class VideoBroadcatingServer
    {
        WebMStreamer _webmStreamer;
        MediaConnector _connector;

        public VideoBroadcatingServer(WebCamera _camera)
        {
            _webmStreamer = new WebMStreamer(new OzConf_P_WebmStreamer(ServerConfig.Port));
            _webmStreamer.OnClientConnected += Webm_OnClientConnected;
            _webmStreamer.OnClientDisconnected += WebmStreamer_OnClientDisconnected;

            _connector = new MediaConnector();
            _connector.Connect(_camera.VideoChannel, _webmStreamer.VideoReceiver);
        }

        public void StartStreaming()
        {
            _webmStreamer.Start();
            Log.Information("Broadcasting server is online, can be reached at: ");
            foreach (var url in _webmStreamer.StreamerUrls)
            {
                Log.Information(url);
            }
        }

        private static void WebmStreamer_OnClientDisconnected(object sender, GenericEventArgs<WebMStreamClient> e)
        {
            Log.Information("Client Disconnected");
        }

        private static void Webm_OnClientConnected(object sender, GenericEventArgs<WebMStreamClient> e)
        {
            Log.Information("Client connected: " + e.Item.RemoteEndPoint);
            e.Item.StartStreaming();
        }
    }
}
