using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cpu_activity_monitor.Hubs
{
    public class CpuMonitorService
    {
        public decimal Obter()
        {
            List<PerformanceCounter> cpuCounters = new List<PerformanceCounter>();
            var cores = 0;

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                cores = cores + int.Parse(item["NumberOfCores"].ToString());
            }

            int procCount = System.Environment.ProcessorCount;
            for (int i = 0; i < procCount; i++)
            {
                PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                cpuCounters.Add(pc);
            }

            try
            {
                decimal sum = 0;
                foreach (PerformanceCounter c in cpuCounters)
                {
                    sum = sum + (decimal) c.NextValue();
                }
                sum = sum / (cores);

                return Math.Round(sum, 2);
            }
            catch (Exception e) { }

            return 0;
        }
    }
}
