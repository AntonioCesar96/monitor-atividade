using cpu_activity_monitor.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace cpu_activity_monitor.Services
{
    public class ProcessoService
    {
        public List<Processo> ObterProcessos()
        {
            Process[] processos = Process.GetProcesses();

            return processos
                .GroupBy(x => x.ProcessName)
                .Select(x => new
                {
                    x.First().ProcessName,
                    Cpu = x.Sum(y => ObterUsoCpuDoProcesso(y)),
                    Ram = x.Sum(y => y.WorkingSet64),
                    Threads = x.Sum(y => y.Threads.Count),
                })
                .Select(x => new Processo()
                {
                    NomeDoProcesso = x.ProcessName,
                    Cpu = x.Cpu,
                    CpuFormatado = x.Cpu.ToString("F2", CultureInfo.InvariantCulture) + "%",
                    Ram = x.Ram,
                    RamFormatado = string.Format("{0:#,##0.00}", ((x.Ram / 1024) / 1024)) + " MB",
                    Threads = x.Threads,
                })
                .OrderByDescending(x => x.Ram)
                .ToList();
        }

        private decimal ObterUsoCpuDoProcesso(Process prc)
        {
            try
            {
                TimeSpan wallTime = DateTime.Now - prc.StartTime;
                if (prc.HasExited) wallTime = prc.ExitTime - prc.StartTime;
                var procTime = prc.TotalProcessorTime;
                var cpuUsage = (decimal)(procTime.TotalMilliseconds / wallTime.TotalMilliseconds) / Environment.ProcessorCount;

                return Math.Round(cpuUsage * 100, 2);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
