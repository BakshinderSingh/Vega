import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import * as auth0 from 'auth0-js';
import {JwtHelper} from 'angular2-Jwt';

(window as any).global = window;

@Injectable()
export class AuthService {
  private roles=[];
  userProfile: any;

  auth0 = new auth0.WebAuth({
    clientID: 'WYsR7FLk7V0JMnu1kcpL1ZlkiglYGdwv',
    domain: 'bakshinder.auth0.com',
    responseType: 'token id_token',
    audience: 'https://api.vega.com',
    redirectUri: 'http://localhost:50049/callback',
    scope: 'openid email profile',
    additionalSignUpFields: [
      {
        name: 'name',
        placeholder:"Name"
      }
    ]
  });

  constructor(public router: Router) {
    this.ReadRolesFromLocalStorage();
  }

  private ReadRolesFromLocalStorage() {
    var token = localStorage.getItem('access_token');
    if (token) {
      var jwtHelper = new JwtHelper;
      var decodedToken = jwtHelper.decodeToken(token);
      this.roles = decodedToken['https://vega.com/roles'];
    }
  }

  public IsInRole(roleName): boolean{
    //console.log(this.roles);
    return this.roles.indexOf(roleName) > -1;
  }

  public login(): void {
    this.auth0.authorize();
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // Access Token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '{}');
    return new Date().getTime() < expiresAt;
  }

  public handleAuthentication(): void {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        this.setSession(authResult);
        this.ReadRolesFromLocalStorage();
        //this.auth0.getUserInfo(authResult, (profile, error) => {
        //})
        this.router.navigate(['/home']);
      } else if (err) {
        this.router.navigate(['/home']);
        console.log(err);
      }
    });
  }

  public getProfile(cb): void {
    const accessToken = localStorage.getItem('access_token');
    if (!accessToken) {
      throw new Error('Access Token must exist to fetch profile');
    }

    const self = this;
    this.auth0.client.userInfo(accessToken, (err, profile) => {
      if (profile) {
        self.userProfile = profile;
      }
      cb(err, profile);
    });
  }

  private setSession(authResult): void {
    // Set the time that the Access Token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', expiresAt);
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    this.roles = [];
    // Go back to the home route
    this.router.navigate(['/']);
  }

}
