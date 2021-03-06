import {IExercise} from './IExercise'
import {IHomework} from './IHomework';

export class Homework implements IHomework {
    id:string;
    description:string;
    score:number;
    dueDate:Date;
    exercises:Array<IExercise>;
    rating:number;

    constructor(id:string, description:string, score:number, dueDate:Date, rating:number, exercises:Array<IExercise>) {
        this.id = id;
        this.description = description;
        this.score = score;
        this.dueDate = dueDate;
        this.exercises = exercises;
        this.rating = rating;
    }
}