import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';
import { CanActivate } from '@angular/router';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(protected auth: AuthService) { }

  canActivate() {
    if (this.auth.isAuthenticated())
      return true;
    this.auth.login();
    return false;
  }

}
