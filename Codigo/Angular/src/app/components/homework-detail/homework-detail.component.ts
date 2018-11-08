import { Component, OnInit } from '@angular/core';
import { Homework } from '../../models/Homework';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-homework-detail',
  templateUrl: './homework-detail.component.html',
  styleUrls: ['./homework-detail.component.css']
})
export class HomeworkDetailComponent implements OnInit {
  pageTitle : string = 'HomeworkDetail';
  homework : Homework;

  constructor(private _currentRoute: ActivatedRoute,
    private _router : Router) {  }

  ngOnInit() {
    let id =+ this._currentRoute.snapshot.params['id'];
	  // definimos el string con interpolacion 
	  this.pageTitle +=  `: ${id}`;
  }
  
  onBack(): void {
    this._router.navigate(['/homeworks']);
  }
}