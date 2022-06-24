import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

import { ActionsModule } from '../../../Planner.Web.Frontend/src/app/components/actions/actions.module';
import { LoginComponent } from '../../../Planner.Web.Frontend/src/app/components/login/login.component';
import { LogoutComponent } from '../../../Planner.Web.Frontend/src/app/components/logout/logout.component';
import { NavBarComponent } from '../../../Planner.Web.Frontend/src/app/components/nav-bar/nav-bar.component';
import { TagsModule } from '../../../Planner.Web.Frontend/src/app/components/tags/tags.module';
import { ACCESS_TOKEN_KEY } from '../../../Planner.Web.Frontend/src/app/services/auth.service';
import { AUTH_API_URL, RESOURCE_API_URL } from './app-injections-tokens';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoaderComponent } from './components/loader/loader.component';
import { SignupComponent } from './components/signup/signup.component';
import { ThemeSwitchComponent } from './components/theme-switch/theme-switch.component';
import { LoaderInterceptor } from './interceptors/loader.interceptor';
import {ProjectsComponent} from "./components/projects/projects.component";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatSelectModule} from "@angular/material/select";

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY) ?? sessionStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    NavBarComponent,
    LogoutComponent,
    ThemeSwitchComponent,
    LoaderComponent,
    ProjectsComponent,
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
    MatTooltipModule,
    MatNativeDateModule,
    MatSidenavModule,
    MatExpansionModule,
    MatDividerModule,
    MatFormFieldModule,
    MatChipsModule,
    MatProgressSpinnerModule,

    ActionsModule,
    TagsModule,
    MatSlideToggleModule,
    MatDatepickerModule,
    MatSelectModule,
  ],
  providers: [
    {
      provide: AUTH_API_URL,
      useValue: environment.authApi
    },
    {
      provide: RESOURCE_API_URL,
      useValue: environment.resourceApi
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
