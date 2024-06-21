import { inject } from '@angular/core';
import { CanActivateChildFn, Router, CanLoadFn } from '@angular/router';
import { AuthService } from '../../services/auth.service/auth.service';

export const roleGuard: CanActivateChildFn = (childRoute, state): any => {
  const route = inject(Router);
  const authService = inject(AuthService);

  const userDetails = authService.getUserDetails().subscribe((userDetails) => {
    console.log(userDetails);
    if (userDetails.role == 'Admin') {
      return true;
    } else {
      return route.navigate(['/403']);
    }
  });
};
