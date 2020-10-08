import { Component, Input, OnInit ,ViewChild} from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule,MatFormFieldControl } from '@angular/material/form-field';
import { Student, StudentResponseDTO } from 'src/app/Model/student.model';
import { StudentService } from 'src/app/Service/student.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { HelpService } from 'src/app/Help/help.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { PagingDTO } from 'src/app/Help/paging/pagination.model';
@Component({
  selector: 'app-transfer-student',
  templateUrl: './transfer-student.component.html',
  styleUrls: ['./transfer-student.component.css']
})

export class TransferStudentComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Input() totalCount: number;
  pageEvent: PageEvent;
  paging: PagingDTO
  fullName: string
  constructor(
    public StudentService:StudentService,
    public spinner: NgxSpinnerService
    ,private router: Router
    ,public helpService:HelpService
    ,  public SpecializationService: SpecializationService
  ) { 
    super( spinner,helpService);
  }

  ngOnInit(): void {
    this.SpecializationService.getEnabled();
    this.get();
    this.paging = new PagingDTO

   // this.allFilter()
  }
showStudentDegree(Ssn){
    this.StudentService.StudentPreviousYearSetting(Ssn).subscribe(res=>{
      this.router.navigate(['/home/TransferStudentDegree/',Ssn]);
          },err=>{
            this.helpService.toastr.error('','لايوجد سنة للطالب');
          });
}
   
  // SubjectWithDegree:SubjectWithDegree[]=[];
  get()
  {
    this.StudentService.StudentNeedProcessing().subscribe(
      res=>{
        console.log(res)
        this.dataSource = new MatTableDataSource(this.StudentService.GetTransferStudent=res as StudentResponseDTO[]);
        this.dataSource.sort = this.sort;
        this.StudentService.GetTransferStudent.forEach(item=>{
          item.Specialization=this.SpecializationService.specialall.find(s=>s.Id==item.SpecializationId)
        }) 
        this.CheckArrayIsNull(this.StudentService.GetTransferStudent,"طلاب")
        this.dataSource.paginator = this.paginator;
      this.displayedColumns= [ 'FirstName','FatherName','LastName', 'specialaztion','More'];
      },
      err => {
        this.hideSpinner();
      }
    );
    
    
    
  }  
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue;
  }
  switchPage(event: PageEvent) {
   
    this.paging.allItemsLength=event.length
    this.paging.RowCount =  event.pageSize
    this.paging.Page = event.pageIndex+1
   
   
    
   }
   
/* AddSubjectWithDegree(Student:StudentResponseDTO){
   this.SubjectWithDegree.filter(s=>{
    Student.Subjects.filter(st=>{s.SubjectId=st.Id})
   });
this.StudentService.SetDegreeForTransformStudent(Student.Ssn,this.SubjectWithDegree).subscribe(
  res=>{
    
  }
);
  }
*/
}

