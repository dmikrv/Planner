import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  form!: FormGroup;
  registerInvalid: boolean = true;
  formSubmitAttempt!: boolean;
  errors: any[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private as: AuthService) { }

  async onSubmit() {
    this.formSubmitAttempt = false;

    if (this.form.valid) {
      const username = this.form.get('username')!.value;
      const password = this.form.get('password')!.value;
      // const confirmPassword = this.form.get('confirmPassword')!.value;

      // if (password != confirmPassword) {
      //   return;
      // }

      this.as.signup(username, password)
        .subscribe(res => {
          this.registerInvalid = false;
          },
          error => {
          if (error.status == 409)
            this.errors = [{description: "Emailaddress already in use"}];
          else
            this.errors = error.error;
        });
    } else {
      this.formSubmitAttempt = true;
    }
  }

  passwordConfirming(c: AbstractControl): { invalid: boolean } {
    if (c.get('password')?.value !== c.get('confirm_password')?.value) {
      return {invalid: true};
    }
    return {invalid: false}
  }

  async ngOnInit() {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      passwords: this.fb.group({
        password: ['', [Validators.required]],
        confirm_password: ['', [Validators.required]],
      }, {validator: this.passwordConfirming}),
    });

    console.log(this.form.get('passwords') as FormGroup);

    if (this.as.isAuthenticated) {
      this.router.navigate(['']);
    }
  }
}
