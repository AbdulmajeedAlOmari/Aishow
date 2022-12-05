import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import IUser from '../models/user.model';
import { AuthService } from '../services/auth.service';
declare var $: any;

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  inSubmission: boolean = false;
  showAlert: boolean = false;
  alertMsg: string = '';
  alertColor: string = '';
  isSignUpFailed: boolean = false;
  openLoginModel: boolean = false;
  isSuccessful: boolean = false;
  isLoggedIn: boolean = false;

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit(): void {}

  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.min(8)]),
  });

  register(event: Event) {
    console.warn(this.registerForm.value);
    event.preventDefault();
    this.auth.register(this.registerForm.value as IUser).subscribe(
      (data) => {
        console.log(data);
        this.inSubmission = true;
        this.isSignUpFailed = false;
        console.log('register successfully');
      },

      (err) => {
        this.showAlert = true;
        this.alertColor = 'danger';
        this.alertMsg = 'Something Went Wrong';
        this.inSubmission = false;
        this.isSignUpFailed = true;
        console.log(this.alertMsg);
      }
    );
    // this.inSubmission = true;
  }

  switchToLoginModel(elementId: string) {
    let element = document.getElementById('id02');
    console.log(document.getElementById(elementId));
    console.log(document.getElementById('id02'));

    (document.getElementById(elementId) as HTMLInputElement).style.display =
      'none';

    if (element) {
      (document.getElementById('id02') as HTMLInputElement).style.display =
        'block';
    }
  }
  closeModel(elementId: string) {
    (document.getElementById(elementId) as HTMLInputElement).style.display =
      'none';
  }

  openModel(elementId: string) {
    (document.getElementById(elementId) as HTMLInputElement).style.display =
      'block';
  }
}
