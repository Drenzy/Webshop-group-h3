import { AddressService } from './../services/address.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Customer } from '../models/customer';
import { customerService } from '../services/customer.service';
import { Address } from '../models/address';
import { Login } from '../models/login';
import { FormsModule } from '@angular/forms';
import { ZipCodeService } from '../services/zipcode.service';
import { ZipCode } from './../models/zipcode';

@Component({
  selector: 'app-admin-customers-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
    <h1>id: {{ customer.id }} name: {{ customer.name }}</h1>
    <p>phonenumber: {{ customer.phoneNr }}</p>
    <p>country: {{ customer.addresses?.country }}</p>
    <p>streetName: {{ customer.addresses?.streetName }}</p>
    <p>city: {{ customer.addresses?.city }}</p>
    <p>email: {{ customer.logins?.email }}</p>
    <p>username: {{ customer.logins?.userName }}</p>

    <h2>Edit info here:</h2>
    <label>Name:</label><br>
    <input type="text" [(ngModel)]="customer.name"><br>
    <label>phonenumber:</label><br>
    <input type="text" [(ngModel)]="customer.phoneNr"><br>
    <label>streetName:</label><br>
    <input type="text" [(ngModel)]="customer.addresses!.streetName"><br>
    <!-- Want to make it an drop down but didn't have enough time to make it work, 
    and we should show the city instead of id here but couldn't make it work. -->
    <label>city:</label><br>
    <input type="text" [(ngModel)]="customer.addresses!.city"><br>
    <button (click)="Save()">Save Changes</button>
  `,
  styles: [],
})
export class AdminCustomersDetailComponent implements OnInit {
  customer = this.resetCustomer();
  message: string = '';
  customers: Customer[] = [];
  address = this.resetAddress();
  addresses: Address[] = [];
  zipCode = this.resetZipCode();
  zipCodes: ZipCode[] = [];

  resetZipCode(): ZipCode{
    return {id: 0, city: ''};
  }

  resetAddress(): Address {
    return { id: 0, streetName: '', zipCodeId: 0, country: '', city: '' };
  }

  resetLogin(): Login {
    return { id: 0, userName: '', password: '', email: '' };
  }

  resetCustomer(): Customer {
    return {     id: 0,
      name: '',
      phoneNr: '',
      addressId: 0,
      loginId: 0,
      logins: this.resetLogin(),
      addresses: this.resetAddress() }
  }

  constructor(
    private customerService: customerService,
    private route: ActivatedRoute,
    private addressService: AddressService,  
    private zipcodeService: ZipCodeService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.customerService
        .findById(Number(params.get('customerId')))
        .subscribe((x) => {
          this.customer = x;
        });
    });
  }

  Save() {
    this.customerService.update(this.customer).subscribe({
      error: (err) => {
        console.error(Object.values(err.error.errors).join(', '));
      },
      complete: () => {
        // this.customer = this.resetCustomer();
        this.customerService.getAll().subscribe(x => this.customers = x);
      }
    });
    this.addressService.create(this.customer.addresses!).subscribe({
      error: (err) => {
        console.error(Object.values(err.error.errors).join(', '));
      },
      complete: () => {
        // this.customer.addresses = this.resetAddress();
        this.addressService.getAll().subscribe(x => this.addresses = x);
      }
    });
    //does not work
    this.zipcodeService.findById(this.zipCode.id).subscribe({
            error: (err) => {
        console.error(Object.values(err.error.errors).join(', '));
      },
      complete: () => {
        // this.zipCode = this.resetZipCode();
        this.zipcodeService.getAll().subscribe(x => this.zipCodes = x);
      }
    });
  }
}
