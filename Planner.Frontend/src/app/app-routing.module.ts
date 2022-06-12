import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { LabelsComponent } from './components/tags/labels/labels.component';
import { AuthGuardService } from './services/auth-guard.service';
import {TrashComponent} from "./components/actions/trash/trash.component";


const routes: Routes = [
  { path: '', redirectTo: 'tags/labels', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  // { path: 'next', component: NextComponent },
  // { path: 'list/inbox', component: LabelsComponent, canActivate: [AuthGuardService] },
  // { path: 'list/next', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'list/trash', component: TrashComponent, canActivate: [AuthGuardService] },


  { path: 'tags/labels', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'tags/contacts', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'tags/areas', component: LabelsComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
