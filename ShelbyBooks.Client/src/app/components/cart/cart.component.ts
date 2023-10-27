import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {Book} from "../../services/book.service";
import {OrderService} from "../../services/order.service";
import {Title} from "@angular/platform-browser";
import {WalletService} from "../../services/wallet.service";
import {CartService} from "../../services/cart.service";

export interface Error{
  StatusCode: number;
  Message: string;
}

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  title = 'Корзина';
  cart: Book[] = [];
  cost: number = 0;
  currentError: Error = {StatusCode: 0, Message:''};

  constructor(private router: Router, private orderService: OrderService, private cartService: CartService,
              private titleService: Title, private walletService: WalletService) { }


  ngOnInit(){
    this.titleService.setTitle(this.title);
    this.cartService.getCart().subscribe((res:Book[]) => {
      this.cart = res
      this.updateCost();
    });
  }

  addBook(bookId:number) {
    this.currentError = {StatusCode: 0, Message:''};

    this.cartService.addBook(bookId).subscribe(response => {
      if (response.status === 200) {
        let book = this.cart.find(b => b.id === bookId)!;
        book.quantity += 1;
        this.updateCost();
      }
    },
      (error) => {
        this.currentError = <Error>(error.error)
      })
  }

  removeBook(bookId:number, bookQuantity:number){
    this.currentError = {StatusCode: 0, Message:''};

    if (bookQuantity > 0) {

      this.cartService.removeBook(bookId).subscribe(response => {
        if(response.status === 200){
          let book = this.cart.find(b=> b.id === bookId)!;
          book.quantity -= 1;
          this.updateCost();
        }
      });

    }
  }

  createOrder(){
    this.currentError = {StatusCode: 0, Message:''};

    let quantity: number = 0;
    for (let book of this.cart){ quantity += book.quantity; }
    this.walletService.wallet().subscribe(response => {

      if (quantity < 1) {
        this.currentError = {StatusCode: 500, Message:'Не удалось сформировать заказ: корзина пуста.'};
      }
      else if (response < this.cost) {
        this.currentError = {StatusCode: 500, Message:'Не удалось сформировать заказ: баланс недостаточен.'};
      }
      else {
        this.orderService.createOrder().subscribe(response => {
          if (response.status === 200){
            this.cart = [];
            this.router.navigate(['order']);
          }
        })
      }

    });
  }

  updateCost(){
    let res: number = 0;
    for (let book of this.cart){
      res += book.cost*book.quantity;
    }
    this.cost = res;
  }

}
