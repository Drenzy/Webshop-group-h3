import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Categories } from '../models/categories';
import { CategoryService } from '../services/category.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-category',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
     <h1>Admin Category</h1>

     <form [formGroup]="form" (ngSubmit)="save()">
<div class="formControl">
<label>Name</label>
<input type="text" formControlName="name">
<span class="error"
 *ngIf="form.get('name')?.touched && !form.get('name')?.valid">Udfyld!</span>
</div>
<button [disabled]="!form.valid || !form.touched">Gem</button>
<button type="button" (click)="cancel()">Annuller</button>
</form>
<span *ngIf="message != ''">{{ message }}</span>

    <table>
      <tr>
        <th>Handling</th>
        <th>Id</th>
        <th>Name</th>
      </tr>
      <tr *ngFor="let category of categories">
        <th>
          <button (click)="edit(category)">Ret</button>
          <button (click)="delete(category)">Slet</button>
        </th>
        <th> {{category.id}}</th>
        <th> {{category.name}}</th>
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
export class AdminCategoryComponent {
  categories: Categories[] = [];
  category: Categories = this.resetCategory();
  categoryId: number = 0;
  message: string = '';
  form: FormGroup = this.resetForm();

  constructor(private categoryService: CategoryService) { }

  resetCategory(): Categories {
    return { id: 0, name: '' };
  }

  ngOnInit(): void {
    this.categoryService.getAll().subscribe(x => this.categories = x);
  }

  resetForm(): FormGroup {
    return new FormGroup({
      name: new FormControl(null, Validators.required)
    });
  }

  delete(category: Categories): void {
    if (confirm('Er du sikker på at du vil slette ' + category.name + '?')) {
      this.categoryService.delete(category.id).subscribe(() => {
        this.categories = this.categories.filter(x => x.id != category.id)
      });
    }
  }
  cancel(): void {
    this.categoryId = 0;
    this.category = this.resetCategory();
    this.form = this.resetForm();
  }

  edit(category: Categories): void {
    this.categoryId = category.id; // grab the Id for use when saving
    this.form.patchValue(category);
  }

  save(): void {
    this.message = '';
    if (this.form.valid && this.form.touched) {
      if (this.categoryId == 0) {
        this.categoryService.create(this.form.value).subscribe({
          next: (x) => {
            this.categories.push(x);
            this.cancel();
          },
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          }
        })
      } else {
        this.category = this.form.value;
        this.category.id = this.categoryId;
        this.categoryService.update(this.category).subscribe({
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          },
          complete: () => {
            this.cancel();
            this.categoryService.getAll().subscribe(x => this.categories = x);
          }
        })
      }
    }
  }
}
