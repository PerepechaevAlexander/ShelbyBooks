import {Component, OnInit} from '@angular/core';
import {Order, OrderService} from "../../services/order.service";
import {Title} from "@angular/platform-browser";
import {Error} from "../cart/cart.component";

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  title = 'Заказы';
  orders:Order[] = [];
  currentError: Error = {StatusCode: 0, Message:''};


  constructor(private orderService: OrderService, private titleService: Title) { }

  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.orderService.getOrders().subscribe( (res:Order[]) => { this.orders = res });
  }

  update(){
    this.currentError = {StatusCode: 0, Message:''};
    this.orderService.getOrders().subscribe( (res:Order[]) => { this.orders = res });
  }

  receive(orderId: number){
    this.currentError = {StatusCode: 0, Message:''};

    this.orderService.receiveOrder(orderId).subscribe(response => {
      if (response.status === 200){
        let order = this.orders.find(o=> o.id === orderId)!;
        order.status = 'Выполнен';
      }
    },
      (error) => {
        this.currentError = <Error>(error.error)
      })
  }

  cancel(orderId: number){
    this.currentError = {StatusCode: 0, Message:''};

    this.orderService.cancelOrder(orderId).subscribe(response => {
      if (response.status === 200){
        let order = this.orders.find(o=> o.id === orderId)!;
        order.status = 'Отменён';
      }
    },
      (error) => {
        this.currentError = <Error>(error.error)
      })
  }

  protected readonly localStorage = localStorage;
}
