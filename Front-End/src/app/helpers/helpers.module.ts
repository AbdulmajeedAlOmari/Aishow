import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { AlertComponent } from "./alert/alert.component";
import { InputComponent } from "./input/input.component";
import { NgxMaskModule } from 'ngx-mask';

@NgModule({
    declarations: [
      InputComponent,
      AlertComponent,
      
    ],
    imports: [
      CommonModule,
      ReactiveFormsModule,
      NgxMaskModule.forRoot()
    ],
    exports:[
        InputComponent,
        AlertComponent
  
    ]
  })
  export class HelpersModule { }
  