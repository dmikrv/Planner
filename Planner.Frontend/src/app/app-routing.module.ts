import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NextComponent } from "./components/actions/next/next.component";
import {LabelsComponent} from "./components/tags/labels/labels.component";
import {LoginComponent} from "./components/login/login.component";
import {AuthGuardService} from "./services/auth-guard.service";


const routes: Routes = [
  { path: '', redirectTo: 'labels', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  // { path: 'next', component: NextComponent },
  { path: 'labels', component: LabelsComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
