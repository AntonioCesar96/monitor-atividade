using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace cpu_activity_monitor.Hubs
{
    public class CpuMonitorService
    {
        static List<PerformanceCounter> cpuCounters = new List<PerformanceCounter>();
        static int cores = 0;

        public float Obter()
        {
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                cores = cores + int.Parse(item["NumberOfCores"].ToString());
            }

            int procCount = Environment.ProcessorCount;
            for (int i = 0; i < procCount; i++)
            {
                PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                cpuCounters.Add(pc);
            }

            try
            {
                float sum = 0;
                foreach (PerformanceCounter c in cpuCounters)
                {
                    sum = sum + c.NextValue();
                }
                sum = sum / (cores);
                return sum >= 100 ? 100 : sum;
            }
            catch (Exception e) { }
            return 0;
        }
    }
}
