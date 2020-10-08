import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExampSystemService } from 'src/app/Service/examp-system.service';
import { ExampSystem } from 'src/app/Model/examp-system.model';

@Component({
  selector: 'app-exam-system',
  templateUrl: './exam-system.component.html',
  styleUrls: ['./exam-system.component.css']
})
export class ExamSystemComponent extends AppComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  displayedColumns: string[];
  dataSource = new MatTableDataSource<ExampSystem>(this.service.examSystems);
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  semelar: ExampSystem;
   tempExamSystem: ExampSystem;
  errorMessage:string;
  constructor(public service: ExampSystemService, public helperService: HelpService,
     public roleservic: RoleService,public spinner: NgxSpinnerService) {
      super( spinner,helperService);
    this.service.examsystem = new ExampSystem();
   }


  ngOnInit(): void {
    this.service.examsystem = new ExampSystem;
    this.get();

  }
  GetSimilarForUpdate( examSystem:ExampSystem){
    if(examSystem.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(examSystem, this.service.examSystems);  
  }

  GetSimilarForAdd(ExampSystem: ExampSystem) {
    if(ExampSystem.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(ExampSystem, this.service.examSystems);
  }
  error():boolean {
    if (this.service.examsystem.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarExamSystem= this.helperService.GetComplitlySimilarWithAnotherId(this.service.examsystem,this.service.examSystems);
     if( similarExamSystem!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation( examSystem:ExampSystem):boolean{
    if(examSystem==null||examSystem.Id==0){
      return false;
    }
    if(examSystem.Name.trim()==""){
      return false;
    }
    if(examSystem.Name==this.tempExamSystem.Name&&examSystem.IsEnabled==this.tempExamSystem.IsEnabled){
      return false;
    }
    var similarExamSystem = this.helperService.GetComplitlySimilarWithAnotherId(examSystem,this.service.examSystems);
 
    if(similarExamSystem!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.examsystem.Name = this. tempExamSystem.Name;
    this.service.examsystem.IsEnabled = this. tempExamSystem.IsEnabled;
    this.service.examsystem = new ExampSystem;
    this.service.examsystem.IsEnabled=true;
  }
  get() {
   this.showSpinner();
    this.displayedColumns = ['Name', 'IsEnabled', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    this.service.GetExamSystem().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.examSystems = response as ExampSystem[]);
      this.CheckArrayIsNull(this.service.examSystems,"نظام امتحان")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },err=>{
      this.hideSpinner();
     
    });
    if (!this.roleservic.CanUpdateDegree() || !this.roleservic.CanDeleteDegree()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.examsystem = new ExampSystem;
    this.service.examsystem.IsEnabled=true
    this.semelar=null
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  submit(form:NgForm) {
this.showSpinner();
    this.service.AddExamSystem().subscribe(res => {
      this.hideSpinner();
      this.service.examSystems.push(res as ExampSystem);
      this.dataSource._updateChangeSubscription();
      this.helperService.add();   this.resetForm(form);
      this.service.examsystem = new ExampSystem();
      this.service.examsystem.IsEnabled=true
      this.semelar=null
      this.CheckArrayIsNull(this.service.examSystems,"نظام امتحان")
    },
      err => {
         this.hideSpinner();
      })
 


  }
  edit(form:NgForm) {
    this.errorMessage=" ";
    this.showSpinner();
    this.service.UpdateExamSystem().subscribe(res => {
      this.hideSpinner();    
      this.get();
      this.service.examsystem = new ExampSystem();
       this.helperService.edit();
      this.resetForm(form);
      this. tempExamSystem = null;
    },
      err => {
         this.hideSpinner();
        this.closeModal();
        
      })

  }

  

  fillData(item) {
    this.errorMessage="";
    this.service.examsystem = item;
    this. tempExamSystem = Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.DeleteExamSystem(id).subscribe(res=>{
      this.hideSpinner();
    var ex = this.service.examSystems.filter(c=>c.Id==id)[0];    
      var index= this.dataSource.data.indexOf(ex);
      this.dataSource.data.splice(index,1);
      this.dataSource._updateChangeSubscription();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.examSystems,"نظام امتحان")
    },
      err => {
        this.hideSpinner();
      });
  }
  UpdateName():boolean{
    if(this.service.examsystem.Name=='')
    return true
    else
    return false
  }
}
