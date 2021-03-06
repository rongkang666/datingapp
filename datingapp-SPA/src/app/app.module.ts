import { BrowserModule } from '@angular/platform-browser';
import {NgxPaginationModule} from 'ngx-pagination';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BsDropdownModule, TabsModule, BsDatepickerModule} from 'ngx-bootstrap';
import {NgxGalleryModule} from 'ngx-gallery';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import {AlertifyService} from './_services/alertify.service';

import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberCardComponent } from './member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailComponent } from './member-detail/member-detail.component';
import { MemberDetailResolver } from 'src/_resolvers/member-detail.resolver';
import { MemberEditResolver } from 'src/_resolvers/member-edit.resolver';
import { MemberEditComponent } from './member-edit/member-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';

import {TimeAgoPipe} from 'time-ago-pipe';
import { MessagesResolver } from 'src/_resolvers/messages.resolver';
import { MemberMessagesComponent } from './member-messages/member-messages.component';


export function tokenGetter() {
   return localStorage.getItem('token');
}


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      RegisterComponent,
      HomeComponent,
      ListsComponent,
      MemberListComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      TimeAgoPipe,
      MemberMessagesComponent
   ],
   imports: [
      BrowserModule,
      NgxPaginationModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      ReactiveFormsModule,
      TabsModule.forRoot(),
      NgxGalleryModule,
      FileUploadModule,
      JwtModule.forRoot({
         config: {
           // tslint:disable-next-line:object-literal-shorthand
           tokenGetter: tokenGetter,
           whitelistedDomains: ['localhost:5000'],
           blacklistedRoutes: ['localhost:5000/api/auth']
         }
       })
      ],
   providers: [
      AuthService,
      AlertifyService,
      AuthGuard,
      PreventUnsavedChanges,
      MemberDetailResolver,
      MemberEditResolver,
      MessagesResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }


// imports: [
//    BrowserModule,
//    AppRoutingModule,
//    HttpClientModule,
//    FormsModule,
//    BsDropdownModule.forRoot(),
//    TabsModule.forRoot(),
//    NgxGalleryModule,
//    FileUploadModule,
//    JwtModule.forRoot({
//       config: {
//         // tslint:disable-next-line:object-literal-shorthand
//         tokenGetter: tokenGetter,
//         whitelistedDomains: ['localhost:5000'],
//         blacklistedRoutes: ['localhost:5000/api/auth']
//       }
//     })
//    ],
