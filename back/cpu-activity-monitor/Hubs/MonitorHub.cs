using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace cpu_activity_monitor.Hubs
{
    public class MonitorHub : Hub
    {

        public async Task ObterMonitor()
        {
            Clients.Client(Context.ConnectionId)
                .SendAsync("ReceberMonitor", retorno);
        }
    }
}
