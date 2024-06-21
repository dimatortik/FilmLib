import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'; // Import the Router module
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  FormControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatLabel } from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { MatError } from '@angular/material/form-field';
import { AuthService } from '../services/auth.service/auth.service';
import { RegisterRequest } from '../services/auth.service/auth.service';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [
    MatFormField,
    MatInput,
    MatButton,
    MatError,
    MatLabel,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.scss',
})
export class SignUpComponent implements OnInit {
  registerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }
  ngOnInit(): void {}

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.authService
      .register(this.registerForm.value as RegisterRequest)
      .subscribe(
        () => {
          alert('Registration successful');
          this.router.navigate(['/']);
        },
        (error) => {
          alert(`Registration failed ${error.message}`);
        }
      );
  }
}
