import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChangeUsernameComponent } from './change-username/change-username.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AuthService } from '../services/auth.service/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [MatCardModule, MatProgressBarModule, MatButtonModule, NgIf],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
})
export class UserComponent {
  userName: string = '';
  email: string = '';
  role: string = '';
  isLoading = true;

  constructor(private userService: AuthService, public dialog: MatDialog) {
    this.userService.getUserDetails().subscribe((userDetails) => {
      this.userName = userDetails.username;
      this.email = userDetails.email;
      this.role = userDetails.role;
      this.isLoading = false;
    });
  }

  openChangeUserNameDialog(): void {
    const dialogRef = this.dialog.open(ChangeUsernameComponent, {
      width: '300px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.userName = result;
      }
    });
  }

  openChangePasswordDialog(): void {
    this.dialog.open(ChangePasswordComponent, {
      width: '300px',
    });
  }
}
