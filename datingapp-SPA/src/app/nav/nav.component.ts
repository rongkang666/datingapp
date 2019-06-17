import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import {AlertifyService} from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(

      next => {
        console.log('log in successfully');
        this.alertify.success('Log in successfully');
        this.router.navigate(['/members']);

      },
      error => {
        console.log('Failed to log in');
        this.alertify.error('Failed to log in');
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  LoggedOut() {
    localStorage.removeItem('token');
    console.log('logged out');
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);

  }


}
