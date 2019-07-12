import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  users: User[];

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.userService.getLikers(this.authService.decodedToken.nameid).subscribe(data => {
      this.users = data;
    }, error => {
      this.alertify.error(error);
    });
  }
}
