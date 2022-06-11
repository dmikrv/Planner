import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

import { AUTH_API_URL, RESOURCE_API_URL } from './app-injections-tokens';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ActionsModule } from './components/actions/actions.module';
import { LoginComponent } from './components/login/login.component';
import { TagsModule } from './components/tags/tags.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { ACCESS_TOKEN_KEY } from './services/auth.service';

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY) ?? sessionStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavBarComponent
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
