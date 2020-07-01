import { EventEmitter, Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { Subscription, Subject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AppSignalRService implements OnDestroy {
  private _urlHub = 'http://localhost:5001/monitorHub';
  private _connectionEstablished = new EventEmitter<boolean>();
  private _iniciarConexaoTimeoutDelay = 60000;
  private _hubConnection: HubConnection;
  private _connectedSubscription: Subscription;
  private _monitorSubject = new Subject<any>();

  constructor() { }

  receberMonitor(): Observable<any> {
    return this._monitorSubject.asObservable();
  }

  buscarMonitor() {
    this.run('ObterMonitor');
  }

  criarConexao() {
    if (!this._hubConnection) {
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(this._urlHub)
        .configureLogging(LogLevel.Information)
        .build();

      this._hubConnection.onclose(() => {
        this.iniciarConexao();
      });

      this._hubConnection.on('ReceberMonitor', (res) => {
        this._monitorSubject.next(res);
      });
    }
  }

  iniciarConexao() {
    if (this._hubConnection.state === HubConnectionState.Disconnected) {
      this._hubConnection
        .start()
        .then(() => {
          console.log('Conectado ao Hub');
          this._connectionEstablished.emit();
        })
        .catch(err => {
          console.log('Erro ao tentar conectar, tentando novamente...');
          setTimeout(() => {
            this.iniciarConexao();
          }, this._iniciarConexaoTimeoutDelay);
        });
    }
  }

  run(method: string, ...args: any[]) {
    switch (this._hubConnection.state) {
      case HubConnectionState.Connected:
        this._hubConnection.invoke(method, ...args);
        break;
      case HubConnectionState.Connecting:
        this._connectedSubscription = this._connectionEstablished.subscribe((data: any) => {
          this._hubConnection.invoke(method, ...args);
          this._connectedSubscription.unsubscribe();
        });
        break;
      default:
        this._hubConnection.start()
          .then(() => {
            this._hubConnection.invoke(method, args);
          })
          .catch(err => console.error(err.toString()));
        break;
    }
  }

  ngOnDestroy() {
    if (!this._connectedSubscription) { return; }
    this._connectedSubscription.unsubscribe();
  }
}
