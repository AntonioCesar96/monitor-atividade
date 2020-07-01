using System.Diagnostics;

namespace cpu_activity_monitor.Hubs
{
    public class Processo
    {
        public string NomeDoProcesso { get; set; }
        public decimal Cpu { get; set; }
        public string CpuFormatado { get; set; }
        public float Ram { get; set; }
        public string RamFormatado { get; set; }
        public int Threads { get; set; }
    }
}
