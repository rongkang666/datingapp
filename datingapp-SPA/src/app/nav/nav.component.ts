import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import {AlertifyService} from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(

      next => {
        console.log('log in successfully');
        this.alertify.success('Log in successfully');

      },
      error => {
        console.log('Failed to log in');
        this.alertify.error('Failed to log in');
      }
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  LoggedOut() {
    localStorage.removeItem('token');
    console.log('logged out');
    this.alertify.message('Logged out');

  }


}
