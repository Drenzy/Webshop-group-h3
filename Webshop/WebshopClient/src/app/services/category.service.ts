import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Categories } from '../models/categories';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly apiUrl = enviorment.apiUrl + 'category';
  constructor(private http: HttpClient) { }

  getAll(): Observable<Categories[]> {
    return this.http.get<Categories[]>(this.apiUrl);
  }

  findById(categoryId: number): Observable<Categories> {
    return this.http.get<Categories>(this.apiUrl + '/' + categoryId)
  }

  create(category: Categories): Observable<Categories> {
    return this.http.post<Categories>(this.apiUrl, category);
  }

  delete(categoryId: number): Observable<Categories> {
    return this.http.delete<Categories>(this.apiUrl + '/' + categoryId);
  }

  update(category: Categories): Observable<Categories> {
    return this.http.put<Categories>(this.apiUrl + '/' + category.id, category);
  }
}
