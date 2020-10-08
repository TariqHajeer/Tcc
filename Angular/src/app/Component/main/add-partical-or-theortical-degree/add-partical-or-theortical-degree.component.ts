import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { StudentSubjectService } from 'src/app/Service/student-subject.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HelpService } from 'src/app/Help/help.service';
import { StudentSubjectDTO } from 'src/app/Model/student.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NumberValidators } from 'src/app/Help/validation';
import { StudentSubjectDTOService } from 'src/app/Service/student-subject-dto.service'
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-add-partical-or-theortical-degree',
  templateUrl: './add-partical-or-theortical-degree.component.html',
  styleUrls: ['./add-partical-or-theortical-degree.component.css']
})
export class AddParticalOrTheorticalDegreeComponent extends AppComponent implements OnInit {


  constructor(public studentSubjectservice: StudentSubjectService
    , public helperSerive: HelpService,
    public spinner: NgxSpinnerService,
    public formbuilder: FormBuilder,
    private studentSubjectDTOService: StudentSubjectDTOService,
    public router: Router
    , public getroute: ActivatedRoute) { super(spinner, helperSerive); }
  studentSubjectDgree: StudentSubjectDTO
  getstudent: StudentSubjectDTO[] = []
  getNominateStudent: StudentSubjectDTO[] = []
  showStudentsNominate: boolean = true;
  //data table
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  //validation
  formDgree: FormGroup
  maxnum = 100
  minnum = 0
  isValidFormSubmitted = null;
  TheoricalDegreeNullable = true;

  semesterid
  yearid
  subjectid
  ngOnInit(): void {
    // this.count=0;
    this.getroute.params.subscribe(par => {
      this.subjectid = par['subjectid'] as string
      this.yearid = par['yearid'] as string
      this.semesterid = par['semesterid'] as string
    });
    this.formDgree = this.formbuilder.group({
      theoreticlaDegree: ['', [Validators.required, NumberValidators.isNumberCheck, Validators.max(this.maxnum), Validators.min(this.minnum)]],
      practicalDegree: ['', [Validators.required, NumberValidators.isNumberCheck, Validators.max(this.maxnum), Validators.min(this.minnum)]]
    })
    this.get();

  }
  get form() { return this.formDgree.get("theoreticlaDegree"); }
  get() {
    this.showSpinner();
    this.studentSubjectDgree = JSON.parse(localStorage.getItem('subjectinfo')) as StudentSubjectDTO;
    this.studentSubjectDgree.ExamSemesterId = this.semesterid
    this.studentSubjectDgree.year.Id = this.yearid
    this.studentSubjectDgree.Subject.Id = this.subjectid
    this.studentSubjectservice.getStudentSubject(this.subjectid, this.yearid, this.semesterid).subscribe(res => {
      this.getstudent = res as StudentSubjectDTO[]
      this.CheckArrayIsNull(this.getstudent, "طلاب")
      this.studentSubjectDgree = this.getstudent.find(s => s.Subject.Id == this.subjectid)
      this.dataSource = new MatTableDataSource(this.getstudent);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.displayedColumns = ['Ssn', 'StudentName', 'PracticalDegree', 'TheoreticlaDegree', 'Note'];
      this.hideSpinner();
      this.TheoricalDegreeNullable = true;
      this.ValidDegree();
    }, err => {
      this.hideSpinner();


    });


  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  AddSubjectDegrees() {
    if (this.getstudent.filter(c => c.PracticalDegree == null).length > 0) {
      this.helperService.toastr.error("يجب ملئ كامل علامات العملي", "erre");
      return;
    }
    // if(!this.TheoricalDegreeNullable){
    //   if(this.getstudent.filter(c=>c.TheoreticlaDegree==null).length>0){
    //     this.helperService.toastr.error("يجب ملئ كامل علامات النظري","erre");
    //     return;
    //   }
    // }
    this.showSpinner()
    this.studentSubjectservice.SetStudentsDegreeBySubject(this.getstudent).subscribe(res => {

      this.hideSpinner()
      //  location.reload();
      this.router.navigateByUrl("home/addsubjectmark")
    },
      err => {
        this.hideSpinner()
      })
  }
  // NumberRangeValidation(element,event,min,max){
  //   console.log(max);
  // this.studentSubjectDgree.Subject.SubjectType.PracticalDegree
  //   var value= Number( element.value);
  //   if( value<min||value>max){
  //     console.log('x');
  //     console.log(event);
  //     event.preventDefault();
  //   }
  // }
  // count=0

  showNominateStudent() {

    //  this.getstudent.forEach(item => {
    //   if (item.PracticalDegree < item.Subject.SubjectType.NominateDegree) {
    //     item.TheoreticlaDegree = null
    //     item.disabledTheoreticlaDegree = true
    //     var find=this.getstudent.find(s=>s==item)
    //     if(find!=null){
    //       this.getstudent.filter(s=>s!=item)
    //     }
    //     return
    //   }
    //   if (item.PracticalDegree >= item.Subject.SubjectType.NominateDegree) {
    //     var find=this.getstudent.find(s=>s==item)
    //     if(find==null){
    //       this.getstudent.push(item)
    //     }
    //     item.TheoreticlaDegree = null
    //     item.disabledTheoreticlaDegree = false
    //     }
    //  })
    this.dataSource = new MatTableDataSource(this.getstudent);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.displayedColumns = ['StudentName', 'PracticalDegree', 'TheoreticlaDegree', 'Note'];
  }
  ValidDegree() {
    this.getstudent.forEach(c => {
      if (c.Subject.SubjectType.NominateDegree== 0) {
        c.disabledPracticalDegree = true
        c.PracticalDegree = 0;
        c.disabledTheoreticlaDegree = false;
      }
      else if (c.PracticalDegree == null) {
        c.PracticalDegree = 0;
        c.disabledTheoreticlaDegree = true;
      }
      else {
        if (this.studentSubjectDTOService.IsNominate(c)) {
          c.disabledTheoreticlaDegree = false;
        }
      }
    });
  }
  CheckParticalDgree(element: StudentSubjectDTO) {
    console.log(element)
    if (this.studentSubjectDTOService.IsNominate(element)) {
      element.disabledTheoreticlaDegree = false;
    } else {
      element.disabledTheoreticlaDegree = true;
      if (!this.TheoricalDegreeNullable) {
        element.TheoreticlaDegree = 0;
      }
      else {
        element.TheoreticlaDegree = element.TheoreticlaDegree == null ? null : 0;
      }
    }
  }
  DisabledPracticalDegree(element: StudentSubjectDTO): boolean {
    if (element.ExamSemesterNumber != element.Subject.MainSemesterNumber)
      return true
    else
      return false
  }
  numberOnly(element, event, min, max): boolean {


    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode == 8)
      return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

}
