import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

export interface OrderBook{
  id:string;
  title:string;
  quantity:number;
}

export interface Order{
  id:number;
  status:string;
  orderBooks:OrderBook[];
  cost:number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  headers = new HttpHeaders({
    'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
  });

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }

  getOrders() {
    return this.http.post<Order[]>(this.baseUrl + '/Order',null,{ 'headers': this.headers });
  }

  createOrder(){
    return this.http.post(this.baseUrl + '/Order/CreateOrder', null, { 'headers': this.headers, observe: 'response' });
  }

  receiveOrder(orderId: number){
    return this.http.post(this.baseUrl + '/Order/ReceiveOrder', orderId, { 'headers': this.headers, observe: 'response' });
  }

  cancelOrder(orderId: number){
    return this.http.post(this.baseUrl + '/Order/CancelOrder', orderId, { 'headers': this.headers, observe: 'response' });
  }

}
