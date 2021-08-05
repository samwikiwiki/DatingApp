import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}
  
  //loggedIn : boolean;
  //currentUser$: Observable<User>;

  constructor(public accountService : AccountService) { }

  ngOnInit(): void {
    //this.getCurrentUser();

    //el current user se setea en el app.component dependiendo si existe la cookie o no será null
    //si es null, el html de este componente mostrará el formulario del login
    //si no es null, mostrará que estamps logueados
    //aqui lo que hace es hacer como un get de este valor seteado en app.component
    //this.currentUser$ = this.accountService.currentUser$;
  }

  login(){
    //nuestro metodo login está devolviendo un observable
    //un observable es lazy, no hace nada hasta que nos suscribimos, así que...
    this.accountService.login(this.model).subscribe(response=> {
      console.log("login nav");
      console.log(response);
      
      //this.loggedIn = true;


    }, error => {
      console.log(error);
    })
  }

  logout(){
    this.accountService.logout();
    //this.loggedIn = false;
  }

  //getCurrentUser(){
    /*this.accountService.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    }, error => {
      console.log(error);
    })
    }*/
    /*if (this.accountService.getCurrentUser()==null){
      this.loggedIn = false;
    }else{
      this.loggedIn = true;
      
    }*/
  //}
  

}

