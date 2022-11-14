import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private tokenStorageService:TokenStorageService, private router:Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean  {
    let isAuthenticated = this.tokenStorageService.getToken()
    if(isAuthenticated){
      return true;
    } else {
      this.router.navigateByUrl('/login');
      return false
    }
    
  }
}
