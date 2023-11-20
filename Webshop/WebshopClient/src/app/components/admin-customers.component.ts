import { RouterModule } from '@angular/router';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Customer } from '../models/customer';
import { customerService } from '../services/customer.service';
import { LoginService } from '../services/login.service';
import { Login } from '../models/login';
import { Role } from '../models/role';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';



@Component({
  selector: 'app-admin-customers',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  template: `
    <p>Customers: </p>
<!-- Reactive form for Admin Login Panel -->
    <form [formGroup]="form" (ngSubmit)="save()">
  <div class="formControl">
    <h1>Login Panel</h1>
  <label>Brugernavn</label>
  <input type="text" formControlName="userName">
  <!-- a span that checks if the input is touched and is valid -->
  <span class="error"
  *ngIf="form.get('userName')?.touched && !form.get('userName')?.valid">Udfyld!</span>
  </div>

  <div class="formControl">
  <label>Kode</label>
  <input type="text" formControlName="password">
  </div>

  <div class="formControl">
  <label>Email</label>
  <input type="text" formControlName="email">
  <span class="error"
  *ngIf="form.get('email')?.touched && !form.get('email')?.valid">Udfyld!</span>
  </div>


  <label>Rolle</label><br>
  <!-- dropdown with the two roles a login can have -->
      <select formControlName="role">
        <option *ngFor="let role of Role | keyvalue" value="{{role.key}}">
          {{role.value}}</option>
      </select>

      <!-- BTN that for creating a new login and save edit for an existing login -->
      <button [disabled]="!form.valid">Gem</button>
    <button type="button" (click)="cancel()">Annuller</button>
</form>

    <!-- tables with all of the logins -->
    <table>
      <!-- <tr> defines a row of cells in a table -->
      <tr>
        <!-- <th> (header cell) elements -->
        <th>Handling</th>
        <th>Id</th>
        <th>Brugernavn</th>
        <th>Email</th>
        <th>Rolle</th>
      </tr>

      <!-- Defines a new row of cells for every login made -->
      <tr *ngFor="let login of logins">
        <th>
          <!-- calling on delete and edit function for each login -->
          <button (click)="edit(login)">Ret</button>
          <button (click)="delete(login)">Slet</button>
        </th>
        <th> {{login.id}}</th>
        <th> {{login.userName}}</th>
        <th> {{login.email}}</th>
        <th> {{login.role}}</th>
      </tr>
      </table>

      <!-- List of useres with a routerlink to see their details -->
      <h2>Brugere: </h2>
    <div *ngFor="let customer of customers">
      <p><a [routerLink]="['customer', customer.id]">Id: {{customer.id}} Name: {{customer.name}}</a></p>
    </div>
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
  `
  ]
})
export class AdminUsersComponent {
  // importing models
  customers: Customer[] = [];
  logins: Login[] = [];
  Role = Role;
  login: Login = this.resetLogin();
  form: FormGroup = this.resetForm();
  loginId: number = 0;
  message: string = '';

  //importing services and making variables in constructor
  constructor(private customerService: customerService,
    private loginService: LoginService) { }

  // reset form method for each input that also makes them required
  resetForm(): FormGroup<any> {
    return new FormGroup({
      userName: new FormControl(null, Validators.required),
      password: new FormControl(null),
      email: new FormControl(null, Validators.required),
      role: new FormControl(null, Validators.required)
    });
  }


  resetLogin(): Login {
    return { id: 0, userName: '', password: '', email: '', role: Role.Admin };
  }


  // get all customers and logins 
  ngOnInit(): void {
    this.customerService.getAll().subscribe(x => this.customers = x);
    this.loginService.getAll().subscribe(x => this.logins = x);
  }

  // edit method
  edit(login: Login): void {
    this.loginId = login.id; // grab the Id for use when saving
    // patching the login object to the database
    this.form.patchValue(login);
  }

  // delete method
  delete(login: Login): void {
    if (confirm('Er du sikker på at du vil slette ' + login.userName + '?')) {
      // calling the delte method from the service filtering from the loginId
      this.loginService.delete(login.id).subscribe(() => {
        this.logins = this.logins.filter(x => x.id != login.id)
      });
    }
  }

  cancel(): void {
    // sets the login id to null and resets the form and login
    this.loginId = 0;
    this.login = this.resetLogin();
    this.form = this.resetForm();
  }

  save(): void {
    this.message = '';
    if (this.form.valid && this.form.touched) {
      if (this.loginId == 0) {
        // calling create method from the service and subscribing
        this.loginService.create(this.form.value).subscribe({
          next: (x) => {
            // push the changes into the database
            this.logins.push(x);
            this.cancel();
          },
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          }
        })
      } else {
        this.login = this.form.value;
        this.login.id = this.loginId;
        this.loginService.update(this.login).subscribe({
          error: (err) => {
            this.message = Object.values(err.error.errors).join(', ');
          },
          complete: () => {
            this.cancel();
            this.loginService.getAll().subscribe(x => this.logins = x);
          }
        })
      }
    }
  }
}
