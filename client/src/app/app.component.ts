import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  palabro = 'ola k ase';

  users : any //any es para almacenar cualquier tipo de dato en ts (string, int...)

  constructor (private http: HttpClient){}

  ngOnInit() {
    this.http.get('https://localhost:5001/api/users').subscribe(response =>{
      this.users = response;
    }, error =>{
      console.log(error);
    })
  }
}
