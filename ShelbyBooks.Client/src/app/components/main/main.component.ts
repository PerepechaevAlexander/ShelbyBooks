import {Component, OnDestroy, OnInit} from '@angular/core';
import {Book, BookService} from "../../services/book.service";
import {Title} from "@angular/platform-browser";
import {CartService} from "../../services/cart.service";
import {BookCountService} from "../../services/book-count.service";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})

export class MainComponent implements OnInit, OnDestroy {
  protected readonly localStorage = localStorage;
  title: string = 'Главная';
  books: Book[] = [];
  show: boolean = true;

  constructor(private bookService: BookService, private cartService: CartService,
              private titleService: Title, private bookCountService: BookCountService) { }

  ngOnInit(): void {
    this.titleService.setTitle(this.title);
    this.bookService.GetBooks().subscribe((res:Book[]) => { this.books = res });
    // Присасываемся к сокету
    this.bookCountService.startConnection();
    // Обрабатываем пришедшее событие
    this.bookCountService.hubConnection.on("updateBookCountResponse", (bookId, count) =>{
      for (let book of this.books){
        if (book.id === bookId){ book.quantity = count }
      }
    })
  }

  ngOnDestroy() {
    this.bookCountService.hubConnection.off('');
  }

  addBook(bookId:number, bookQuantity:number) {
    if (bookQuantity > 0) {
      this.cartService.addBook(bookId).subscribe(response => {
        if(response.status === 200){
        }
      });

    }
  }

  showAvailable() {
    this.show = false;
  }
  showAll() {
    this.show = true;
  }
}


