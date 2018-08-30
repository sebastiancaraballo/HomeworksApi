using System;
using Homeworks.Domain;

public class HomeworkModel {
    public Guid Id {get; set;}
    public DateTime DueDate {get; set;}
    public string Description {get; set;}
    public HomeworkModel() {

    }
    
    public HomeworkModel(Homework homework) {
        Id = homework.Id;
        DueDate = homework.DueDate;
        Description = homework.Description;
    }

    public Homework ToEntity() {
        return new Homework {
            Id = this.Id,
            DueDate = this.DueDate,
            Description = this.Description
        };
    }
}