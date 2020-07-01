using cpu_activity_monitor.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace cpu_activity_monitor.Hubs
{
    public class MonitorHub : Hub
    {
        private readonly MonitorService _monitorService;
        private readonly ProcessoService _processoService;

        public MonitorHub(
            MonitorService monitorService,
            ProcessoService processoService)
        {
            _monitorService = monitorService;
            _processoService = processoService;
        }

        public async Task ObterMonitor()
        {
            var monitor = _monitorService.ObterDadosDoComputador();
            monitor.Processos = _processoService.ObterProcessos();

            await Clients.Client(Context.ConnectionId)
                .SendAsync("ReceberMonitor", monitor);
        }
    }
}
