import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import {AlertifyService} from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(
      next => {
        console.log('register successfully.');
        this.alertify.success(' Register successfully');
      },
      error => {
        console.log(error);
        this.alertify.error('Register error');
      }

    );
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('logged out');
  }

}
