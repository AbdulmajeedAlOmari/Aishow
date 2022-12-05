import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from './services/token-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Front-End';
  isLoggedIn = '';

  constructor(private tokenStorageService:TokenStorageService, private router:Router){

  }

  ngOnInit(){
    this.isLoggedIn = this.tokenStorageService.getToken();
  }

  logout(){
    this.tokenStorageService.signOut()
    console.log('logout');
    this.router.navigateByUrl('/')
    window.location.reload();
    
  }

  openModel(elementId: string){
    if(document.getElementById(elementId)){
      (document.getElementById(elementId) as HTMLInputElement).style.display='block'

    }
    // (document.getElementById(elementId) as HTMLInputElement).style.display='block'
  }
}
