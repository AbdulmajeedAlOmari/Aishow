import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { APIs } from './APIs';



@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient) { }

  getUsers() : Observable<any>{
    return this.http.get(APIs.USER_API)
  }
}
