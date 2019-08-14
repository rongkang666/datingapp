import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { HttpClient } from 'selenium-webdriver/http';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];
  currentUserId: number = this.authService.decodedToken.nameid;

  constructor(private userService: UserService, private aletify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
   this.userService.getUsers().subscribe((users: User[]) => {
    this.users = users;
    // console.log(users.length);
   }, error => {
     this.aletify.error('Cannot get users list');
   });
  }

}
