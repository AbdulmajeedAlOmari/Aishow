import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import IUser from '../models/user.model';
import { AuthService } from '../services/auth.service';
import { TokenStorageService } from '../services/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  inSubmission: boolean = false;
  showAlert: boolean = false;
  alertMsg: string = '';
  alertColor: string = '';
  isSuccessful = false;
  isSignUpFailed = false;
  isLoggedIn = false;
  isLoginFailed = false;
  roles: string[] = [];


  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.min(8)]),
  });

  constructor(private auth: AuthService, private tokenStorage:TokenStorageService) { }

  ngOnInit(): void {
  }

  switchToRegisterModel(elementId: string) {
    (document.getElementById(elementId) as HTMLInputElement).style.display =
      'none';
    (document.getElementById('id01') as HTMLInputElement).style.display =
      'block';
    console.log('login');
  }

  closeModel(elementId: string){
    (document.getElementById(elementId) as HTMLInputElement).style.display='none'
  }

  openModel(elementId: string){
    (document.getElementById(elementId) as HTMLInputElement).style.display='block'
  }

  login(event:Event){
    this.auth.login(this.loginForm.value as IUser).subscribe(
      data => {
        this.tokenStorage.saveToken(data.token);
        this.tokenStorage.saveUser(data);

        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.inSubmission = true;
        // this.roles = this.tokenStorage.getUser().roles;
         this.reloadPage();
      },
      err => {
        this.alertMsg = 'something went wrong';
        this.isLoginFailed = true;
        this.inSubmission = false;
      }
    )
    
  }

  reloadPage(): void {
    window.location.reload();
  }

}
