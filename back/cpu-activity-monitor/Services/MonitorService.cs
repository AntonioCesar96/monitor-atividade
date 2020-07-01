using cpu_activity_monitor.Hubs;
using System;
using System.Globalization;

namespace cpu_activity_monitor.Services
{
    public class MonitorService
    {
        public Monitor ObterDadosDoComputador()
        {
            Int64 phav = PerformanceInfoService.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfoService.GetTotalMemoryInMiB();
            decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
            decimal percentOccupied = 100 - percentFree;
            var availablePhysicalMemory = string.Format("{0:#,##0.00}", phav) + " MB";
            var memoryInUse = string.Format("{0:#,##0.00}", tot - phav) + " MB";
            var totalMemory = string.Format("{0:#,##0.00}", tot) + " MB";
            var free = percentFree.ToString("F2", CultureInfo.InvariantCulture) + "%";
            var occupied = percentOccupied.ToString("F2", CultureInfo.InvariantCulture) + "%";
            var cpuMaquina = new CpuMonitorService().Obter().ToString("F2", CultureInfo.InvariantCulture) + "%";

            return new Monitor()
            {
                CpuUsoMaquina = cpuMaquina,
                MemoriaDisponivel = availablePhysicalMemory,
                MemoriaDisponivelPorcentagem = free,
                MemoriaOcupadaPorcentagem = occupied,
                MemoriaTotalDoComputador = totalMemory,
                MemoriaUsada = memoryInUse
            };
        }
    }
}
