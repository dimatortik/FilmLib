import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthService } from '../services/auth.service/auth.service';
import { MatCard } from '@angular/material/card';
import { MatFormField } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { MatError } from '@angular/material/form-field';
import { MatCardHeader } from '@angular/material/card';
import { MatCardContent } from '@angular/material/card';
import { MatCardTitle } from '@angular/material/card';
import { MatLabel } from '@angular/material/form-field';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [
    NgIf,
    MatCard,
    MatFormField,
    MatInput,
    MatButton,
    MatError,
    MatCardHeader,
    MatCardContent,
    MatCardTitle,
    MatLabel,
    ReactiveFormsModule,
  ],
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});

  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit() {}

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authService
      .login(this.loginForm.value.email, this.loginForm.value.password)
      .subscribe(
        (data) => {
          alert('Login successful');
          this.router.navigate(['/']);

          this.loading = false;
        },
        (error) => {
          alert(`Invalid credentials: ${error.error.message}`);
          this.loading = false;
        }
      );
  }
}
