import { Injectable } from '@angular/core';
import { enviorment } from '../enviorments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly apiUrl = enviorment.apiUrl + 'product';
  constructor(private http: HttpClient) { }

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  findById(productId: number): Observable<Product> {
    return this.http.get<Product>(this.apiUrl + '/' + productId)
  }

  create(prodcut: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, prodcut);
  }

  delete(productId: number): Observable<Product> {
    return this.http.delete<Product>(this.apiUrl + '/' + productId);
  }

  update(prodcut: Product): Observable<Product> {
    return this.http.put<Product>(this.apiUrl + '/' + prodcut.id, prodcut);
  }
}
