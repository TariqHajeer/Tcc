import { Component, OnInit, ViewChild } from '@angular/core';
import { ConstraintsService } from 'src/app/Service/constraints.service';
import { HonestyService } from 'src/app/Service/honesty.service';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { Honesty } from 'src/app/Model/honesty.model';
import { Constraints } from 'src/app/Model/constraints.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-constraints',
  templateUrl: './constraints.component.html',
  styleUrls: ['./constraints.component.css']
}) 
export class ConstraintsComponent extends AppComponent implements OnInit {
  displayedColumns: string[] ;
  dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: Constraints;
  tempConstrint: Constraints;
  errorMessage:string;
  constructor(public service:ConstraintsService,public honestyservic:HonestyService,
    public roleservic:RoleService,public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
    this.get();
    this.honestyservic.getEnabled();
    
  }

  GetSimilarForUpdate(constraint:Constraints){
    if(constraint.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(constraint, this.service.constraintsall);  
  }

  GetSimilarForAdd(constraint: Constraints) {
    if(constraint.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(constraint, this.service.constraintsall);
  }

  error():boolean {
    if (this.service.constraint.Name.trim() == ''||this.service.constraint.HonestyId==null) {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    // var similarconstraint= this.helperService.GetComplitlySimilarWithAnotherId(this.service.constraint,this.service.constraintsall);
    // if( similarconstraint!=null){
    //   this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
    //   return true;
    // }
    var findhonesty=this.service.constraintsall.find(c=>c.Name==this.service.constraint.Name&&c.HonestyId==this.service.constraint.HonestyId)
    if(findhonesty!=null||findhonesty!=undefined){
     this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }else
   { this.errorMessage="";
    return false;
  }
  }
  UpdateValidation(constraint:Constraints):boolean{
    if(constraint==null||constraint.Id==0){
      return false;
    }
    if(constraint.Name.trim()==""){
      return false;
    }
    if(constraint.Name==this.tempConstrint.Name&&constraint.IsEnabled==this.tempConstrint.IsEnabled){
      return false;
    }
    var similarconstraint = this.helperService.GetComplitlySimilarWithAnotherId(constraint,this.service.constraintsall);
    if(similarconstraint!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.constraint.Name = this.tempConstrint.Name;
    this.service.constraint.IsEnabled = this.tempConstrint.IsEnabled;
    this.service.constraint = new Constraints;
    this.service.constraint.IsEnabled=true
  } 

  
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  { 
    this.showSpinner();
    this.service.getConstraints().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.constraintsall= response as Constraints[]);
      this.CheckArrayIsNull(this.service.constraintsall,"قيود")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    this.displayedColumns= ['Name', 'IsEnabled','Honesty', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateConstraint() || !this.roleservic.CanDeleteConstraint()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.constraint=new Constraints;
    this.service.constraint.IsEnabled=true
        this.semelar=null
  }
  submit(form:NgForm){
   this.showSpinner();
      this.service.postConstraints().subscribe(res=>{
        this.hideSpinner();
       // this.service.constraintsall.push(res as Constraints);
       this.get();
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
        this.service.constraint=new Constraints;
        this.service.constraint.IsEnabled=true
        this.semelar=null
        this.CheckArrayIsNull(this.service.constraintsall,"قيود")

      },
      err=>{
       
         this.hideSpinner();
      })
  
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putConstraints().subscribe(res=>{

          this.hideSpinner();
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.service.constraint.IsEnabled=true
          this.semelar=null
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }
  fillData(item){
    this.service.constraint=item;
    this.tempConstrint= Object.assign({}, item);
    this.service.constraint.Name= this.tempConstrint.Name;
    this.service.constraint.IsEnabled = this.tempConstrint.IsEnabled;
   
  }

  delete(id){
    this.showSpinner();
    this.service.deleteConstraints(id).subscribe(res=>{

      this.hideSpinner();
    var constraint = this.service.constraintsall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(constraint);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.constraintsall,"قيود")

    },err=>{
      this.hideSpinner();
    });
  }
 

   
}
