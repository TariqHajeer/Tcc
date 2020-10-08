import { Component, OnInit,ViewChild } from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { MaincountryService } from 'src/app/Service/maincountry.service';
import { Maincountry } from 'src/app/Model/maincountry.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-maincountry',
  templateUrl: './maincountry.component.html', 
  styleUrls: ['./maincountry.component.css']
})
export class MaincountryComponent extends AppComponent implements OnInit {

  displayedColumns: string[] ;
  dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: Maincountry;
  temp: Maincountry;
  errorMessage:string;
  constructor(public service:MaincountryService,public roleservic:RoleService,
    public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
   this.service.maincountry=new Maincountry;
   this.service.maincountry.IsEnabled=true
   this.semelar=null
  }
  GetSimilarForUpdate(maincountry:Maincountry){
    if(maincountry.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(maincountry, this.service.maincountryall);  
  }

  GetSimilarForAdd(maincountry: Maincountry) {
    if(maincountry.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(maincountry, this.service.maincountryall);
  }

  error():boolean {
    if (this.service.maincountry.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarMaincountry= this.helperService.GetComplitlySimilarWithAnotherId(this.service.maincountry,this.service.maincountryall);
     if( similarMaincountry!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(maincountry:Maincountry):boolean{
    if(maincountry==null||maincountry.Id==0){
      return false;
    }
    if(maincountry.Name.trim()==""){
      return false;
    }
    if(maincountry.Name==this.temp.Name&&maincountry.IsEnabled==this.temp.IsEnabled){
      return false;
    }
    var similarMaincountry = this.helperService.GetComplitlySimilarWithAnotherId(maincountry,this.service.maincountryall);
    if(similarMaincountry!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.maincountry.Name = this.temp.Name;
    this.service.maincountry.IsEnabled = this.temp.IsEnabled;
    this.service.maincountry = new Maincountry;
    this.service.maincountry.IsEnabled=true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getcountry().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.maincountryall= response as Maincountry[]);
      this.CheckArrayIsNull(this.service.maincountryall,"بلدان")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
    ,
      err => {
        this.hideSpinner();
      });
    this.displayedColumns= ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateCountry() || !this.roleservic.CanDeleteCountry()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
  }
  
  submit(form:NgForm){
this.showSpinner();
      this.service.postcountry().subscribe(res=>{
        this.hideSpinner();
          this.service.maincountryall.push(res as Maincountry);
          this.dataSource._updateChangeSubscription();
          this.helperService.add();   this.resetForm(form);
      this.service.maincountry=new Maincountry;
      this.service.maincountry.IsEnabled=true
      this.semelar=null
      this.CheckArrayIsNull(this.service.maincountryall,"بلدان")
      },
      err=>{
         this.hideSpinner();
      })
 
   
  
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putcountry().subscribe(res=>{
          this.hideSpinner();
          this.get();this.resetForm(form);
          this.service.maincountry=new Maincountry;
           this.helperService.edit();
      this.resetForm(form);
          this.service.maincountry.IsEnabled=true
          this.semelar=null
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }

    
  fillData(item){
    this.service.maincountry=item;
    this.temp= Object.assign({}, item);
     
  }

  delete(id){
    this.showSpinner();
    this.service.deletecountry(id).subscribe(res=>{
      this.hideSpinner();
    var maincountry = this.service.maincountryall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(maincountry);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
    this.helperService.delete();
    this.CheckArrayIsNull(this.service.maincountryall,"بلدان")
  },
    err => {
      this.hideSpinner();
    });
  }



}
