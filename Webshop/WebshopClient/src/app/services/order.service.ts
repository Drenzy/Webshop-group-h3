import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly apiUrl = enviorment.apiUrl + 'Order';
  constructor(private http: HttpClient) { }

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  findById(orderId: number): Observable<Order> {
    return this.http.get<Order>(this.apiUrl + '/' + orderId)
  }

  create(order: Order): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }

  update(order: Order): Observable<Order> {
    return this.http.put<Order>(this.apiUrl + '/' + order.id, order);
  }

  updateStatus(orderId: number, statusId: number): Observable<Order>{
    return this.http.patch<Order>(this.apiUrl + '/' + orderId + '/' + statusId, {})
  }
}
