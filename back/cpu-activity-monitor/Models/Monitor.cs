using System.Collections.Generic;
using System.Diagnostics;

namespace cpu_activity_monitor.Hubs
{
    public class Monitor
    {
        public string MemoriaDisponivel { get; set; }
        public string MemoriaUsada { get; set; }
        public string MemoriaTotalDoComputador { get; set; }
        public string MemoriaDisponivelPorcentagem { get; set; }
        public string MemoriaOcupadaPorcentagem { get; set; }
        public string CpuUsoMaquina { get; set; }
        public List<Processo> Processos { get; set; }
    }
}
