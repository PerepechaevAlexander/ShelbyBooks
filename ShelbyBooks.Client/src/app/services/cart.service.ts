import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Book} from "./book.service";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  headers = new HttpHeaders({
    'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
  });

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }

  getCart() {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
    });
    return this.http.get<Book[]>(this.baseUrl + '/Cart', { 'headers': this.headers });
  }

  addBook(bookId:number){
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
    });
    return this.http.post(this.baseUrl + '/Cart/AddToCart', bookId, { 'headers': this.headers, observe: 'response' });
  }

  removeBook(bookId:number){
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('idToken')}`
    });
    return this.http.post(this.baseUrl + '/Cart/RemoveFromCart', bookId, { 'headers': this.headers, observe: 'response' });
  }
}
