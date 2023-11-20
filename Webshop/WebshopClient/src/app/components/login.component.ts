import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoginService } from '../services/login.service';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './login.component.html',
  styles: [`
  `]
})
export class LoginComponent implements OnInit {
  email: string = '';
  password: string = '';
  error = ''


  constructor(
    private router: Router,
    private authService: AuthService,
    private route: ActivatedRoute
  ) { }


  ngOnInit(): void {
    if (this.authService.currentLoginValue != null && this.authService.currentLoginValue.loginId > 0) {
      this.router.navigate(['/']);
    }
  }

  login(): void {
    this.error = '';
    this.authService.login(this.email, this.password)
      .subscribe({
        next: () => {
          // get return url from route parameters or default to '/'
          let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigate([returnUrl]);
        },
        error: err => {
          if (err.error?.status == 400 || err.error?.status == 401 || err.error?.status == 500) {
            this.error = 'Forkert brugernavn eller kodeord';
          }
          else {
            this.error = err.error.title;
          }
        }
      });
  }
}
