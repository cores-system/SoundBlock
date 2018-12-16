using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SoundBlock
{
    public class GraficHub : Hub
    {
        static IClientProxy hub;

        async public override Task OnDisconnectedAsync(Exception exception)
        {
            hub = null;
            await base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            hub = Clients.Client(Context.ConnectionId);
            return base.OnConnectedAsync();
        }


        async public static void Send(float curent, float level, float limiter)
        {
            if (hub != null)
                await hub.SendAsync("OnGrafic", curent, level, limiter);
        }

        async public static void OnLog(string text)
        {
            if (hub != null)
                await hub.SendAsync("OnLog", text);
        }
    }
}
