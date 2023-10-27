import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './components/main/main.component';
import {CartComponent} from "./components/cart/cart.component";
import {OrderComponent} from "./components/order/order.component";
import {WalletComponent} from "./components/wallet/wallet.component";

const routes: Routes = [
  {path:'', component: MainComponent},
  {path:'cart', component: CartComponent},
  {path:'order', component: OrderComponent},
  {path:'wallet', component: WalletComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
