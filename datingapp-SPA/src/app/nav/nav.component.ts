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
  photoUrl: string;
  public innerWidth: any;





  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
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
    localStorage.removeItem('user');

    this.authService.decodedToken = null;
    this.authService.currentUser = null;

    console.log('logged out');
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);

  }

}
