import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-users-board',
  templateUrl: './users-board.component.html',
  styleUrls: ['./users-board.component.css']
})
export class UsersBoardComponent implements OnInit {

  constructor(private userService:UserService) { }

  ngOnInit(): void {
    this.userService.getUsers().subscribe(console.log)
  }

}
