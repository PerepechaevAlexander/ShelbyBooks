import {Inject, Injectable} from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class BookCountService {
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl(
    this.baseUrl + '/BookCount', {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    })
    .build();

  constructor(@Inject('BASE_API_URL') private baseUrl: string) { }

  startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(
      this.baseUrl + '/BookCount', {
       skipNegotiation: true,
       transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection.start().then( () => {
      console.log('Hub connection started')
    })
      .catch(err => console.log('Error while starting hub connection: ' + err))
  }


}
