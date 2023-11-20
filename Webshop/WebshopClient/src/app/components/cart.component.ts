import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderItem } from '../models/orderitem';
import { CartService } from '../services/cart.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router'
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './cart.component.html',
  styles: [
  ]
})
export class CartComponent implements OnInit {
  cartItems: OrderItem[] = [];
  amount: number = 1;
  currentUser = this.authService.currentLoginValue;
  
  constructor(public cartService: CartService, public router: Router, private authService: AuthService) {
    

  }
  
  nextStep(): void{
    this.currentUser = this.authService.currentLoginValue;
    if(this.currentUser != null ){
      this.router.navigateByUrl('/cart/address');
    }else{
      alert("Du skal login!");
    }
  }

  ngOnInit(): void {
    this.cartService.currentBasket.subscribe(x => this.cartItems = x);  
  }

  clearCart(): void {
    this.cartService.clearBasket();
  }

  updateCart(): void {
    this.cartService.saveBasket(this.cartItems);
  }

  removeItem(item: OrderItem): void {
    if (confirm(`Er du sikker på du vil fjerne ${item.productId} ${item.name}?`)) {
      this.cartItems = this.cartItems.filter(x => x.productId != item.productId);
      this.cartService.saveBasket(this.cartItems);
    }
  }
}
