import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {

  @Input() color: string = '';

  get bgColor() {
    return `alert-${this.color}`;
  }

  constructor() { }

  ngOnInit(): void {
  }

}
