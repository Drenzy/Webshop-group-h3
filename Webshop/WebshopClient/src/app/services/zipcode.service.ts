import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { ZipCode } from '../models/zipcode';

@Injectable({
  providedIn: 'root'
})
export class ZipCodeService {
  private readonly apiUrl = enviorment.apiUrl + 'zipcode';
  constructor(private http: HttpClient) { }

  getAll(): Observable<ZipCode[]> {
      return this.http.get<ZipCode[]>(this.apiUrl);
    }
  
    findById(zipcodeId: number): Observable<ZipCode> {
      return this.http.get<ZipCode>(this.apiUrl + '/' + zipcodeId)
    }
}
