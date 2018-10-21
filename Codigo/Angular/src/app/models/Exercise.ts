
export class Exercise {
    id:string;
    problem:string;
    score:number;

    constructor(id:string = "", problem = "", score = 0) {
        this.id = id;
        this.problem = problem;
        this.score = score;
    }
}