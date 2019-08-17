import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
import { User } from './_models/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  jwtHelper = new JwtHelperService();

  currentYear: number;

  constructor(private authService: AuthService) {
    this.currentYear = new Date().getFullYear();
  }

  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));

    if (token != null) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user != null) {
      this.authService.currentUser = user;
    }
    if (user.photoUrl != null) {
      this.authService.changMemberPhoto(user.photoUrl);
    }
  }
}
