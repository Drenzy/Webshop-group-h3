import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrderItem } from '../models/orderitem';

@Injectable({
  providedIn: 'root'
})

export class CartService {


  private basketName = "WebShopBasket";

  // this is the current cartItems stored in basket
  currentBasketSubject: BehaviorSubject<OrderItem[]>;

  // this is the observable basket others can subscribe to
  currentBasket: Observable<OrderItem[]>;

  //Grabs waht is stored in the local storage or
  //create and empty array if storage is empty
  constructor() {
    this.currentBasketSubject = new BehaviorSubject<OrderItem[]>(
      JSON.parse(localStorage.getItem(this.basketName) || "[]")
    );
    this.currentBasket = this.currentBasketSubject.asObservable();
  }

  get currentBasketValue(): OrderItem[] {
    return this.currentBasketSubject.value;
  }

  saveBasket(basket: OrderItem[]): void {
    localStorage.setItem(this.basketName, JSON.stringify(basket));
    this.currentBasketSubject.next(basket);
  }

  addToBasket(item: OrderItem): void {
    let productFound = false;
    let basket = this.currentBasketValue;

    basket.forEach(basketItem => {
      if (basketItem.productId == item.productId) {
        basketItem.quantity += item.quantity;
        productFound = true;
        if (basketItem.quantity <= 0) {
          this.removeItemFromBasket(item.productId);
        }
      }
    });

    if(!productFound){
      basket.push(item);
    }
    this.saveBasket(basket);
  }

  removeItemFromBasket(productId: number): void {
    let basket = this.currentBasketValue;
    for (let i = basket.length; i >= 0; i--) {
      if (basket[i].productId == productId) {
        basket.splice(i, 1);
      }
    }
    this.saveBasket(basket);
  }

  clearBasket(): void {
    let basket: OrderItem[] = [];
    this.saveBasket(basket);
  }

  getBasketTotal(): number {
    let total: number = 0;
    this.currentBasketSubject.value.forEach(item => {
      total += item.price * item.quantity;
    });
    return total;
  }
}
