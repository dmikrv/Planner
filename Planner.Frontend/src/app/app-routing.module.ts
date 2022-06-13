import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { LabelsComponent } from './components/tags/labels/labels.component';
import { AuthGuardService } from './services/auth-guard.service';
import {TrashComponent} from "./components/actions/trash/trash.component";
import {ContactsComponent} from "./components/tags/contacts/contacts.component";
import {AreasComponent} from "./components/tags/areas/areas.component";
import {NextComponent} from "./components/actions/next/next.component";


const routes: Routes = [
  { path: '', redirectTo: 'list/next', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  // { path: 'list/inbox', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'list/next', component: NextComponent, canActivate: [AuthGuardService] },
  { path: 'list/trash', component: TrashComponent, canActivate: [AuthGuardService] },


  { path: 'tags/labels', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'tags/contacts', component: ContactsComponent, canActivate: [AuthGuardService] },
  { path: 'tags/areas', component: AreasComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
