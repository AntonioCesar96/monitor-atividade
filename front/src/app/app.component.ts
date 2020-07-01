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
  monitor: any;

  constructor(private signalRService: AppSignalRService) { }

  ngOnInit() {
    this.signalRService.criarConexao();
    this.signalRService.iniciarConexao();

    this.signalRService
      .receberMonitor()
      .pipe(takeUntil(this.destroy$))
      .subscribe((monitor) => {
        this.monitor = monitor;
        setTimeout(() => {
          this.signalRService.buscarMonitor();
        }, 100);
      });

    this.signalRService.buscarMonitor();
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
