import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ReportComponent } from './report/report.component';
import { USMAPReportComponent } from './usmap/usmapreport.component';
import { ActualVsRetweetsComponent } from './actualVsRetweets/actualVsRetweets.component';
import { SwingStateAnalysisComponent } from './swingstateanalysis/swingstateanalysis.component';
import { DayWiseSentimentComponent } from './daywisesentiment/daywisesentiment.component';
import { AllstatesAnalysisComponent } from './allstatesanalysis/allstatesanalysis.component';
import { PredictionComponent } from './prediction/prediction.component';


const HomeRoutes: Routes = [

  { path: '', component: HomeComponent, data: { menu: 'home', breadcrumb: '<a>Dashboard</a>' } },
  { path: 'partywisereport', component: ReportComponent, data: { menu: 'partywisereport', breadcrumb: '<a>All States Analysis</a>' } },
  { path: 'usmapreport', component: USMAPReportComponent, data: { menu: 'usmapreport', breadcrumb: '<a>Day Wise Analysis</a>' } },
  { path: 'actualVsRetweets', component: ActualVsRetweetsComponent, data: { menu: 'actualVsRetweets', breadcrumb: '<a>Actual Vs Retweets</a>' } },
  { path: 'swingstatesnalysis', component: SwingStateAnalysisComponent, data: { menu: 'swingstatesnalysis', breadcrumb: '<a>Swing State Analysis</a>' } },
  { path: 'daywisesentiment', component: DayWiseSentimentComponent, data: { menu: 'daywisesentiment', breadcrumb: '<a>Day Wise Sentiment</a>' } },
  { path: 'allstatesanalysis', component: AllstatesAnalysisComponent, data: { menu: 'allstatesanalysis', breadcrumb: '<a>All States Analysis</a>' } },
  { path: 'prediction', component: PredictionComponent, data: { menu: 'prediction', breadcrumb: '<a>Analysis</a>' } },


];

@NgModule({
  imports: [RouterModule.forChild(HomeRoutes)],

})
export class HomeRoutingModule { }

