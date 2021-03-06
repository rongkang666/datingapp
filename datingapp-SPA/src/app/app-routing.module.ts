import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './member-detail/member-detail.component';
import { MemberDetailResolver } from 'src/_resolvers/member-detail.resolver';
import { MemberEditComponent } from './member-edit/member-edit.component';
import { MemberEditResolver } from 'src/_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MessagesResolver } from 'src/_resolvers/messages.resolver';

const routes: Routes = [
  {path: '', component: HomeComponent},

  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [

      {path: 'members', component: MemberListComponent},
      {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
      {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
      {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
      {path: 'lists', component: ListsComponent}
    ]
  },
  {path: '**', redirectTo: '', pathMatch: 'full'}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
