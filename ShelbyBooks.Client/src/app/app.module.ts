import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './components/main/main.component';

import { AuthModule, LogLevel} from 'angular-auth-oidc-client';
import { NavbarComponent } from './components/navbar/navbar.component';
import {CartComponent} from "./components/cart/cart.component";
import {OrderComponent} from "./components/order/order.component";
import { WalletComponent } from './components/wallet/wallet.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    NavbarComponent,
    CartComponent,
    OrderComponent,
    WalletComponent,
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        ReactiveFormsModule,
        AuthModule.forRoot({
            config: {
                authority: 'https://localhost:5000',
                redirectUrl: window.location.origin,
                postLogoutRedirectUri: window.location.origin,
                clientId: 'interactive',
                scope: 'openid profile offline_access email',
                responseType: 'code',
                silentRenew: true,
                useRefreshToken: true,
                logLevel: LogLevel.Debug,
            },
        }),
        FormsModule,
    ],
  providers: [
    Title,
    {
      provide: "BASE_API_URL", useValue: 'https://localhost:7000'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
