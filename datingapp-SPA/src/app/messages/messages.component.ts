import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];

  messageContainer = 'Unread';

  constructor(private userService: UserService, private authService: AuthService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line:no-string-literal
      this.messages = data['messages'];
    });
  }

  loadMessages() {
    this.userService.getMessages(this.authService.decodedToken.nameid, this.messageContainer)
    .subscribe((messages: Message[]) => {
      this.messages = messages;
    }, error => {
      this.alertify.error(error);
    });
  }

  deleteMessage(id: number) {
    this.userService.deleteMessage(id, this.authService.decodedToken.nameid).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
      this.alertify.success('Message has been deleted');
    }, error => {
      this.alertify.error(error);
    });
  }

}
