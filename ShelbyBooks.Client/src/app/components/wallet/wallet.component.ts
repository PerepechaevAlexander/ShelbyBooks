import {Component, OnInit} from '@angular/core';
import {Title} from "@angular/platform-browser";
import {WalletService} from "../../services/wallet.service";
import {Error} from "../cart/cart.component";

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {
  title = 'Кошелёк';
  topUp: boolean = false;
  balance: number = 0;
  refill: number = 0;
  currentError: Error = {StatusCode: 0, Message:''};


  constructor(private titleService: Title, private walletService: WalletService) { }

  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.walletService.wallet().subscribe(response => {
      this.balance = response
    });
  }

  TopUpAvailable(){
    this.topUp = true;
  }

  onSubmit(){
    this.currentError = {StatusCode: 0, Message:''};
    const amount = this.refill;
    if (amount > 0){
      this.walletService.topWalletUp(amount).subscribe(response => {
        if (response.status === 200){
          this.balance += amount;
          this.topUp = false;
        }
      })
    }
    else{
      this.currentError = {StatusCode: 500, Message:'Пополнение не удалось: сумма должна быть положительной.'};
    }
  }


  protected readonly localStorage = localStorage;
}
