import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { HelpDgreeDto } from 'src/app/Model/help-dgree-dto';
import { ResponseStudentHelpDegreeDto } from 'src/app/Model/response-student-help-degree-dto';
import { StudentSubjectDTO } from 'src/app/Model/student.model';
import { MainComponent } from '../../../main.component';
import { StudentSubjectDTOService } from '../../../../../Service/student-subject-dto.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/Help/confirm-dialog/confirm-dialog.component';
@Component({
  selector: 'app-student-need-help-degree',
  templateUrl: './student-need-help-degree.component.html',
  styleUrls: ['./student-need-help-degree.component.css']
})
export class StudentNeedHelpDegreeComponent implements OnInit {

  constructor(public StudentInformation: StudentInformationComponent,
    public MainCombonent: MainComponent,
    public studentSubjectDTOService: StudentSubjectDTOService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.GetSubjectNeedHelpDegree()
  }
  SubjectsNeedHelpDegree: ResponseStudentHelpDegreeDto = new ResponseStudentHelpDegreeDto()
  GetSubjectNeedHelpDegree() {
    this.SubjectsNeedHelpDegree = new ResponseStudentHelpDegreeDto
    this.StudentInformation.studentSubjectService.SubjectNeedHelpDegree(this.StudentInformation.SSN).subscribe(res => {
      this.SubjectsNeedHelpDegree = res as ResponseStudentHelpDegreeDto
      if (this.SubjectsNeedHelpDegree == null) {
        this.SubjectsNeedHelpDegree = new ResponseStudentHelpDegreeDto
        this.hiddenSave = true
        this.StudentInformation.CheckArrayIsNull(this.SubjectsNeedHelpDegree.StudentSubjects, "مواد")
      }
      else{
        this.StudentInformation.CheckArrayIsNull(this.SubjectsNeedHelpDegree.StudentSubjects, "مواد")
        this.hiddenSave = false
      }
     
    }, err => {
      this.StudentInformation.CheckArrayIsNull(this.SubjectsNeedHelpDegree.StudentSubjects, "مواد")

      this.hiddenSave = true
    });

  }
  SetOrUnSetHelpDegree(item: StudentSubjectDTO) {
    item.HelpDegree = !item.HelpDegree;
    var subjectHaveHelpDegreeCount = this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree == true).length;
    var subjectDontHaveHelpDegree = this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree == false);
    if (subjectHaveHelpDegreeCount == this.SubjectsNeedHelpDegree.StillToSucess || this.SubjectsNeedHelpDegree.HelpDegreeDivideOn == subjectHaveHelpDegreeCount) {
      subjectDontHaveHelpDegree.forEach(c => c.disabledHelpDegree = true);
      return;
    }
    var sumOfGivenHelpDegree = this.SumOfGivenHelpDgree();

    if (sumOfGivenHelpDegree == this.SubjectsNeedHelpDegree.HelpDgreeCount) {
      subjectDontHaveHelpDegree.forEach(c => c.disabledHelpDegree = true);
      return;
    }
    var stillHelpDgree = this.SubjectsNeedHelpDegree.HelpDgreeCount - this.SumOfGivenHelpDgree();
    subjectDontHaveHelpDegree.forEach(element => {
      var needed = this.studentSubjectDTOService.SucessMark(element) - this.studentSubjectDTOService.TotalOrigenalDegree(element);
      if (needed > stillHelpDgree) {
        element.disabledHelpDegree = true;
      } else {
        element.disabledHelpDegree = false;
      }
    });
  }
  SumOfGivenHelpDgree(): number {
    var subjectHabeHelpDegree = this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree == true);
    var totalGivenHelpDegree = 0;
    subjectHabeHelpDegree.forEach(element => {
      totalGivenHelpDegree += this.studentSubjectDTOService.SucessMark(element) - this.studentSubjectDTOService.TotalOrigenalDegree(element)
    });
    return totalGivenHelpDegree;
  }
  IsStudentSucess(): boolean {
    var subjectHaveHelpDegree = this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree == true);
    // var subjectHaventHelpDegree = this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree != true);
    if (subjectHaveHelpDegree.length == this.SubjectsNeedHelpDegree.StillToSucess) {
      return true;
    }
    return false;
  }

  disabledSave(): boolean {

    if (this.SubjectsNeedHelpDegree.StudentSubjects != [] && this.SubjectsNeedHelpDegree.StudentSubjects.filter(c => c.HelpDegree == true).length == 0)
      return false;
    return !this.IsStudentSucess();
  }
  GetGivenHelpDegee(item: StudentSubjectDTO): String {
    if (!item.HelpDegree) {
      return '0';
    }
    var sucessDegree = this.studentSubjectDTOService.SucessMark(item);
    var total = this.studentSubjectDTOService.TotalOrigenalDegree(item);
    return "" + (sucessDegree - total);
  }
  Subjects: HelpDgreeDto[] = []
  Subject: HelpDgreeDto = new HelpDgreeDto
  hiddenSave = true
  SetHelpDegree() {
    this.Subjects = []
    this.Subject = new HelpDgreeDto
    this.SubjectsNeedHelpDegree.StudentSubjects.forEach(s => {
      if (s.HelpDegree == true) {
        this.Subject.Id = s.Id
        this.Subject.Note = s.Note
        this.Subjects.push(this.Subject)
        this.Subject = new HelpDgreeDto
      }

    })
    if (this.Subjects.length == 0) {

      const confirmDialog = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'خطأ',
          message: 'يجب اضافة علامات مساعدة '
        }
      });

    } else {
      const confirmDialog = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'تأكيد',
          message: 'هل انت متاكد من اضافة علامات مساعدة '
        }
      });
      confirmDialog.afterClosed().subscribe(result => {
        if (result == true) {
          this.StudentInformation.showSpinner()
          this.StudentInformation.studentSubjectService.SetHelpDgree(this.StudentInformation.SSN, this.Subjects).subscribe(
            res => {
              this.StudentInformation.hideSpinner()
              this.GetSubjectNeedHelpDegree()
              this.StudentInformation.ReloadPage()
            //  this.MainCombonent.StudentService.studentsNeedHelpDegreeCount--;
            }, err => {
              this.StudentInformation.hideSpinner()
            }
          )
        }
      });


    }

  }
}
