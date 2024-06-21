import { Component, HostListener } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NgForOf, NgOptimizedImage, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { MatAnchor, MatIconButton } from '@angular/material/button';
import { AuthService } from '../../../features/auth/services/auth.service/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports: [
    MatMenuModule,
    RouterLinkActive,
    NgOptimizedImage,
    RouterLink,
    MatIconModule,
    NgForOf,
    MatAnchor,
    MatIconButton,
    NgIf,
  ],
  standalone: true,
})
export class NavbarComponent {
  isScrolled = false;
  isAuthenticated = false;
  isAdmin = false;
  authSubscription: Subscription;
  roleSubscription: Subscription;

  @HostListener('window:scroll')
  scrollEvent() {
    this.isScrolled = window.scrollY >= 30;
  }

  constructor(private authService: AuthService) {
    this.authSubscription = this.authService
      .isAuthenticated()
      .subscribe((isAuthenticated) => {
        this.isAuthenticated = isAuthenticated;
      });

    this.roleSubscription = this.authService
      .getUserDetails()
      .subscribe((detail) => {
        this.isAdmin = detail.role === 'Admin';
      });
  }

  onLogout(): void {
    this.authService.logout();
    this.isAuthenticated = false;
  }
}
