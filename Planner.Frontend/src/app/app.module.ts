import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {HttpClientModule} from "@angular/common/http";

import {MatCardModule} from "@angular/material/card";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {MatTableModule} from "@angular/material/table";
import {MatFormFieldModule} from "@angular/material/form-field";
import {ActionsModule} from "./components/actions/actions.module";
import { LabelsComponent } from './components/tags/labels/labels.component';

import {AUTH_API_URL, RESOURCE_API_URL} from "./app-injections-tokens";
import {environment} from "../environments/environment";
import {JwtModule} from "@auth0/angular-jwt";
import {ACCESS_TOKEN_KEY} from "./services/auth.service";
import { LoginComponent } from './components/login/login.component';
import {NextComponent} from "./components/actions/next/next.component";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {TagsModule} from "./components/tags/tags.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatCheckboxModule} from "@angular/material/checkbox";

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY) ?? sessionStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,

    // автоматическая подстановка токена
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: environment.allowedDomains // домены для которых будет выполнятся подстановка
      }
    }),

    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatFormFieldModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatCheckboxModule,

    ActionsModule,
    TagsModule,

  ],
  providers: [{
    provide: AUTH_API_URL,
    useValue: environment.authApi
  },
  {
    provide: RESOURCE_API_URL,
    useValue: environment.resourceApi
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
