import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Status } from '../models/status';

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private readonly apiUrl = enviorment.apiUrl + 'status';
  constructor(private http: HttpClient) { }

  getAll(): Observable<Status[]> {
      return this.http.get<Status[]>(this.apiUrl);
    }
  
    findById(statusId: number): Observable<Status> {
      return this.http.get<Status>(this.apiUrl + '/' + statusId)
    }
}
