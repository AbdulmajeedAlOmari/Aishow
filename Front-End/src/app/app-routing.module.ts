import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuardService } from './services/auth-guard.service';
import { UserService } from './services/user.service';
import { UsersBoardComponent } from './users-board/users-board.component';

const routes: Routes = [
  {path:'register' ,component:RegisterComponent},
  {path:'login',component:LoginComponent},
  {path:'user', component:UsersBoardComponent, canActivate: [AuthGuardService]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
