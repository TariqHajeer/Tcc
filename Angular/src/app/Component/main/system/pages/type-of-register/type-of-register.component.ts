import { Component, OnInit ,ViewChild} from '@angular/core';

import { NgForm } from '@angular/forms';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { TypeOfRegisterService } from 'src/app/Service/type-of-register.service';
import { TypeOfRegister } from 'src/app/Model/type-of-register.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-type-of-register',
  templateUrl: './type-of-register.component.html',
  styleUrls: ['./type-of-register.component.css']
})
export class TypeOfRegisterComponent extends AppComponent implements OnInit {


  displayedColumns: string[] ;
  dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: TypeOfRegister;
  temptypeofregister: TypeOfRegister;
  errorMessage:string;
  constructor(public service:TypeOfRegisterService,public roleservic:RoleService,
    public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
  
  }
  
  GetSimilarForUpdate(typeofregister:TypeOfRegister){
    if(typeofregister.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(typeofregister, this.service.typeofregisterall);  
  }

  GetSimilarForAdd(typeofregister: TypeOfRegister) {
    if(typeofregister.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(typeofregister, this.service.typeofregisterall);
  }

  error():boolean {
    if (this.service.typeofregister.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similartypeofregester= this.helperService.GetComplitlySimilarWithAnotherId(this.service.typeofregister,this.service.typeofregisterall);
     if( similartypeofregester!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(typeofregister:TypeOfRegister):boolean{
    if(typeofregister==null||typeofregister.Id==0){
      return false;
    }
    if(typeofregister.Name.trim()==""){
      return false;
    }
    if(typeofregister.Name==this.temptypeofregister.Name&&typeofregister.IsEnabled==this.temptypeofregister.IsEnabled){
      return false;
    }
    var similartypeofregester = this.helperService.GetComplitlySimilarWithAnotherId(typeofregister,this.service.typeofregisterall);
    if(similartypeofregester!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.typeofregister.Name = this.temptypeofregister.Name;
    this.service.typeofregister.IsEnabled = this.temptypeofregister.IsEnabled;
    this.service.typeofregister = new TypeOfRegister;
    this.service.typeofregister.IsEnabled = true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getTypeOfRegister().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.typeofregisterall= response as TypeOfRegister[]);
      this.CheckArrayIsNull(this.service.typeofregisterall,"نوع التسجيل ")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    this.service.typeofregister=new TypeOfRegister;
    this.displayedColumns= ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateTypeOfRegistar() || !this.roleservic.CanDeleteTypeOfRegistar()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.typeofregister.IsEnabled=true
   this.semelar=null
  }
  
  submit(form:NgForm){ 
   this.showSpinner();
      this.service.postTypeOfRegister().subscribe(res=>{
        this.hideSpinner();
        this.service.typeofregisterall.push(res as TypeOfRegister);
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
        this.service.typeofregister = new TypeOfRegister();
        this.CheckArrayIsNull(this.service.typeofregisterall,"نوع التسجيل ")
      this.service.typeofregister.IsEnabled=true
   this.semelar=null
      },
      err=>{
         this.hideSpinner();
      })
  
    }   
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putTypeOfRegister().subscribe(res=>{
          this.hideSpinner();
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.closeModal();
        },
        err=>{
           this.hideSpinner();
        })
      
    }
  
    
  fillData(item){
    this.service.typeofregister=item;
    this.temptypeofregister= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deleteTypeOfRegister(id).subscribe(res=>{
      this.hideSpinner();
    var typeofregister = this.service.typeofregisterall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(typeofregister);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.typeofregisterall,"نوع التسجيل ")
    },err=>{
      this.hideSpinner();
    });
  }
 

}
