using Microsoft.AspNetCore.SignalR;

namespace OpenPrintServerVueNet.Server.Hibs
{
    public class NotificationsHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("on.notification", message);
        }
    }
}

