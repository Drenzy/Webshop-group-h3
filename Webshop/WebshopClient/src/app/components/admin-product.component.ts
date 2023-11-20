import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../models/product';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../services/product.service';
import { CategoryService } from '../services/category.service';
import { Categories } from '../models/categories';

@Component({
  selector: 'app-admin-product',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <h1>Admin Product</h1>
<form [formGroup]="form" (ngSubmit)="save()">
  <div class="formControl">
    <label>Navn</label>
    <input type="text" formControlName="name">
    <span class="error"
 *ngIf="form.get('name')?.touched && !form.get('name')?.valid">Udfyld!</span>
  </div>

  <div class="formControl">
    <label>Pris</label>
    <input type="text" formControlName="price">
    <span class="error"
    *ngIf="form.get('price')?.touched && !form.get('price')?.valid">Udfyld!</span>
  </div>

  <div class="formControl">
    <label>Beskrivelse</label>
    <input type="text" formControlName="description">
    <span class="error"
    *ngIf="form.get('description')?.touched && !form.get('description')?.valid">Udfyld!</span>
  </div>

  <div class="formControl">

      <label>Kategori</label><br>
      <select formControlName="categoryId">
        <option value="0">Vælg en Kategori</option>
        <option *ngFor="let category of categories" value="{{category.id}}">
          {{category.name}}</option>
      </select>
      
      <span class="error" *ngIf="form.get('productId')?.touched && 
      !form.get('productId')?.valid">Udfyld!</span>
    </div>
    <br>
    <button [disabled]="!form.valid || !form.touched">Gem</button>
    <button type="button" (click)="cancel()">Annuller</button>
</form>

<span *ngIf="message != ''">{{ message }}</span>

    <table>
      <tr>

        <th>Handling</th>
        <th>Id</th>
        <th>Name</th>
        <th>Pris</th>
        <th>Beskrivelse</th>
        <th>Kategori ID</th>
      </tr>

      <tr *ngFor="let product of products">
        <th>
          <button (click)="edit(product)">Ret</button>
          <button (click)="delete(product)">Slet</button>
        </th>
        <th> {{product.id}}</th>
        <th> {{product.name}}</th>
        <th> {{product.price}}</th>
        <th> {{product.description}}</th>
        <th> {{product.categoryId}}</th>
      </tr>
    </table>
  `,
  styles: [`
  .formControl label{
    display: inline-block;
    width:85px;
    text-align:right;
    }
    .formControl{
    margin:7px 0;
  }
    input[type='text'], textarea{
    width:200px;
    margin:0 3px;
    border: solid 2px #333;
  }
    input.ng-invalid.ng-touched, textarea.ng-invalid.ng-touched{
    border: solid 2px #f00;
  }
  `]
})
export class AdminProductComponent implements OnInit {
  products: Product[] = [];
  categories: Categories[] = [];
  product: Product = this.resetProduct();
  category: Categories = this.resetCategory();
  productId: number = 0;
  message: string = '';
  form: FormGroup = this.resetForm();

  constructor(private productService: ProductService,
    private categoryService: CategoryService) { }

  resetProduct(): Product {
    return { id: 0, name: '', price: 0, description: '', categoryId: 0 };
  }

  resetCategory(): Categories {
    return { id: 0, name: '' };
  }

  ngOnInit(): void {
    this.productService.getAll().subscribe(x => this.products = x);
    this.categoryService.getAll().subscribe(x => this.categories = x);
  }

  resetForm(): FormGroup {
    return new FormGroup({
      name: new FormControl(null, Validators.required),
      price: new FormControl(null, Validators.required),
      description: new FormControl(null, Validators.required),
      categoryId: new FormControl(0, Validators.required)
    });
  }

  delete(product: Product): void {
    if (confirm('Er du sikker på at du vil slette ' + product.name + '?')) {
      this.productService.delete(product.id).subscribe(() => {
        this.products = this.products.filter(x => x.id != product.id)
      });
    }
  }
  cancel(): void {
    this.productId = 0;
    this.product = this.resetProduct();
    this.form = this.resetForm();
  }

  edit(product: Product): void {
    this.productId = product.id; // grab the Id for use when saving
    this.form.patchValue(product);
  }

  save(): void {
    this.message = '';
    if (this.form.valid && this.form.touched) {
      if (this.productId == 0) {
        this.productService.create(this.form.value).subscribe({
          next: (x) => {
            this.products.push(x);
            this.cancel();
          },
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          }
        })
      } else {
        this.product = this.form.value;
        this.product.id = this.productId;
        this.productService.update(this.product).subscribe({
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          },
          complete: () => {
            this.cancel();
            this.productService.getAll().subscribe(x => this.products = x);
          }
        })
      }
    }
  }
}
