import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class WalletService {
  headers = new HttpHeaders({
    'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
  });

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }

  wallet(){
    return this.http.post<number>(this.baseUrl + '/Wallet', null,{ 'headers': this.headers })
  }

  topWalletUp(amount: number){
    return this.http.post(this.baseUrl + '/Wallet/TopWalletUp', amount, { 'headers': this.headers, observe: 'response' })
  }
}
