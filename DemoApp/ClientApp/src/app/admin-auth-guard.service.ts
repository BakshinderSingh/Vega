import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';
import { CanActivate, Router } from '@angular/router';
import { AuthGuardService } from './auth-guard.service';

@Injectable()
export class AdminAuthGuardService extends AuthGuardService {

  constructor(auth: AuthService, private router: Router) {
    super(auth);
  }

  canActivate() {
    return super.canActivate() ? this.auth.IsInRole('Admin') : false;
  }

}
