import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title:string = 'HomeworksAngular';
  name:string = 'Santiago Mendez';
  email:string = 'santi17mendez@hotmail.com';
  address = {
    street: "la direccion del profe",
    city: "Montevideo",
    number: 1234
  };
}

