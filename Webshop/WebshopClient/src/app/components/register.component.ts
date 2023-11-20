import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ZipCode } from '../models/zipcode';
import { ZipCodeService } from '../services/zipcode.service';
import { LoginService } from '../services/login.service';
import { FormsModule } from '@angular/forms';
import { Signup } from '../models/signup';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './register.component.html',
  styles: [`
  
  `]
})
export class RegisterComponent implements OnInit {
  zipcodes: ZipCode[] = [];
  signup: Signup = this.resetSignup();

  resetSignup(): Signup {
    return {username: '', password: '', email: '',
    name: '', phoneNr: '',
    streetName: '', zipCode: 0, country: ''};
  }
  constructor(private zipcodeService: ZipCodeService,
    private loginService: LoginService) { }

  ngOnInit(): void {
    this.zipcodeService.getAll().subscribe(x => this.zipcodes = x);
  }

  save(): void {
    console.log("save()",this.signup);
    this.loginService.signup(this.signup).subscribe ({
      next: () => {
        console.log("next")
        this.signup = this.resetSignup();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
}
