import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { OrderItem } from '../models/orderitem';
import { CartService } from  '../services/cart.service'

@Component({
  selector: 'app-prodcut-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <p>
      prodcut-detail works!
    </p>

    <h1>{{product.name | uppercase}}</h1>
    <p> {{product.description}} </p>
    <p> Pris: {{product.price}}kr. </p>
    <button (click)="addToCart()" class="btn btn-primary btn-sm">Put {{ product.name }} i kurven</button><br>
    <a routerLink="/">Forside</a>
  `,
  styles: [
  ]
})
export class ProdcutDetailComponent implements OnInit {
  product: Product = {id: 0, name: '', price: 0, description:'', categoryId:0}
  constructor(private productService: ProductService, private route: ActivatedRoute, private cartService: CartService) {}
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.productService.findById(Number(params.get('productId')))
      .subscribe(x => this.product = x);
    })
  }

  addToCart(item?: OrderItem): void {
    if(item == null){
      item = {
        productId: this.product.id,
        price: this.product.price,
        quantity: 1,
        name: this.product.name,
      } as OrderItem;
    }

    this.cartService.addToBasket(item);
  }
}
