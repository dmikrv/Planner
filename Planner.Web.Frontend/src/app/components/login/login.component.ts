import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form!: FormGroup;
  loginInvalid!: boolean;
  formSubmitAttempt!: boolean;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private as: AuthService) { }

  async onSubmit() {
    console.log('login on submit')
    this.loginInvalid = false;
    this.formSubmitAttempt = false;

    if (this.form.valid) {
      const username = this.form.get('username')!.value;
      const password = this.form.get('password')!.value;
      const rememberMe = this.form.get('rememberMe')!.value;

      this.as.login(username, password, rememberMe)
        .subscribe(res => { this.router.navigate(['']) },
          error => { this.loginInvalid = true; });

    } else {
      this.formSubmitAttempt = true;
    }
  }

  async ngOnInit() {
    this.form = this.fb.group({
      username: ['', Validators.email],
      password: ['', Validators.required],
      rememberMe: ['']
    });

    if (this.as.isAuthenticated) {
      this.router.navigate(['']);
    }
  }
}
