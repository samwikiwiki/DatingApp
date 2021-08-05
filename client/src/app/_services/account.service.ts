import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = "https://localhost:5001/api/";

  private currentUserSource = new ReplaySubject<User>(1);

  //creamos un observable a partir de la variable anterior para que pueda recoger el usuario desde otros componentes
  currentUser$ = this.currentUserSource.asObservable();
  //private currentUser : User;

  constructor(private http:HttpClient) {}

    login(model: any){
      //return this.http.post(this.baseUrl + "account/login", model);
      return this.http.post(this.baseUrl + "account/login", model).pipe(
        map((response: User) => {
          const user = response;
         
          if (user){
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
            //this.currentUser = user;
          }
          
        })
      )
    }

    register(model:any){
      return this.http.post(this.baseUrl+'account/register', model).pipe(
        map((user: User) => {
          localStorage.setItem('user', JSON.stringify(user));
          //this.currentUserSource.next(user);
        })
      )
    }

    setCurrentUser(user: User){
      //this.currentUser = user;
      this.currentUserSource.next(user);
    }

    logout(){
      console.log("logout");
      localStorage.removeItem('user');
      //this.currentUser = null;
      this.currentUserSource.next(null);
    }

    /*getCurrentUser(): User{
      return this.currentUser;
    }*/
  
   
}



