import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APIs } from './APIs';
import IUser from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {



  constructor(private http: HttpClient) { }


  register(user: IUser): Observable<IUser> {
    return this.http.post<IUser>(APIs.AUTH_API + 'register', {
      firstName: user.firstName,
      lastName: user.lastName,
      username: user.username,
      email: user.email,
      password: user.password
    });
  }

  login(user:IUser): Observable<IUser>{
    return this.http.post<IUser>(APIs.AUTH_API + 'login',{
      username:user.username,
      password: user.password
    })
  }

}
