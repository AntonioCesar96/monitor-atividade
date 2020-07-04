import { OrdenarPor } from './ordenar-por.enum';
import { AppSignalRService } from './services/app-signalr.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  destroy$: Subject<boolean> = new Subject<boolean>();
  tempoParaBuscarDadosNovamente = 200;
  enumOrdenarPor = OrdenarPor;
  monitor: any;
  ordenarPor = OrdenarPor.Cpu;
  alternarNomeProcesso = false;
  alternarCpu = false;
  alternarMemoria = false;
  alternarThreads = false;

  constructor(private signalRService: AppSignalRService) { }

  ngOnInit() {
    this.signalRService.criarConexao();
    this.signalRService.iniciarConexao();

    this.signalRService
      .receberMonitor()
      .pipe(takeUntil(this.destroy$))
      .subscribe((monitor) => this.receberMonitor(monitor));

    this.signalRService.buscarMonitor();
  }

  receberMonitor(monitor) {
    this.monitor = monitor;
    this.ordenar(this.ordenarPor);

    setTimeout(() => this.signalRService.buscarMonitor(), this.tempoParaBuscarDadosNovamente);
  }

  ordenar(ordenarPor: OrdenarPor) {
    this.ordenarPor = ordenarPor;
    let campoParaOrdenar: string;
    let alternar: boolean;

    switch (ordenarPor) {
      case OrdenarPor.NomeProcesso: {
        alternar = this.alternarNomeProcesso;
        campoParaOrdenar = 'nomeDoProcesso';
        break;
      }
      case OrdenarPor.Cpu: {
        alternar = this.alternarCpu;
        campoParaOrdenar = 'cpu';
        break;
      }
      case OrdenarPor.Memoria: {
        alternar = this.alternarMemoria;
        campoParaOrdenar = 'ram';
        break;
      }
      case OrdenarPor.Threads: {
        alternar = this.alternarThreads;
        campoParaOrdenar = 'threads';
        break;
      }
    }

    this.ordenarProcessos(alternar, campoParaOrdenar);
  }

  ordenarProcessos(alternar, campoParaOrdenar) {
    if (!this.monitor || !this.monitor.processos) { return; }

    this.monitor.processos.sort((p1, p2) => {
      if (typeof p1[campoParaOrdenar] === 'string') {
        return alternar
        ? p1[campoParaOrdenar].localeCompare(p2[campoParaOrdenar])
        : p2[campoParaOrdenar].localeCompare(p1[campoParaOrdenar]);
      }

      return alternar
      ? p1[campoParaOrdenar] - p2[campoParaOrdenar]
      : p2[campoParaOrdenar] - p1[campoParaOrdenar];
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
