import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { LabelsComponent } from './components/tags/labels/labels.component';
import { AuthGuardService } from './services/auth-guard.service';


const routes: Routes = [
  { path: '', redirectTo: 'list/next', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  // { path: 'next', component: NextComponent },
  { path: 'labels', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'list/inbox', component: LabelsComponent, canActivate: [AuthGuardService] },
  { path: 'list/next', component: LabelsComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
