import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Order } from '../models/order';
import { OrderItem } from '../models/orderitem';
import { OrderService } from '../services/order.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Address } from '../models/address';
import { Customer } from '../models/customer';
import { Status } from '../models/status';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <p>
      order-detail works!
    </p>

    <h1>OrderId: {{order.id}}</h1>
    <p>Dato: {{order.orderDate}}</p>
    <p>Kunde Navn: {{order.customer?.name}}</p>
    <p>Kunde telefon: {{order.customer?.phoneNr}}</p>
    <p>Vej: {{order.address?.streetName}}</p>
    <p>Postnummer: {{order.address?.zipCodeId}}</p>
    <p>By: {{order.address?.city}}</p>
    <p>Land: {{order.address?.country}}</p>
    <p>Status: {{order.status?.name}}</p>
    <h2>Samlet price: {{ getTotal() | currency:'kr.'}}</h2>
    <table>
      <tr>
        <th class="first">Id</th>
        <th>Navn</th>
        <th>Price</th>
        <th>Antal</th>
      </tr>
      <tr *ngFor="let orderItem of order.orderItems" class="items">
        <th class="first">{{orderItem.id}}</th>
        <th>{{orderItem.name}}</th>
        <th>{{orderItem.price}} </th>
        <th>{{orderItem.quantity}}</th>
      </tr>
    </table>

    <a routerLink="/admin/order">Back</a>
  `,
  styles: [`
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
export class OrderDetailComponent implements OnInit {
  order: Order = {
    id: 0,
    customerId: 0,
    addressId: 0, 
    orderDate: new Date(),
    address: this.resetAddress(),
     customer: this.resetCustomer(),
     orderItems: this.resetOrderItems(),
     status: this.resetStatus()
  };
  sum: number = 0;

  constructor(private orderService: OrderService, 
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.orderService.findById(Number(params.get('orderId')))
      .subscribe(x => this.order = x);
    })
  }
  
  resetOrderItems(): OrderItem[] {
    return [];
  }
  resetAddress(): Address {
    return {id:0, streetName:'', zipCodeId:0, city: '', country:''}
  }

  resetCustomer(): Customer {
    return {id:0, name:'', phoneNr:''}
  }

  resetStatus(): Status {
    return {id:0, name:''}
  }

  getTotal(): number {
    let total: number = 0;
    this.order.orderItems?.forEach(item => {
      total += item.price * item.quantity;
    });
    return total;
  }

}
