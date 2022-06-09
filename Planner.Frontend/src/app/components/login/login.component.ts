import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private as: AuthService) { }

  form!: FormGroup;
  loginInvalid!: boolean;
  formSubmitAttempt!: boolean;

  async onSubmit() {
    console.log('login on submit')
    this.loginInvalid = false;
    this.formSubmitAttempt = false;

    if (this.form.valid) {
      const username = this.form.get('username')!.value;
      const password = this.form.get('password')!.value;
      const rememberMe = this.form.get('rememberMe')!.value;
      await this.as.login(username, password, rememberMe)
        .subscribe(res => {this.router.navigate([''])},
            error => {this.loginInvalid = true;});

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

    if (this.as.isAuthenticated()) {
      await this.router.navigate(['']);
    }
  }
}
