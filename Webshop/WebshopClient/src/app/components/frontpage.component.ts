import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { CategoryService } from '../services/category.service';
import { Categories } from '../models/categories';

@Component({
  selector: 'app-frontpage',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule,
  FormsModule],
  template: `
  
    <!-- filters out the list of products depending on the search -->
    <input type="text" placeholder="Søg efter items..." [(ngModel)]="search" (keyup)="sendData()">
    <!-- filters out the list of products depending on the minimum value of the lowest value item -->
    <p>Indtast minimum pris</p>
    <input type="number" [(ngModel)]="minsearch" (keyup)="sendData()">
    <!-- filters out the list of products depending on the max value of the highest value item -->
    <p>Indtast maksimum pris</p>
    <input type="number" [(ngModel)]="maxsearch" (keyup)="sendData()"><br><br>
    <!-- filters out the list of products depending on the category -->
    <label>Vælg Kategori du vil sortere fra</label><br>
    <select [(ngModel)]="categorySearch" (change)="sendData()">
      <option *ngFor="let category of categories" value="{{category.id}}">{{category.name}} </option>
    </select> <br><br>
    <!-- calls a resetform method to reset the form -->
    <button (click)="resetForm()">Reset</button>

    
    <!-- shows a list of products in the database -->
    <p>Produkter: </p>
    <div *ngFor="let product of products">
      <p><a [routerLink]="['product', product.id]">{{product.name}}</a></p>
    </div>
  `,
  styles: [
  ]
})

export class FrontpageComponent implements OnInit {

  search: string = '';
  minsearch: number = 0;
  maxsearch: number = 100;
  products: Product[] = [];
  data: Product[] = [];
  categories: Categories[] = [];
  category: Categories[] = [];
  categorySearch: number = 0;

  constructor(private productService: ProductService, private categoryService: CategoryService) {}
  
  ngOnInit(): void {
    //gets products and max price
    this.productService.getAll().subscribe(x => {
      this.data = x;
      this.products = this.data;
      this.maxsearch = this.getMaxPrice();
    });
    this.categoryService.getAll().subscribe(x => this.categories = x);
  }
  //searches for max price of all products
  getMaxPrice(): number{
    let Max = 0;
    this.data.forEach(x => {
    if(Max < x.price){
      Max = x.price;
    }
    });
    return Max;
  }
  //resets the form to its initial state
  resetForm(){
    this.search = '';
    this.minsearch = 0;
    this.maxsearch = this.getMaxPrice();
    this.categorySearch = 0;
    this.sendData();
  }

  sendData() {
    //filters out the list according to the minimum price and maximum price
    this.products = this.data.filter(x => (x.price >= this.minsearch && x.price <= this.maxsearch))
    //filters out the list according to the input text for the name and description
    if(this.search != ""){
      this.products = this.products.filter(x => (x.name.toLocaleLowerCase().indexOf(this.search.toLocaleLowerCase()) > -1 
      || x.description.toLocaleLowerCase().indexOf(this.search.toLocaleLowerCase()) > -1))
    }
    //filters out the list according to the category chosen
    if(this.categorySearch != 0){
      this.products = this.products.filter(x => (x.categoryId == this.categorySearch))
    }

    //shows what is outputted in the console
    console.log(this.products);
  }
}