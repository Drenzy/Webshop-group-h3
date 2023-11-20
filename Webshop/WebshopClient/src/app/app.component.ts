import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Login } from './models/login';
import { OrderItem } from './models/orderitem';
import { CartService } from './services/cart.service';
import { AuthService } from './services/auth.service';
import { SignIn } from './models/signin';
import { Role } from './models/role';

@Component({
  selector: 'app-root',
  template: `

  <nav class="navbar navbar-expand ">
    <div class="container-fluid">
      <a class="navbar-brand" routerLink="/" routerLinkActive="active">Home</a>
      <a class="navbar-brand" routerLink="/login"  routerLinkActive="active" *ngIf="currentLogin == null">Login</a>
        <a class="navbar-brand" *ngIf="currentLogin != null" (click)="logout()">Logout</a>
        <a class="navbar-brand" routerLink="/cart"  routerLinkActive="active">Kurv ({{ basket.length }})</a>
        <a class="navbar-brand" routerLink="/cart/address" routerLinkActive="active"
      *ngIf="currentLogin != null && currentLogin.role == 'Admin'">Checkout Address</a>  

        <!-- Admin Panels, only accesable if role = Admin -->
        <a class="navbar-brand" routerLink="/admin/category" routerLinkActive="active"
      *ngIf="currentLogin != null && currentLogin.role == 'Admin'">Category Panel</a>
        <a class="navbar-brand" routerLink="/admin/product" routerLinkActive="active"
      *ngIf="currentLogin != null && currentLogin.role == 'Admin'">Product Panel</a>
        <a class="navbar-brand" routerLink="/admin/order"  routerLinkActive="active"
      *ngIf="currentLogin != null && currentLogin.role == 'Admin'">Order Panel</a>
        <a class="navbar-brand" routerLink="/admin/customers" routerLinkActive="active"
      *ngIf="currentLogin != null && currentLogin.role == 'Admin'">Customer Panel</a>
    </div>

      </nav>
      
      
    <router-outlet></router-outlet>`,
  styles: []
})
export class AppComponent {
  title = 'WebshopClient';
  basket: OrderItem[] = [];
  currentLogin: SignIn = { loginId: 0, role: Role.User, token: ''};

  constructor(
    private cartService: CartService,
    private router: Router,
    private authService: AuthService) {
    this.authService.currentLogin.subscribe(x => this.currentLogin = x);
  }

  ngOnInit(): void {
    console.log("test");
    this.cartService.currentBasket.subscribe(x => this.basket = x)
  }

  logout() {
    if (confirm('Er du sikker på du vil logge ud')) {
      // ask auth service to perform logout
      this.authService.logout();
      // subscribe to the changes in currentUser, and load Home component
      this.authService.currentLogin.subscribe(x => {
        this.currentLogin = x
        this.router.navigate(['/']);
      });
    }
  }
}
