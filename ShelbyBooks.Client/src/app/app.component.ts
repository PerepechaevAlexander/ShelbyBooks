import {Component, OnDestroy, OnInit} from '@angular/core';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {Router} from "@angular/router";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private router: Router, public oidcSecurityService: OidcSecurityService) { }

  ngOnInit() {
    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken, idToken, configId }) => {
      if(isAuthenticated){
        localStorage.setItem('isAuthenticated', 'true');
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('idToken', idToken);
        localStorage.setItem('name', userData.name);
        localStorage.setItem('email', userData.email)
      }
      else{
        localStorage.setItem('isAuthenticated', 'false');
        this.router.navigate(['']);
      }
    });
  }


}
