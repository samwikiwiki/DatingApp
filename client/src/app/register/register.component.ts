import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input() usersFromHomeComponent: any;
  @Output() cancelFromRegisterComponent = new EventEmitter(); 

  model: any = {};

  constructor(private accountService : AccountService, private toaster: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(response=>{
      console.log("registe");
      console.log(response);
      this.cancel();
    }), error =>{
      console.log(error);
      this.toaster.error(error.error);
    }
  }

  cancel(){
    this.cancelFromRegisterComponent.emit(false);
  }

}
