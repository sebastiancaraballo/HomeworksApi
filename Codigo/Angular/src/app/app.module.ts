import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeworksListComponent } from './components/homeworks-list/homeworks-list.component';
import { HomeworksFilterPipe } from './components/homeworks-list/homeworks-filter.pipe';
import { HomeworksService } from './services/homeworks.service';
import { StarComponent } from './components/star/star.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { HttpModule } from '@angular/http';
import { HomeworkDetailComponent } from './components/homework-detail/homework-detail.component';
import { HomeworkDetailGuard } from './shared/homework-detail.guard';

@NgModule({
  declarations: [
    AppComponent,
    HomeworksListComponent,
    HomeworksFilterPipe,
    StarComponent,
    WelcomeComponent,
    HomeworkDetailComponent
  ],
  imports: [
    HttpModule,
    FormsModule,
    BrowserModule,
    RouterModule.forRoot([
        { path: 'homeworks', component: HomeworksListComponent },
        { path: 'homeworks/:id', 
          component: HomeworkDetailComponent,
          canActivate: [HomeworkDetailGuard]
        },
        { path: 'welcome', component:  WelcomeComponent }, 
        { path: '', redirectTo: 'welcome', pathMatch: 'full' },
        { path: '**', redirectTo: 'welcome', pathMatch: 'full'}
    ])
  ],
  providers: [
    HomeworksService,
    HomeworkDetailGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
