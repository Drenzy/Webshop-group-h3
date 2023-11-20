import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { Login } from '../models/login';
import { Signup } from '../models/signup';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private readonly apiUrl = enviorment.apiUrl + 'login';
  private readonly apiUrlRegister = enviorment.apiUrl + 'login/register';
  constructor(private http: HttpClient) { }

  signup(signup: Signup): Observable<Login> {
    console.log("service.signup", this.apiUrlRegister)
    return this.http.post<Login>(this.apiUrlRegister, signup);
  }

  getAll(): Observable<Login[]> {
    return this.http.get<Login[]>(this.apiUrl);
  }


  findById(loginId: number): Observable<Login> {
    return this.http.get<Login>(this.apiUrl + '/' + loginId)
  }

  create(login: Login): Observable<Login> {
    return this.http.post<Login>(this.apiUrl, login);
  }

  delete(loginId: number): Observable<Login> {
    return this.http.delete<Login>(this.apiUrl + '/' + loginId);
  }

  update(login: Login): Observable<Login> {
    return this.http.put<Login>(this.apiUrl + '/' + login.id, login);
  }
}
