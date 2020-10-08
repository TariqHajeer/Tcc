import { Component, OnInit,ViewChild } from '@angular/core';
import { AppComponent } from 'src/app/app.component';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { LanguagesService } from 'src/app/Service/languages.service';
import { Languages } from 'src/app/Model/languages.model';
import { RoleService } from 'src/app/Help/role.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-languages',
  templateUrl: './languages.component.html',
  styleUrls: ['./languages.component.css']
})
export class LanguagesComponent extends AppComponent implements OnInit {

  displayedColumns: string[] ;
   dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: Languages;
  temp: Languages;
  errorMessage:string;
  constructor(public service:LanguagesService,public roleservic:RoleService
    ,public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
   this.service.language=new Languages;
   this.service.language.IsEnabled=true
   this.semelar=null
  }
  GetSimilarForUpdate(language:Languages){
    if(language.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(language, this.service.languageall);  
  }

  GetSimilarForAdd(language: Languages) {
    if(language.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(language, this.service.languageall);
  }

  error():boolean {
    if (this.service.language.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarlanguage= this.helperService.GetComplitlySimilarWithAnotherId(this.service.language,this.service.languageall);
     if( similarlanguage!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(language:Languages):boolean{
    if(language==null||language.Id==0){
      return false;
    }
    if(language.Name.trim()==""){
      return false;
    }
    if(language.Name==this.temp.Name&&language.IsEnabled==this.temp.IsEnabled){
      return false;
    }
    var similarlanguage = this.helperService.GetComplitlySimilarWithAnotherId(language,this.service.languageall);
    if(similarlanguage!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.language.Name = this.temp.Name;
    this.service.language.IsEnabled = this.temp.IsEnabled;
    this.service.language = new Languages;
    this.service.language.IsEnabled =true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getLanguages().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.languageall= response as Languages[]);
      this.CheckArrayIsNull(this.service.languageall,"لغات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
    ,
      err => {
        this.hideSpinner();
      });
    this.displayedColumns= ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateLanguages() || !this.roleservic.CanDeleteLanguages()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
  }
  
  submit(form:NgForm){
   this.showSpinner();
      this.service.postLanguages().subscribe(res=>{
        this.hideSpinner();
        this.service.languageall.push(res as Languages);
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
      this.service.language=new Languages;
      this.service.language.IsEnabled=true
      this.semelar=null
      this.CheckArrayIsNull(this.service.languageall,"لغات")
      },
      err=>{
         this.hideSpinner();
      })
   
   
  
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
       this.service.putLanguages().subscribe(res=>{
         this.hideSpinner();
         
          this.get();
          this.service.language=new Languages;
           this.helperService.edit();
      this.resetForm(form);
          this.service.language.IsEnabled=true
          this.semelar=null
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }
   
    
  fillData(item){
    this.service.language=item;
    this.temp= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deleteLanguages(id).subscribe(res=>{
      this.hideSpinner();
    var language = this.service.languageall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(language);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.languageall,"لغات")
    },err=>{
      this.hideSpinner();
    });
  }
 
}
