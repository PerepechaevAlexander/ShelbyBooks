import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  protected readonly localStorage = localStorage;
  constructor(private router: Router, public oidcSecurityService: OidcSecurityService) { }

  ngOnInit() {
    this.oidcSecurityService.isAuthenticated().subscribe((isAuthenticated) => {
      if (isAuthenticated) {
        localStorage.setItem('isAuthenticated', 'true');
      }
    });
  }

  login() {
        this.oidcSecurityService.authorize();
  }

  logout() {
    localStorage.setItem('isAuthenticated', 'false');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('idToken');
    localStorage.removeItem('name');
    localStorage.removeItem('email');
    this.oidcSecurityService
      .logoff()
      .subscribe((result) => console.log(result));
  }

}

