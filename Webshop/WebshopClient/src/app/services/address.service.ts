import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Address } from '../models/address';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private readonly apiUrl = enviorment.apiUrl + 'address';
  constructor(private http: HttpClient) { }

  getAll(): Observable<Address[]> {
    return this.http.get<Address[]>(this.apiUrl);
  }

  findById(addressId: number): Observable<Address> {
    return this.http.get<Address>(this.apiUrl + '/' + addressId)
  }

  create(address: Address): Observable<Address> {
    return this.http.post<Address>(this.apiUrl, address);
  }

  update(address: Address): Observable<Address> {
    return this.http.put<Address>(this.apiUrl + '/' + address.id, address);
  }
}
