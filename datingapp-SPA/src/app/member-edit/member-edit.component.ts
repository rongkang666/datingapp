import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../_models/User';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm', {static: false}) editForm: NgForm;

  user: User;
  photoUrl: string;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private aletify: AlertifyService, private userService: UserService,
    // tslint:disable-next-line:align
    private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line:no-string-literal
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  updateUser(editForm) {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.aletify.success('Updated successfully');
      // console.log(editForm);
      // console.log(this.editForm);
       // tslint:disable-next-line:align
       editForm.reset(this.user);
    }, error => {
      this.aletify.error(error);
    });

  }

  updateMemberPhoto(photoUrl) {
    this.user.photoUrl = photoUrl;
  }

}
