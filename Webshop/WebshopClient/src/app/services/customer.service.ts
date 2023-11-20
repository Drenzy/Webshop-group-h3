import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Customer } from '../models/customer';

@Injectable({
    providedIn: 'root'
})
export class customerService {
    private readonly apiUrl = enviorment.apiUrl + 'customer';
    constructor(private http: HttpClient) { }

    create(customer: Customer): Observable<Customer> {
        return this.http.post<Customer>(this.apiUrl, customer);
    }

    getAll(): Observable<Customer[]> {
        return this.http.get<Customer[]>(this.apiUrl);
    }

    GetAllById(loginId: number): Observable<Customer[]> {
        return this.http.get<Customer[]>(this.apiUrl + '/login/'+ loginId);
    }

    findById(customerId: number): Observable<Customer> {
        return this.http.get<Customer>(this.apiUrl + '/' + customerId)
    }

    delete(customerId: number): Observable<Customer> {
        return this.http.delete<Customer>(this.apiUrl + '/' + customerId);
    }
    
    update(customer: Customer): Observable<Customer> {
        return this.http.put<Customer>(this.apiUrl + '/' + customer.id, customer);
    }
}
