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
  males: User[] = [];
  females: User[] = [];
  allUsers: User[] = [];
  minAge = 18;
  maxAge = 41;

  currentUserId: number = this.authService.decodedToken.nameid;
  p = 1;

  genderFilter = 'All';

  constructor(private userService: UserService, private aletify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
   this.userService.getUsers().subscribe((users: User[]) => {
    this.users = users;
   }, error => {
     this.aletify.error('Cannot get users list');
   }, () => {
    // console.log(this.users.length);

     // tslint:disable-next-line:prefer-for-of
     for (let i = 0; i < this.users.length; i++) {
        if (this.users[i].gender === 'male' && this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
          this.males.push(this.users[i]);
        }
        if (this.users[i].gender === 'female' && this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
          this.females.push(this.users[i]);
        }
        if (this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
          this.allUsers.push(this.users[i]);
        }
    }
   });
  }

  applyAgeFilter() {
    this.males.length = 0;
    this.females.length = 0;
    this.allUsers.length = 0;
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].gender === 'male' && this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
        this.males.push(this.users[i]);
      }
      if (this.users[i].gender === 'female' && this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
        this.females.push(this.users[i]);
      }
      if (this.users[i].age >= this.minAge && this.users[i].age <= this.maxAge) {
        this.allUsers.push(this.users[i]);
      }
  }
  }

}
