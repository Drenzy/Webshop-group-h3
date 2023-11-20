import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Order } from '../models/order';
import { OrderService } from '../services/order.service';
import { StatusService } from '../services/status.service';
import { Status } from '../models/status';

@Component({
  selector: 'app-order-admin',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
  <p>Orders</p>
  <span>Søg efter OrderId</span>
  <span>Søg efter CustomerId</span>
  <span id="last">Søg efter Status:</span><br>
  <input type="number" placeholder="Søg efter OrderId" [(ngModel)]="searchOrderId" (keyup)="SendData()">
  <input type="number" placeholder="Søg efter CustomerId" [(ngModel)]="searchCustomerId" (keyup)="SendData()">
  <select [(ngModel)]="statussearch" (change)="SendData()"> 
    <option *ngFor="let status of statuses" value="{{status.id}}" >{{status.id}} </option>
  </select>
  <button (click)="resetForm()">Reset</button>

    <table>
      <tr>

        <th class="first">Details</th>
        <th>Id</th>
        <th>Customer Id</th>
        <th>Address Id</th>
        <th>Order Date</th>
        <th>Status Id</th>
        <th>Change status</th>
      </tr>

      <tr *ngFor="let order of orders">
        <th class="first">
          <button><a [routerLink]="['Order', order.id]">Details</a></button>
        </th> 
        <th> {{order.id}}</th>
        <th> {{order.customerId}}</th>
        <th> {{order.addressId}}</th>
        <th> {{order.orderDate}}</th>
        <th> {{order.statusId}}</th>
        <th> <select (change)="onChange(order.id, $event)">
            <option>{{order.statusId}}</option>
            <option *ngFor="let status of statuses" value="{{status.id}}">
              {{status.id}}, {{status.name}}</option>
          </select>
        </th>
      </tr>
    </table>
  `,
  styles: [`
  table{
    width: 100%;
    border-radius: 10px;
    border-collapse: collapse;
    background-color: #c2d6d6;
    text-align: center; 
  }
  th{
    padding: 5px;
    border-collapse: collapse;
    border-left: 2px solid #e0ebeb;
  }
  .first{
    border-style: none;
  }
  button{
    border-radius: 4px;
    border-style: none;
    background-color: #75a3a3;
  }
  button.a,  a:link, a:visited, a:hover, a:active{
    color: black;
    text-decoration: none;
  }
  span{
    padding: 25px;
  }
  #last{
    padding-left: 0px;
  }
  input{
    margin: 5px;
  }
  `
  ]
})
export class OrderAdminComponent implements OnInit {
  orderId: number = 0;
  orders : Order[] = [];
  order: Order = this.resetOrder();
  statuses: Status[] = [];
  message: string = '';
  
  status: Status[] = [];
  statussearch: number = 0;
  data: Order[] = [];
  searchOrderId: number = 0;
  searchCustomerId: number = 0;
  
  constructor(private orderService: OrderService,
    private statusService: StatusService) { }

  ngOnInit(): void {
    // this.orderService.getAll().subscribe(x => this.orders = x);
    this.orderService.getAll().subscribe(x => {
      this.data = x;
      this.orders = this.data;
    });
    this.statusService.getAll().subscribe(x => this.statuses = x);
  }



  resetOrder(): Order {
    return { id: 0,
      customerId: 0,
      addressId: 0,
      statusId: 0,};
  }

  onChange(orderid: number, statusid: any){
    console.log("a")
    this.updateStatus(orderid, statusid.target.value)
  }

  updateStatus(orderId: number, statusId: number): void{
    console.log(orderId, statusId)
    this.orderService.updateStatus(orderId, statusId).subscribe({
      next: (x) =>{
        console.log("Status Updated", x)
        this.orderService.getAll().subscribe(x => this.orders = x);
      },
      error: (err) => {
        this.message = Object.values(err.error.errors).join(', ');
      }
    })
  }

  //resets the form to its initial state
  resetForm(){
    this.statussearch = 0;
    this.searchOrderId = 0;
    this.searchCustomerId = 0;
    this.ngOnInit();
  }

  SendData(): void {
    if(this.searchCustomerId != 0 || this.searchOrderId != 0 || this.statussearch != 0){
      this.orders = this.data;
      if( this.statussearch != 0){
        this.orders = this.orders.filter(x => x.statusId == this.statussearch);
      }
      if(this.searchOrderId != 0){
        this.orders = this.orders.filter(x => x.id == this.searchOrderId)
      }
      if(this.searchCustomerId != 0){
        this.orders = this.orders.filter(x => x.customerId == this.searchCustomerId)
      }
  }
  }
}
