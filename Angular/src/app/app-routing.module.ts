import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './Component/main/Auth/login/login.component';
import { MainComponent } from './Component/main/main.component';
import { SystemComponent } from './Component/main/system/system.component';
import { CountriesComponent } from './Component/main/system/pages/countries/countries.component';
import { MaincountryComponent } from './Component/main/system/pages/maincountry/maincountry.component';
import { AttachmentComponent } from './Component/main/system/pages/attachment/attachment.component';
import { DegreeComponent } from './Component/main/system/pages/degree/degree.component';
import { HonestyComponent } from './Component/main/system/pages/honesty/honesty.component';
import { ConstraintsComponent } from './Component/main/system/pages/constraints/constraints.component';
import { SpecializationComponent } from './Component/main/system/pages/specialization/specialization.component';
import { AuthGuard } from './Component/main/Auth/auth.guard';
import { UserComponent } from './Component/main/system/pages/user/user.component';
import { GroupComponent } from './Component/main/system/pages/group/group.component';
import { PrivilageComponent } from './Component/main/system/pages/privilage/privilage.component';
import { PhoneTypeComponent } from './Component/main/system/pages/phone-type/phone-type.component';
import { LanguagesComponent } from './Component/main/system/pages/languages/languages.component';
import { CollegesComponent } from './Component/main/system/pages/colleges/colleges.component';
import { NationalityComponent } from './Component/main/system/pages/nationality/nationality.component';
import { SocialStatesComponent } from './Component/main/system/pages/social-states/social-states.component';
import { TypeOfRegisterComponent } from './Component/main/system/pages/type-of-register/type-of-register.component';
import { ForbiddenComponent } from './Help/forbidden/forbidden.component';
import { ProfileComponent } from './Component/main/profile/profile.component';
import { ErrorPageComponent } from './Component/main/error-page/error-page.component';
import { YearSystemComponent } from './Component/main/system/pages/year-system/year-system.component';
import { YearComponent } from './Component/main/system/pages/year/year.component';
import { StudyPlanComponent } from './Component/main/study-plan/study-plan.component';
import { StudentComponent } from './Component/main/student/student.component';
import { SubjectTypeComponent } from './Component/main/system/pages/subject-type/subject-type.component';
import { TransferStudentComponent } from './Component/main/transfer-student/transfer-student.component';
import { ShowStudentComponent } from './Component/main/show-student/show-student.component';
import { AddSubjectMarksComponent } from './Component/main/add-subject-marks/add-subject-marks.component';
import { TransferStudentDegreeComponent } from './Component/main/transfer-student-degree/transfer-student-degree.component';
import { HomeComponent } from './Component/main/home/home.component';
import { ExamSystemComponent } from './Component/main/system/pages/exam-system/exam-system.component';
import { AddParticalOrTheorticalDegreeComponent } from './Component/main/add-partical-or-theortical-degree/add-partical-or-theortical-degree.component';
import { ShowStudyplanComponent } from './Component/main/show-studyplan/show-studyplan.component';
import { StudentInformationComponent } from './Component/main/show-student/student-information/student-information.component';
import { StudentsNeedHelpDegreeComponent } from './Component/main/students-need-help-degree/students-need-help-degree.component';
import { BlockedYearComponent } from './Component/main/system/pages/blocked-year/blocked-year.component';
import { ShowStudentnominatedComponent } from './Component/main/show-studentnominated/show-studentnominated.component';


const routes: Routes = [
  { path: "", component: LoginComponent, pathMatch: "full" },
  {
    path: "home", component: MainComponent, children: [
      {
        path: "system", component: SystemComponent, data: { title: 'system' }, canActivate: [AuthGuard], children: [
          { path: "city", component: CountriesComponent, canActivate: [AuthGuard] },
          { path: "country", component: MaincountryComponent, canActivate: [AuthGuard] },
          { path: "attachment", component: AttachmentComponent, canActivate: [AuthGuard] },
          { path: "degree", component: DegreeComponent, canActivate: [AuthGuard] },
          { path: "phonetype", component: PhoneTypeComponent, canActivate: [AuthGuard] },
          { path: "language", component: LanguagesComponent, canActivate: [AuthGuard] },
          { path: "colleges", component: CollegesComponent, canActivate: [AuthGuard] },
          { path: "nathionalty", component: NationalityComponent, canActivate: [AuthGuard] },
          { path: "socialstate", component: SocialStatesComponent, canActivate: [AuthGuard] },
          { path: 'honesty', component: HonestyComponent, canActivate: [AuthGuard] },
          { path: 'constains', component: ConstraintsComponent, canActivate: [AuthGuard] },
          { path: 'specialzation', component: SpecializationComponent, canActivate: [AuthGuard] },
          { path: 'user', component: UserComponent, canActivate: [AuthGuard], data: { title: 'user' } },
          { path: 'group', component: GroupComponent, canActivate: [AuthGuard] },
          { path: 'privilage', component: PrivilageComponent, canActivate: [AuthGuard] },
          { path: 'typeofregister', component: TypeOfRegisterComponent, canActivate: [AuthGuard] },
          { path: 'yearsystem', component: YearSystemComponent, canActivate: [AuthGuard] },
          { path: 'year', component: YearComponent, canActivate: [AuthGuard] },
          { path: 'blockedyear/:Id', component: BlockedYearComponent, canActivate: [AuthGuard] },
          { path: 'subjectType', component: SubjectTypeComponent, canActivate: [AuthGuard] },
          { path: 'examSystem', component: ExamSystemComponent, canActivate: [AuthGuard] },

        ]
      }
      , { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
      { path: 'welcome', component: HomeComponent, canActivate: [AuthGuard] },
      { path: 'student', component: StudentComponent , canActivate: [AuthGuard]},
      { path: 'showStudent', component: ShowStudentComponent, canActivate: [AuthGuard],children:[
              ] },
             
        { path: 'studentinfo/:Ssn', component: StudentInformationComponent , canActivate: [AuthGuard]},
        { path: 'StudentsNeedHelpDegree', component: StudentsNeedHelpDegreeComponent , canActivate: [AuthGuard]},
      
      { path: 'TransferStudent', component: TransferStudentComponent, canActivate: [AuthGuard] },
       { path: 'TransferStudentDegree/:Ssn', component: TransferStudentDegreeComponent, canActivate: [AuthGuard] },
       { path: 'subjectnominated', component: ShowStudentnominatedComponent, canActivate: [AuthGuard] },
      { path: 'studyplan', component: StudyPlanComponent, canActivate: [AuthGuard] },
      { path: 'showstudyplan', component: ShowStudyplanComponent, canActivate: [AuthGuard] },
      { path: 'addsubjectmark', component: AddSubjectMarksComponent, canActivate: [AuthGuard] },
      { path: 'addPracticalorTheoreticalDegree/:subjectid/:yearid/:semesterid', component: AddParticalOrTheorticalDegreeComponent, canActivate: [AuthGuard] },
      { path: "err404", component: ErrorPageComponent, pathMatch: "full" }

    ]
  }, { path: "err403", component: ForbiddenComponent, pathMatch: "full" }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
