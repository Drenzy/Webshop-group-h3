import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Address } from '../models/address';
import { ZipCode } from '../models/zipcode';
import { AddressService } from '../services/address.service';
import { ZipCodeService } from '../services/zipcode.service';
import { OrderService } from '../services/order.service';
import { Router, RouterModule } from '@angular/router'
import { CartService } from '../services/cart.service';
import { Order } from '../models/order';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-address-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
  <div [ngClass]="this.isordered? 'hidden' : 'display'">
    <div class="outline">
      <label>Ship to customer address</label>
      <div [ngClass]="this.newAddress? 'hidden' : 'display'"><br>
        <form [formGroup]="addressform" id="CustomerAddress">
          <div class="">
            <label>Streetname</label><br>
            <input type="text" disabled value="{{customerAddress.streetName}}">
          </div>

          <div class="formControl">
            <label>Postnummer</label><br>
            <select  formControlName="zipcodeId">
              <option class="closed" disabled value="0">{{customerAddress.zipCodeId}}, {{customerAddress.city}}</option>
            </select>

          </div>

          <div class="">
            <label>Country</label><br>
            <input type="text" disabled value="{{customerAddress.country}}">
          </div>
        </form>
      </div><br>
    </div>


    <div class="outline">

      <label>Ship to another address</label><br>
      <input type="checkbox" ng-model="newAddress" (click)="ShowNewAddress()">

      <div [ngClass]="this.newAddress? 'display' : 'hidden'"><br>

        <form [formGroup]="form" (ngSubmit)="buy()">
          <div class="formControl">

            <label>Streetname</label><br>
            <input type="text" formControlName="streetName">
            <span class="error"*ngIf="form.get('streetName')?.touched && !form.get('streetName')?.valid">Udfyld!</span>
          </div>

          <div class="formControl">
            <label>Postnummer</label><br>
            <select  formControlName="zipcodeId">
              <option value="0">Vælg en ZipCode</option>
              <option *ngFor="let zipcode of zipcodes" value="{{zipcode.id}}">
                {{zipcode.id}}, {{zipcode.city}}</option>
            </select>
            <span class="error" *ngIf="form.get('zipcodeId')?.touched && !form.get('zipcodeId')?.valid">Udfyld!</span>
          </div>

          <div class="formControl">
            <label>Country</label><br>
            <input type="text" formControlName="country">
            <span class="error"
            *ngIf="form.get('country')?.touched && !form.get('country')?.valid">Udfyld!</span>
          </div>

          <button type="submit" hidden #btn>a</button>
          <br>
        </form>
      </div>
    </div>

    <button (click)="backToCart()">Tilbage</button>
    <button (click)='newAddress? btn.click() : buy()'>Køb</button>
  </div>
    
  <div [ngClass]="this.isordered? 'display' : 'hidden'">
    <h1>Your order is placed</h1>
    <h2>Samlet price: {{ getTotal() | currency:'kr.'}}</h2>
    <table>
      <tr>
        <th class="first">Navn</th>
        <th>Price</th>
        <th>Antal</th>
      </tr>
      <tr *ngFor="let orderItem of order.orderItems" class="items">
        <th class="first">{{orderItem.name}}</th>
        <th>{{orderItem.price}} </th>
        <th>{{orderItem.quantity}}</th>
      </tr>
    </table>
    <h2><a routerLink="/">Back to the homepage</a></h2>
  </div>

  `,
  styles: [`
  .formControl label{
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
  .outline{
    border: 2px solid blue;
    padding: 10px;
    margin: 10px;
  }
  #NewAddress {
    display: none;
  }
  .display{
    display: block;
  }
  .hidden{
    display: none;
  }
  table{
    width: 100%;
    border-radius: 10px;
    border-collapse: collapse;
    background-color: #b3daff;
  }
  th{
    padding: 5px;
    border-collapse: collapse;
    border-left: 2px solid #cce6ff;
  }
  .first{
    border-style: none;
  }
  .items{
    border-top: 2px solid #cce6ff;
  }
  
  `
  ]
})
export class AddressCheckoutComponent implements OnInit {
  addresses: Address[] = [];
  zipcodes: ZipCode[] = [];
  address: Address = this.resetAddress();
  zipcode: ZipCode = this.resetZipcode();
  message: string = '';
  form: FormGroup = this.resetForm();
  addressform: FormGroup = this.addressForm();
  customerAddress: Address = this.resetAddress();
  newAddress: boolean = false;
  isordered: boolean = false;
  currentUser = this.authService.currentLoginValue;
  order: Order = this.resetOrder();

  constructor(private addressService: AddressService,
    private zipcodeService: ZipCodeService, 
    private router: Router, 
    public cartService: CartService,
    private orderService: OrderService,
    private authService: AuthService) {}

    ShowNewAddress(): void{
      if(this.newAddress == false){
        this.newAddress = true;
      }else{
        this.newAddress = false;
      }
      
      console.log(this.newAddress)
    }

  backToCart(): void{
    this.router.navigateByUrl('/cart')
  }

  resetAddress(): Address {
    return { id: 0,
      streetName: '',
      zipCodeId: 0,
      country:'' ,
      city: '', };
  }

  resetZipcode(): ZipCode {
    return { id: 0, city: '' };
  }

  resetOrder(): Order {
    return { id: 0,
      statusId:0,};
  }


  ngOnInit(): void {
    this.zipcodeService.getAll().subscribe(x => this.zipcodes = x);
    this.addressService.findById(this.authService.currentLoginValue.addressId||0).subscribe(x => this.customerAddress = x)

  }

  addressForm(): FormGroup{
    return new FormGroup({
      zipcodeId: new FormControl(this.address.zipCodeId)
  })}

  resetForm(): FormGroup {
    return new FormGroup({
      streetName: new FormControl(null, Validators.required),
      zipcodeId: new FormControl(0, Validators.required),
      country: new FormControl(null, Validators.required),
      address: new FormControl(0, Validators.required)
    });
  }
  
  cancel(): void {
    this.address = this.resetAddress();
    this.form = this.resetForm();
    this.order = this.resetOrder();
  }

  ordered(order: Order) : void{
    this.order = order;
  }

  getTotal(): number {
    let total: number = 0;
    this.order.orderItems?.forEach(item => {
      total += item.price * item.quantity;
    });
    return total;
  }

  clearCart(): void {
    this.cartService.clearBasket();
  }

  buy(): void {
    this.message = '';
    this.cartService.currentBasketValue

    let order = {
      id: 0,
      orderitems: this.cartService.currentBasketValue,
      customerid: this.authService.currentLoginValue.customerId} as Order;

      if(this.newAddress){
        order.address = this.form.value;
      }else{
        order.addressId = this.authService.currentLoginValue.addressId;
        order.address = this.resetAddress();
      }

    this.orderService.create(order).subscribe({
      next: (x) => {
        console.log("Created",x);
        this.isordered = true;
        this.ordered(x);
        this.clearCart();
      },
      error: (err) =>{
        console.error(err);
        console.log(order);
        console.log(this.authService.currentLoginValue)
      }
    })
  }
}
