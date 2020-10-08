import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { StudentSubjectDTO } from 'src/app/Model/student.model';
import { UpdateStudentSubjectDto } from 'src/app/Model/update-student-subject-dto.model';
import { AuthService } from 'src/app/Service/auth.service';
import { StudentSubjectDTOService } from 'src/app/Service/student-subject-dto.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HelpService } from 'src/app/Help/help.service';
import { YearSystem } from 'src/app/Model/year-system.model';
import { YearService } from 'src/app/Service/year.service';
import { ConfirmDialogComponent } from 'src/app/Help/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MainComponent } from '../../../main.component';
@Component({
  selector: 'app-student-subject',
  templateUrl: './student-subject.component.html',
  styleUrls: ['./student-subject.component.css']
})
export class StudentSubjectComponent  implements OnInit {

  constructor(public StudentInformation: StudentInformationComponent
    , public AuthService: AuthService,
    public studentSubjectDTOService: StudentSubjectDTOService,
    public yearService: YearService,
    private dialog: MatDialog,
   ) {
    
  }

  ngOnInit(): void {
    this.GetSubject()
  }
  yearSystem: YearSystem = new YearSystem();
  Subjects: StudentSubjectDTO[] = []
  tempStudentSubject
  years: YearSemesterCollection[] = [];
  StudentSubject: UpdateStudentSubjectDto = new UpdateStudentSubjectDto()
  step: number;

  //#region  for html
  RowClass(item: StudentSubjectDTO): string {
    if (item.PracticalDegree == null || item.TheoreticlaDegree == null)
      return "";
    if (this.studentSubjectDTOService.IsSucess(item)) {
      return "SuccessClass";
    }
    return "ErrorClass";
  }
  // IsSucess(item: StudentSubjectDTO): boolean {
  //   return this.studentSubjectDTOService.IsSucess(item);
  // }
  Total(item: StudentSubjectDTO) {
    if (item.PracticalDegree == null)
      return;
    return this.studentSubjectDTOService.TotalOrigenalDegree(item);
  }

  //#endregion


  panelOpenState(x: number) {
    this.step = x;
  }

  CounterSubjects(Semester: SemesterSubjectCollection) {
    Semester.CountSubjectSuccess = Semester.Subject.filter(c => this.studentSubjectDTOService.IsSucess(c)).length;
    Semester.CountSubjectFailure = Semester.Subject.filter(c => !this.studentSubjectDTOService.IsSucess(c)&&(c.PracticalDegree!=null&&c.TheoreticlaDegree!=null)).length;
    Semester.CountSubject = Semester.Subject.length;
    Semester.CountSubjectIsNull=Semester.Subject.filter(s=>this.studentSubjectDTOService.SubjectIsNull(s)).length; 
  }
  FullYear
  GetSubject() {
    this.Subjects=[]
    this.years=[]
    this.StudentInformation.studentSubjectService.GetSubjectsBySSN(this.StudentInformation.SSN).subscribe(
      res => {
        this.Subjects = res as StudentSubjectDTO[]
        console.log(res)
        this.Subjects.forEach(item => {
          
          if (item.HelperDegree == null || item.HelperDegree == undefined)
            item.HelperDegree = 0
          item.SuccessSubject = this.studentSubjectDTOService.IsSucess(item);
        });
        var yearGroup = this.Subjects.reduce(function (r, a) {
          r[a.FirstYear] = r[a.FirstYear] || [];
          r[a.FirstYear].push(a);
          return r;
        }, Object.create(null));

        var yearPropertyNames = Object.getOwnPropertyNames(yearGroup);
        yearPropertyNames.forEach(element => {
          var yearcollection = new YearSemesterCollection();
          yearcollection.FirstYear = Number(element);
          var subjectsByYear = yearGroup[element] as StudentSubjectDTO[];
          var semeserGroup = subjectsByYear.reduce((r, a) => {
            r[a.ExamSemesterNumber] = r[a.ExamSemesterNumber] || [];
            r[a.ExamSemesterNumber].push(a);
            return r;
          }, Object.create(null));


          var semesterPropertyNames = Object.getOwnPropertyNames(semeserGroup);
          semesterPropertyNames.forEach(sp => {
            var semesterCollection = new SemesterSubjectCollection();
            semesterCollection.SemesterNumber = Number(sp);
            semesterCollection.Subject = semeserGroup[sp];
            yearcollection.SemesterSubjectCollection.push(semesterCollection);
          });
          this.years.push(yearcollection);
        });
        this.years.forEach(item => {
          if (item.FirstYear == 0)
          item.FullYear = "مواد الطالب من الجامعة التي نقل منها"
          else {
            item.SecoundYear = Number(item.FirstYear) + 1
            item.FullYear = item.FirstYear + "/" + item.SecoundYear
          }
        })
        this.StudentInformation.CheckArrayIsNull(this.years, "مواد")

      },err=>{
        this.StudentInformation.hideSpinner()
      }

    )
  }
  DisabledTheoreticlaDegree(item: StudentSubjectDTO) {
    if(item.Subject.SubjectType.NominateDegree==0){
      item.PracticalDegree=0
      item.disabledPracticalDegree=true
      item.disabledTheoreticlaDegree=false
    }
   else if(item.PracticalDegree==null){
      item.disabledTheoreticlaDegree = true
      item.TheoreticlaDegree = null
      return
    }
    var subject = this.studentSubjectDTOService.IsNominate(item)
    if (subject)
      item.disabledTheoreticlaDegree = false
    else {
      item.disabledTheoreticlaDegree = true
      item.TheoreticlaDegree = 0
    }
  }

  ClickEdit(item: StudentSubjectDTO) {
    item.showEdit = true;
    this.tempStudentSubject = Object.assign({}, item);
    item.HelpDegree=false
    this.StudentInformation.helperService.toastr.warning(' عند التعديل قد تفقد المواد علامات المساعدة الخاصة بهاو قد تضطر لإعادة وضع العلامات وقد تضطر الى فقدان الدورة الثالثة')

    // this.spinner.show();
    // this.yearService.GetSettingsByFirstYear(item.FirstYear).subscribe(res=>{
    //   this.spinner.hide();
    //   this.yearSystem = res as YearSystem;
    // },err=>{
    //   console.log(err);
    //   this.hideSpinner();
    // });
    // const confirmDialog = this.dialog.open(ConfirmDialogComponent, {
    //   data: {
    //     title: 'ملاحظة',
    //     message: 'عند التعديل يجب اعادة وضع علامات المساعدة في واجهة "المواد التي تحتاج علامات مساعدة" '
    //   }
    // });
    // confirmDialog.afterClosed().subscribe(result => {
    //   if (result == true) {

    //   }
    //   else {
    //     this.CancelEditStudentSubject(item)
    //   }
    // });
  

  }

  EditStudentSubject(item: StudentSubjectDTO, semester) {
    this.StudentSubject.Id = item.Id
    this.StudentSubject.HelpDegree = item.HelpDegree
    this.StudentSubject.Note = item.Note
    this.StudentSubject.PracticalDegree = item.PracticalDegree
    this.StudentSubject.TheoreticlaDegree = item.TheoreticlaDegree
    if(this.StudentSubject.TheoreticlaDegree=="")
    this.StudentSubject.TheoreticlaDegree=0
    if( this.StudentSubject.PracticalDegree=="")
    this.StudentSubject.PracticalDegree=0
    console.log( this.StudentSubject.PracticalDegree)
    this.StudentInformation.showSpinner()
    this.StudentInformation.studentSubjectService.UpdateStudentSubject(this.StudentSubject).subscribe(res => {
      this.StudentInformation.hideSpinner()
     this.StudentInformation.ReloadPage()
      item.showEdit = false
      this.CounterSubjects(semester);
      item.Modified = new Date();
      item.ModifiedBy = this.AuthService.auth.Username
        this.GetSubject();
    }, err => {
      this.StudentInformation.hideSpinner()
      this.CancelEditStudentSubject(item)
    })


  }
  CancelEditStudentSubject(item) {
    item = Object.assign(item, this.tempStudentSubject)
    item.showEdit = false
  }
  // GetYearSystem(firstYear:number){
  //   this.yearService.GetSettingsByFirstYear(firstYear).subscribe(res=>{

  //   })
  // }
  DisabledPracticalDegree(element: StudentSubjectDTO): boolean {
    if (element.ExamSemesterNumber != element.Subject.MainSemesterNumber)
      return true
    else
      return false
  }
}
class YearSemesterCollection {
  FirstYear: Number;
  SecoundYear: Number;
  FullYear
  SemesterSubjectCollection: SemesterSubjectCollection[] = [];
}
class SemesterSubjectCollection {
  SemesterNumber: Number;
  CountSubjectIsNull = 0
  CountSubjectFailure = 0
  CountSubjectSuccess = 0
  CountSubject = 0
  Subject: StudentSubjectDTO[] = [];
}