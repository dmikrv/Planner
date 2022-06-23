import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from '../../../Planner.Web.Frontend/src/app/components/login/login.component';
import { LabelsComponent } from '../../../Planner.Web.Frontend/src/app/components/tags/labels/labels.component';
import { AuthGuardService } from '../../../Planner.Web.Frontend/src/app/services/auth-guard.service';
import {TrashComponent} from "../../../Planner.Web.Frontend/src/app/components/actions/trash/trash.component";
import {ContactsComponent} from "../../../Planner.Web.Frontend/src/app/components/tags/contacts/contacts.component";
import {AreasComponent} from "../../../Planner.Web.Frontend/src/app/components/tags/areas/areas.component";
import {NextComponent} from "../../../Planner.Web.Frontend/src/app/components/actions/next/next.component";
import {LogoutComponent} from "../../../Planner.Web.Frontend/src/app/components/logout/logout.component";
import {SignupComponent} from "./components/signup/signup.component";

const routes: Routes = [
  { path: '', redirectTo: 'list/next', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'signup', component: SignupComponent },
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
