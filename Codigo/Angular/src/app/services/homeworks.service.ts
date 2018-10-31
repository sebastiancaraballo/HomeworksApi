import { Injectable } from '@angular/core';
import { Homework } from '../models/Homework';
import { Exercise } from '../models/Exercise';

@Injectable()
export class HomeworksService {

  constructor() { }

  getHomeworks():Array<Homework> {
    return [
      new Homework('1', 'Una tarea', 0, 
      new Date(), 2, [
        new Exercise('1', 'Un problema', 1),
        new Exercise('2', 'otro problema', 10)
      ]),
      new Homework('2', 'Otra tarea', 0, 
        new Date(), 4, [])
    ];
  }
}

