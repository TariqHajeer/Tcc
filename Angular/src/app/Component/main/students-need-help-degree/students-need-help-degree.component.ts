import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PagingDTO } from 'src/app/Help/paging/pagination.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { StudentService } from 'src/app/Service/student.service';
import { Router } from '@angular/router';
import { HelpService } from 'src/app/Help/help.service';
import { StudentResponce } from 'src/app/Model/student-responce.model';
import { MatTableDataSource } from '@angular/material/table';
import { AppComponent } from 'src/app/app.component';
import { SpecializationService } from 'src/app/Service/specialization.service';

@Component({
  selector: 'app-students-need-help-degree',
  templateUrl: './students-need-help-degree.component.html',
  styleUrls: ['./students-need-help-degree.component.css']
})
export class StudentsNeedHelpDegreeComponent extends AppComponent implements OnInit {

  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Input() totalCount: number;
  pageEvent: PageEvent;
  SpecializationId=""
  date =""
  ssn=""
  fatherName=""
  lastName=""
  firstName=""
  paging: PagingDTO
  fullName: string
  constructor(
    public spinner: NgxSpinnerService,
    public studentService: StudentService,
    public router: Router,
    public helpservic: HelpService,
    public SpecializationService: SpecializationService,

  ) { super(spinner, helpservic); }


  ngOnInit(): void {
    this.get();
    this.SpecializationService.getEnabled();
    this.paging = new PagingDTO
this.allFilter();
  }

  GetTransferStudent: StudentResponce[] = []
  get() {
    //this.studentService.getStudent();
    this.dataSource = new MatTableDataSource(this.GetTransferStudent);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.displayedColumns = ['SSN', 'FirstName','FatherName','LastName', 'specialaztion', 'regesterDate', 'CurrentYear','CurrentState','FinalState','More'];
  }
 
  
 allFilter(){
  this.studentService.StudentsNeedHelpDegree(this.ssn, this.SpecializationId, this.firstName, this.fatherName, this.lastName, this.date, this.paging).subscribe(response => {
    this.dataSource=new MatTableDataSource(response.body)
    this.totalCount = JSON.parse(response.headers.get('x-paging')).TotalRows;
   
  },
  err => {
    this.hideSpinner();
  });
 }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  showStudentMarks(ssn) {
    this.router.navigate(['/home/studentinfo/',ssn]);
   
    localStorage.setItem("helpdgree","helpdgree");
  }

  switchPage(event: PageEvent) {
   
   this.paging.allItemsLength=event.length
   this.paging.RowCount =  event.pageSize
   this.paging.Page = event.pageIndex+1
  
this.allFilter();
   
  }

}
