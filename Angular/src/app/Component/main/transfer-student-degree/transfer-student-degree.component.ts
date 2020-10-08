import { Component, OnInit, Type, Input, OnChanges, SimpleChange } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { StudentService } from 'src/app/Service/student.service';
import { StudentResponseDTO, StudentSubjectDTO, Student } from 'src/app/Model/student.model';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HelpService } from 'src/app/Help/help.service';
import { YearSystem } from 'src/app/Model/year-system.model';
import { SettingsEnum } from 'src/app/Model/settings-enum.enum';
import { StudentSubjectDTOService } from 'src/app/Service/student-subject-dto.service'
import { THIS_EXPR, ThrowStmt } from '@angular/compiler/src/output/output_ast';
import { environment } from 'src/environments/environment.prod';
import { element } from 'protractor';
import { takeUntil } from 'rxjs/operators';
import { StudentSubjectService } from 'src/app/Service/student-subject.service';
import { MainComponent } from '../main.component';
@Component({
  selector: 'app-transfer-student-degree',
  templateUrl: './transfer-student-degree.component.html',
  styleUrls: ['./transfer-student-degree.component.css']
})
export class TransferStudentDegreeComponent extends AppComponent implements OnInit {

  constructor(private router: Router, private getroute: ActivatedRoute,
    private studentSubjectDTOService: StudentSubjectDTOService
    , public StudentService: StudentService
    ,public studentSubjectService:StudentSubjectService
    , public spinner: NgxSpinnerService
    , public HelpService: HelpService,
    public MainComponent:MainComponent) {
    super(spinner,HelpService);
  }
  //#region 

  //#endregion
 
  Ssn: string;
  student: StudentResponseDTO = new StudentResponseDTO();
  count: number = 0;
  // disabledTheoreticlaDegree: boolean = true;
  AdministrativeHeighting: number = 0;
  helpDegree: number;
  helpDegreeCountSubject: number;
  temphelpDegree: number;
  temphelpDegreecountSubject: number;

  HelpDegreeMode: boolean = false;


  yearSystem: YearSystem = new YearSystem();
  TempHelpDgree: number;
  TempHelpDgreeCountSubject: number;

  ngOnInit(): void {
   
    this.getroute.params.subscribe(par => {
      this.Ssn = par['Ssn'] as string
    });
    this.getStudent();
    this.GetSettings();
  }
  AllSubjectHaveADegree(): boolean {
    return this.student.Subjects.every(c => c.TheoreticlaDegree != null);
  }
  getStudent() {
    this.showSpinner();
    this.StudentService.StudentNeedProcessingbySsn(this.Ssn).subscribe(res => {
      this.hideSpinner();
      this.student = res;
      this.CheckArrayIsNull( this.student.Subjects,"مواد")
      this.student.Subjects.forEach(item => {
        item.disabledTheoreticlaDegree = true;
        if (item.Subject.SubjectType.NominateDegree == 0) {
          item.disabledTheoreticlaDegree = false;
        }
        item.PracticalDegree = item.Subject.SubjectType.NominateDegree;
        item.TheoreticlaDegree = 0;
        item.disabledHelpDegree = true;
         item.PracticalDegree = 0;
         item.HelperDegree = 0;
      }
      );
    }, err => {
      this.hideSpinner();
     
    });
  }
  GetSettings() {
    this.StudentService.StudentPreviousYearSetting(this.Ssn).subscribe(res => {
      this.yearSystem = res as YearSystem
      
     // console.log(this.yearSystem.Settings.filter(c => c.Id == 1));
     // console.log(this.yearSystem.Settings.filter(c => c.Id == SettingsEnum.NumberOfSubjectOfAdministrativeLift));
      this.yearSystem.Settings.forEach(item => {
        if (item.Id == SettingsEnum.NumberOfSubjectOfAdministrativeLift) this.AdministrativeHeighting = item.Count;
        if (item.Id == SettingsEnum.TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful) { this.helpDegree = item.Count; }
        if (item.Id == SettingsEnum.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful) { this.helpDegreeCountSubject = item.Count; }
      });
    }, err => {
     
    });
  }
  checkPracticalDegree(item: StudentSubjectDTO): boolean {
    if (!this.studentSubjectDTOService.IsNominate(item)) {
      item.disabledTheoreticlaDegree = true;
      item.TheoreticlaDegree = null;
      item.disabledHelpDegree = true;
      item.HelperDegree = 0;
      item.TheoreticlaDegree = 0;
      item.message = "الطالب/ة غير مترشح/ة للمادة";
      return false;
    }
    item.message = "0"
    item.disabledTheoreticlaDegree = false;
    return true;
  }
  CheckTheoreticlaDegree(item: StudentSubjectDTO): boolean {
    if (this.studentSubjectDTOService.IsSucess(item)) {
      item.disabledHelpDegree = true;
      item.HelperDegree = 0;
      return true;
    }
    item.disabledHelpDegree = false;
    return false;
  }
  CheckSubjectDegree(item: StudentSubjectDTO) {
    if (this.checkPracticalDegree(item)) {
      this.CheckTheoreticlaDegree(item);
    }
  }
  BalcneStudentSubject(item: StudentSubjectDTO) {
    this.checkPracticalDegree(item);
    this.CheckTheoreticlaDegree(item);
  }
  CountSucessSubject(): number {
    return this.student.Subjects.filter(c => this.studentSubjectDTOService.IsSucess(c)).length;
  }
  studentStateMessage: string
  disabledHelpDgree: boolean = true;
  helpDegreeMessage: string



  CanSubjectHaveHelpDegree(item: StudentSubjectDTO, testingSubject: StudentSubjectDTO): boolean {
    if (this.studentSubjectDTOService.IsSucess(item))
      return false;
    // debugger;
    var countOfSubjectGivenHelpDegree = this.AvilableSubjectCountCanGetHelpDegree();
    if (item != testingSubject) {
      if (countOfSubjectGivenHelpDegree == 0)
        return false;
    } else {
      if (item.HelperDegree == 0) {
        if (countOfSubjectGivenHelpDegree == 0) {
          return false;
        }
      }
    }
    var avilableHelpDegree = this.AvilableHelpDegree();
    var stillToSucess = this.studentSubjectDTOService.SucessMark(item) - this.studentSubjectDTOService.TotalOrigenalDegree(item);
    return stillToSucess <= avilableHelpDegree;
  }
  AvilableSubjectCountCanGetHelpDegree(): number {
    var countOfSubjectGivenHelpDegree = 0;
    this.student.Subjects.forEach(subject => {
      if (Number(subject.HelperDegree) != 0)
        countOfSubjectGivenHelpDegree++;
    });
    return this.helpDegreeCountSubject - countOfSubjectGivenHelpDegree;
  }
  AvilableHelpDegree(): number {
    var sumOfGivenHelpDegree = 0;
    this.student.Subjects.forEach(subject => {
      if (Number(subject.HelperDegree) != 0)
        sumOfGivenHelpDegree += subject.HelperDegree;
    });
    return this.helpDegree - sumOfGivenHelpDegree;
  }
  DisabledTheroricalDegree(item: StudentSubjectDTO): boolean {
    return item.disabledTheoreticlaDegree;
  }
  GiveOrUnGiveHelpDegree(item: StudentSubjectDTO) {

    if (item.HelperDegree == 0) {
      var total = this.studentSubjectDTOService.TotalOrigenalDegree(item);
      var sucessDegree = this.studentSubjectDTOService.SucessMark(item);
      item.HelperDegree = sucessDegree - total;
    }
    else {
      item.HelperDegree = 0;
    }
    this.BalcneStudentSubject(item);
  }
  onCheckboxChange() {
    this.student.Subjects.forEach(element => {
      element.HelperDegree = 0;
    });
    // if (!this.HelpDegreeMode) {
     this.student.Subjects.forEach(subject => subject.HelperDegree = 0);
    // }
  }
  // !CanSetHelpDegree()||
  IsStudentSucessWithHelpDegree(): boolean {
    var successCount = this.SucessCountWithHelpDegree();
    if (successCount >= environment.SucessSubjectCount) {
      return true;
    }
    if (this.AdministrativeHeighting >= environment.NumberOfSubjectOfAdministrativeLiftIfNotExist) {
      if (successCount >= this.AdministrativeHeighting) {
        return true;
      }
    }
    return false;
  }
  IsStudentSucessWithoutHelpDegee(): boolean {
    var result = false;
    if (this.SuccessCount() >= environment.SucessSubjectCount) {
      result = true;
    }
    if (this.AdministrativeHeighting >=environment.NumberOfSubjectOfAdministrativeLiftIfNotExist) {
      if (this.SuccessCount() >= this.AdministrativeHeighting) {
        result = true;
      }
    }
    if (result) {
      this.student.Subjects.forEach(element => {
        element.HelperDegree = 0;
      });
    }
    return result;
  }
  SucessSubject(): StudentSubjectDTO[] {
    return this.student.Subjects.filter(subject => {
      if (this.studentSubjectDTOService.IsSucess(subject))
        return true;
      return false;
    });
  }
  NonNominateSubject(): StudentSubjectDTO[] {
    return this.student.Subjects.filter(subject => {
      if (!this.studentSubjectDTOService.IsNominate(subject))
        return true;
      return false;
    });
  }

  SubjecCanGetHelpDegree(): StudentSubjectDTO[] {
    var subjectCannotHaveHelpDegree = this.SucessSubject().concat(this.NonNominateSubject());
    return this.student.Subjects.filter(subject => {
      if (!subjectCannotHaveHelpDegree.includes(subject))
        return true;
      return false;
    });
  }
  SubjecCanGetHelpDegreeAndDontHave(): StudentSubjectDTO[] {
    return this.SubjecCanGetHelpDegree().filter(c => {
      return c.HelperDegree == 0;
    });
  }
  SubjectHaveHelpDegree(): StudentSubjectDTO[] {
    return this.SubjecCanGetHelpDegree().filter(c => {
      return c.HelperDegree != 0;
    });
  }

  CanSetHelpDegree(): boolean {
    this.SucessSubject().concat(this.NonNominateSubject()).forEach(
      subject => {
        subject.HelperDegree = 0;
        subject.disabledHelpDegree = true;
      }
    );
    var subjectHaveHelpDegree = this.SubjectHaveHelpDegree();
    var GivenHelpDgree = subjectHaveHelpDegree.map(c => c.HelperDegree).reduce((a, b) => a + b, 0);
    var avilableHelpDegree = this.helpDegree - GivenHelpDgree;
    var subjectCanGetHelpDegree = this.SubjecCanGetHelpDegreeAndDontHave();

    var avilableCount = this.helpDegreeCountSubject - subjectHaveHelpDegree.length;
    if (this.IsStudentSucessWithHelpDegree()) {
      subjectCanGetHelpDegree.forEach(subject => {
        subject.disabledHelpDegree = true;
      })
    } else {
      subjectCanGetHelpDegree = subjectCanGetHelpDegree.filter(subject => {
        if (avilableCount == 0 || avilableHelpDegree == 0) {
          subject.disabledHelpDegree = true;
          return false;
        }
        else {
          if (!(this.studentSubjectDTOService.TotalOrigenalDegree(subject) + avilableHelpDegree >= this.studentSubjectDTOService.SucessMark(subject))) {
            subject.disabledHelpDegree = true;
            return false;
          } else {
            subject.disabledHelpDegree = false;
            return true;
          }
        }
      });
      if (subjectHaveHelpDegree.length == 0 && subjectCanGetHelpDegree.length == 0) {
        return false;
      }
    }
    return true;
  }

  // SubjectsCanHaveHelpDegree() {
  //   var subjectCopy = this.student.Subjects.map(x=>Object.assign({},x));
  //   var faildSubject = subjectCopy.filter(element=>{
  //     if(!this.studentSubjectDTOService.IsSucess(element)&&this.studentSubjectDTOService.IsNominate(element)
  //     &&
  //     this.studentSubjectDTOService.TotalOrigenalDegree(element) + this.helpDegree >= this.studentSubjectDTOService.SucessMark(element)){
  //       return true;
  //     }
  //   });
  //   return null;
  // }

  /**
   * count of  sucess subject without help Degree
   */
  SuccessCount(): number {
    var count = 0;
    this.student.Subjects.forEach(
      element => {
        if (this.studentSubjectDTOService.IsSucess(element))
          count++;
      }
    )
    return count;
  }
  SucessCountWithHelpDegree(): number {
    var count = 0;
    this.student.Subjects.forEach(
      subject => {
        if (this.studentSubjectDTOService.IsSucessWithHelpDegree(subject)) {
          count++;
        }
      }
    );
    return count;
  }

  CheckStudentSubject(): boolean {
    if (this.IsStudentSucessWithoutHelpDegee()) {
      return true;
    }
    var subjectHaveHelpDgreeCount = this.SubjectHaveHelpDegree().length;
    if (subjectHaveHelpDgreeCount == 0)
      return true;
    return this.IsStudentSucessWithHelpDegree();
  }

  save() {
    // console.log(JSON.stringify(this.student.Subjects));
    this.showSpinner()
    this.student.Subjects.forEach(item=>{
      if(item.HelperDegree!=0)
      item.HelpDegree=true
    })
    console.log(JSON.stringify(this.student.Subjects))
    this.studentSubjectService.SetDegreeForTransformStudent(this.student.Subjects).subscribe(res => {
      this.hideSpinner()
      this.HelpService.add()
      this.MainComponent.StudentService.StudentNeedProcessingCount();
      this.router.navigateByUrl('/home/TransferStudent');
    },
      err => {
       console.log(err)
        this.hideSpinner()
      });
  }
  
}
