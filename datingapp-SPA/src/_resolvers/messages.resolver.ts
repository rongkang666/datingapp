// tslint:disable-next-line:quotemark
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';

import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from 'src/app/_models/message';
import { AuthService } from 'src/app/_services/auth.service';


@Injectable()
export class MessagesResolver implements Resolve<Message[]> {

    messageContainer = 'Unread';

    constructor(private userService: UserService, private router: Router,
                private aletify: AlertifyService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        // tslint:disable-next-line:no-string-literal
        return this.userService.getMessages(this.authService.decodedToken.nameid, this.messageContainer).pipe(
            catchError(error => {
                this.aletify.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
