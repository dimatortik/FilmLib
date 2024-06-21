import { Component, Inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../services/auth.service/auth.service';
import { routes } from '../../../../app.routes';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-username',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './change-username.component.html',
  styleUrl: './change-username.component.scss',
})
export class ChangeUsernameComponent {
  usernameForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: AuthService,
    public dialogRef: MatDialogRef<ChangeUsernameComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.usernameForm = this.fb.group({
      newUsername: ['', Validators.required],
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    if (this.usernameForm.valid) {
      const formData = new FormData();
      Object.keys(this.usernameForm.controls).forEach((key) => {
        formData.append(key, this.usernameForm.get(key)?.value);
      });
      this.userService.changeUsername(formData).subscribe(
        () => {
          alert('Successful');
          this.dialogRef.close(this.usernameForm.value);
          this.userService.logout();
          this.router.navigate(['/']);
        },
        (error) => alert(`${error.message}`)
      );
    }
  }
}
