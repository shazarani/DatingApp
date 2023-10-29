import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { BehaviorSubject, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
baseURL='https://localhost:5001/api/';
private currentUserSource= new BehaviorSubject<User | null>(null);
currentUser$=this.currentUserSource.asObservable();
  constructor(private http: HttpClient) {}

  login(module:any){
    return this.http.post<User>(this.baseURL +'account/login',module).pipe(
      map((response:User)=>{
      const user=response;
      if(user){
        localStorage.setItem('user',JSON.stringify(user));
        this.currentUserSource.next(user);
      }}
    )
    )
  }
  setCurrentUser(user : User){
this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
  register(model: any){
    return this.http.post<User>(this.baseURL+'account/register',model).pipe(
      map(user=>{
        if (user) {
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        
      }
       

      )
    )
  }
  
}
