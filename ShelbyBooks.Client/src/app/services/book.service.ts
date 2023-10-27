import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export interface Book{
  id: number;
  isbn?: string;
  title: string;
  author: string;
  year: number;
  image?: string;
  cost: number;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }

  GetBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.baseUrl + '/Book');
  }

  getBookCount(bookId:number): Observable<number> {
    return this.http.post<number>(this.baseUrl + '/Book/BookCount', bookId)
  }

}
