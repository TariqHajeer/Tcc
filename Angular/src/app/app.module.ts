import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule } from '@angular/common/http';


import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { NgxSpinnerModule } from "ngx-spinner";

import { MainComponent } from './Component/main/main.component';
import { SystemComponent } from './Component/main/system/system.component';
import { ConstraintsComponent } from './Component/main/system/pages/constraints/constraints.component';
import { CountriesComponent } from './Component/main/system/pages/countries/countries.component';
import { DegreeComponent } from './Component/main/system/pages/degree/degree.component';
import { GroupComponent } from './Component/main/system/pages/group/group.component';
import { HonestyComponent } from './Component/main/system/pages/honesty/honesty.component';
import { MaincountryComponent } from './Component/main/system/pages/maincountry/maincountry.component';
import { SpecializationComponent } from './Component/main/system/pages/specialization/specialization.component';
import { UserComponent } from './Component/main/system/pages/user/user.component';
import { AttachmentComponent } from './Component/main/system/pages/attachment/attachment.component';
import { CollegesComponent } from './Component/main/system/pages/colleges/colleges.component';
import { LanguagesComponent } from './Component/main/system/pages/languages/languages.component';
import { NationalityComponent } from './Component/main/system/pages/nationality/nationality.component';
import { PrivilageComponent } from './Component/main/system/pages/privilage/privilage.component';
import { SocialStatesComponent } from './Component/main/system/pages/social-states/social-states.component';
import { TypeOfRegisterComponent } from './Component/main/system/pages/type-of-register/type-of-register.component';
import { LoginComponent } from './Component/main/Auth/login/login.component';
import { PhoneTypeComponent } from './Component/main/system/pages/phone-type/phone-type.component';
import { LoginService } from './Service/login.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './Component/main/Auth/auth.interceptor';
import { ForbiddenComponent } from './Help/forbidden/forbidden.component';
import { StudentComponent } from './Component/main/student/student.component';
import { ProfileComponent } from './Component/main/profile/profile.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';

import { ErrorPageComponent } from './Component/main/error-page/error-page.component';
import { YearComponent } from './Component/main/system/pages/year/year.component';
import { YearSystemComponent } from './Component/main/system/pages/year-system/year-system.component';
import { StudyPlanComponent } from './Component/main/study-plan/study-plan.component';
import { SubjectTypeComponent } from './Component/main/system/pages/subject-type/subject-type.component';


//import { ShowStudyPlanComponent } from './Component/main/study-plan/study-plan.component';
import { TransferStudentComponent } from './Component/main/transfer-student/transfer-student.component';
import { ShowStudentComponent } from './Component/main/show-student/show-student.component';
import { AddSubjectMarksComponent } from './Component/main/add-subject-marks/add-subject-marks.component';
import { TransferStudentDegreeComponent } from './Component/main/transfer-student-degree/transfer-student-degree.component';
import { HomeComponent } from './Component/main/home/home.component';
import { ExamSystemComponent } from './Component/main/system/pages/exam-system/exam-system.component';
//import { AddPracticalDegreeSubjectComponent } from './Component/main/add-practical-degree-subject/add-practical-degree-subject.component';
//import { AddTheoreticalDegreeSubjectComponent } from './Component/main/add-theoretical-degree-subject/add-theoretical-degree-subject.component';
import { AddParticalOrTheorticalDegreeComponent } from './Component/main/add-partical-or-theortical-degree/add-partical-or-theortical-degree.component';
import { ShowStudyplanComponent } from './Component/main/show-studyplan/show-studyplan.component';
import { CustomMinDirective, CustomMaxDirective } from './Help/validation';
import { StudentInformationComponent } from './Component/main/show-student/student-information/student-information.component';
import { InputRangeDirective } from './Custom/Directives/input-range.directive';
import { StudentsNeedHelpDegreeComponent } from './Component/main/students-need-help-degree/students-need-help-degree.component';
import { ClosesPersonesComponent } from './Component/main/show-student/student-information/closes-persones/closes-persones.component';
import { StudentPhoneComponent } from './Component/main/show-student/student-information/student-phone/student-phone.component';
import { BrothersComponent } from './Component/main/show-student/student-information/brothers/brothers.component';
import { StudentAttatchmentComponent } from './Component/main/show-student/student-information/student-attatchment/student-attatchment.component';
import { StudentSubjectComponent } from './Component/main/show-student/student-information/student-subject/student-subject.component';
import { StudentNeedHelpDegreeComponent } from './Component/main/show-student/student-information/student-need-help-degree/student-need-help-degree.component';
import { StudentDegreeComponent } from './Component/main/show-student/student-information/student-degree/student-degree.component';
import { RegrsterationComponent } from './Component/main/show-student/student-information/regrsteration/regrsteration.component';
import { InformationComponent } from './Component/main/show-student/student-information/information/information.component';
import { StudentSubjectCanResetComponent } from './Component/main/show-student/student-information/student-subject-can-reset/student-subject-can-reset.component';
import { PartnersComponent } from './Component/main/show-student/student-information/partners/partners.component';
import { ReperationAndSanctionsComponent } from './Component/main/show-student/student-information/reperation-and-sanctions/reperation-and-sanctions.component';
import { EquivalentSubjectComponent } from './Component/main/show-studyplan/equivalent-subject/equivalent-subject.component';
import { DependOnSubjectComponent } from './Component/main/show-studyplan/depend-on-subject/depend-on-subject.component';
import { StudentExamsComponent } from './Component/main/show-student/student-information/student-exams/student-exams.component';
import { ConfirmDialogComponent } from './Help/confirm-dialog/confirm-dialog.component';
import { BlockedYearComponent } from './Component/main/system/pages/blocked-year/blocked-year.component';
import { SubjectService } from './Service/subject.service';
import { ShowStudentnominatedComponent } from './Component/main/show-studentnominated/show-studentnominated.component';
//import { SetHelpDegreeToStudentComponent } from './Component/main/set-help-degree-to-student/set-help-degree-to-student.component';
// import { TransferStudentDegreeComponent } from './Component/main/transfer-student-degree/transfer-student-degree.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    SystemComponent,
    ConstraintsComponent,
    CountriesComponent,
    DegreeComponent,
    GroupComponent,
    HonestyComponent,
    MaincountryComponent,
    SpecializationComponent,
    UserComponent,
    AttachmentComponent,
    CollegesComponent,
    LanguagesComponent,
    NationalityComponent,
    PrivilageComponent,
    SocialStatesComponent,
    TypeOfRegisterComponent,
    LoginComponent,
    PhoneTypeComponent,
    ForbiddenComponent,
    StudentComponent,
    ProfileComponent,
    ErrorPageComponent,
    YearComponent,
    YearSystemComponent,
    StudyPlanComponent,
    SubjectTypeComponent,
    TransferStudentComponent,
    ShowStudentComponent,
    AddSubjectMarksComponent,
    TransferStudentDegreeComponent,
    HomeComponent,
    ExamSystemComponent,
    AddParticalOrTheorticalDegreeComponent,
    ShowStudyplanComponent,
    CustomMinDirective,
    CustomMaxDirective,
    StudentInformationComponent,
    InputRangeDirective,
    StudentsNeedHelpDegreeComponent, ClosesPersonesComponent, StudentPhoneComponent, BrothersComponent, StudentAttatchmentComponent, StudentSubjectComponent, StudentNeedHelpDegreeComponent, StudentDegreeComponent, RegrsterationComponent, InformationComponent, StudentSubjectCanResetComponent, PartnersComponent, ReperationAndSanctionsComponent
    , EquivalentSubjectComponent, DependOnSubjectComponent, StudentExamsComponent, ConfirmDialogComponent, BlockedYearComponent, ShowStudentnominatedComponent,

    // SetHelpDegreeToStudentComponent


  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(
      {

        closeButton: true,

      }),
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatToolbarModule,
    MatSidenavModule,
    MatMenuModule,
    MatStepperModule,
    MatTabsModule,
    MatDatepickerModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatExpansionModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatBottomSheetModule,
    NgxSpinnerModule,
    MatDialogModule,
    MatCardModule,
    MatBadgeModule


  ],
  providers: [LoginService,SubjectService , AppComponent,{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },],
  exports: [StudentInformationComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule { }
