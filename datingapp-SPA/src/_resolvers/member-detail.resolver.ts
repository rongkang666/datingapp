// tslint:disable-next-line:quotemark
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { User } from 'src/app/_models/User';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class MemberDetailResolver implements Resolve<User> {
    constructor(private userService: UserService, private router: Router, private aletify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // tslint:disable-next-line:no-string-literal
        return this.userService.getUser(route.params['id']).pipe(
            catchError(error => {
                this.aletify.error('Problem retrieving data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}
