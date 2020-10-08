import { Component, OnInit,ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { NationalityService } from 'src/app/Service/nationality.service';
import { Nationality } from 'src/app/Model/nationality.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-nationality',
  templateUrl: './nationality.component.html',
  styleUrls: ['./nationality.component.css']
})
export class NationalityComponent extends AppComponent implements OnInit {


  displayedColumns: string[];
    dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: Nationality;
  tempnationalty: Nationality;
  errorMessage:string;
  constructor(public service:NationalityService,public roleservic:RoleService,
    public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
  
  }
  GetSimilarForUpdate(nationalty:Nationality){
    if(nationalty.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(nationalty, this.service.nationaltyall);  
  }

  GetSimilarForAdd(nationalty: Nationality) {
    if(nationalty.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(nationalty, this.service.nationaltyall);
  }

  error():boolean {
    if (this.service.nationalty.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarNationalty= this.helperService.GetComplitlySimilarWithAnotherId(this.service.nationalty,this.service.nationaltyall);
     if( similarNationalty!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(nationalty:Nationality):boolean{
    if(nationalty==null||nationalty.Id==0){
      return false;
    }
    if(nationalty.Name.trim()==""){
      return false;
    }
    if(nationalty.Name==this.tempnationalty.Name){
      return false;
    }
    var similarNationalty = this.helperService.GetComplitlySimilarWithAnotherId(nationalty,this.service.nationaltyall);
    if(similarNationalty!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.nationalty.Name = this.tempnationalty.Name;
    //this.service.nationalty.IsEnabled = this.tempnationalty.IsEnabled;
    this.service.nationalty = new Nationality;
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getNationality().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.nationaltyall= response as Nationality[]);
      this.CheckArrayIsNull(this.service.nationaltyall,"جنسيات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
     
    },
    err => {
      this.hideSpinner();
    });
    this.displayedColumns = ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateNationality() || !this.roleservic.CanDeleteNationality()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.nationalty=new Nationality;
  }
  
  submit(form:NgForm){
    this.showSpinner();
      this.service.postNationality().subscribe(res=>{
        this.hideSpinner();
        this.service.nationaltyall.push(res as Nationality);
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
        this.service.nationalty = new Nationality();
      this.semelar=null
      this.CheckArrayIsNull(this.service.nationaltyall,"جنسيات")
      },
      err=>{
         this.hideSpinner();
      })
  
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putNationality().subscribe(res=>{
          this.hideSpinner();
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.semelar=null
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }
   
    
  fillData(item){
    this.service.nationalty=item;
    this.tempnationalty= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deleteNationality(id).subscribe(res=>{
      this.hideSpinner();
    var nationality = this.service.nationaltyall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(nationality);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.nationaltyall,"جنسيات")
    },err=>{
      this.hideSpinner();
    });
  }

 
}
