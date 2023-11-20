import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { enviorment } from '../enviorments/environment';
import { SignIn } from '../models/signin';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentLoginSubject: BehaviorSubject<SignIn>;
  currentLogin: Observable<SignIn>

  constructor(private http: HttpClient) {

    this.currentLoginSubject = new BehaviorSubject<SignIn>(
      JSON.parse(sessionStorage.getItem('currentlogin') as string)
    );
    this.currentLogin = this.currentLoginSubject.asObservable();

  }

  public get currentLoginValue(): SignIn {
    return this.currentLoginSubject.value;
  }

  login(email: string, password: string) {
    let authenticateUrl = `${enviorment.apiUrl}Login/authenticate`;
    return this.http.post<any>(authenticateUrl, { "email": email, "password": password })
      .pipe(map(login => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        sessionStorage.setItem('currentlogin', JSON.stringify(login));
        this.currentLoginSubject.next(login);
       // console.log(login);
        return login;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    sessionStorage.removeItem('currentlogin');
    // reset currentLoginSubject, by fetching the value in sessionStorage, which is null at this point
    this.currentLoginSubject = new BehaviorSubject<SignIn>(JSON.parse(sessionStorage.getItem('currentLogin') as string));
    // reset currentLogin to the resat LoginSubject, as an obserable
    this.currentLogin = this.currentLoginSubject.asObservable();
  }

}
