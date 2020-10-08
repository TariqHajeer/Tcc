import { Component, OnInit, ViewChild } from '@angular/core';
import { HonestyService } from 'src/app/Service/honesty.service';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { Honesty } from 'src/app/Model/honesty.model';
import { CountriesService } from 'src/app/Service/countries.service';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-honesty',
  templateUrl: './honesty.component.html',
  styleUrls: ['./honesty.component.css']
})
export class HonestyComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
   dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: Honesty;
  tempHonesty: Honesty;
  errorMessage:string;
  constructor(public service:HonestyService,public mainservic:CountriesService
    ,public roleservic:RoleService,public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
    this.get();
    this.mainservic.getEnabled();
     
  }
  
  GetSimilarForUpdate(honesty:Honesty){
    if(honesty.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(honesty, this.service.honestyall);  
  }

  GetSimilarForAdd(honesty: Honesty) {
    if(honesty.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(honesty, this.service.honestyall);
  }

  error():boolean {
    if (this.service.honesty.Name.trim() == ''||this.service.honesty.CountryId==null) {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    // var similarhonsty= this.helperService.GetComplitlySimilarWithAnotherId(this.service.honesty,this.service.honestyall);
    //  if( similarhonsty!=null){
    //  this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
    //  return true;
    //  }
    var findhonesty=this.service.honestyall.find(c=>c.Name==this.service.honesty.Name&&c.CountryId==this.service.honesty.CountryId)
    if(findhonesty!=null||findhonesty!=undefined){
     this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }else
   { this.errorMessage="";
    return false;
  }
  }
  UpdateValidation(honesty:Honesty):boolean{
    if(honesty==null||honesty.Id==0){
      return false;
    }
    if(honesty.Name.trim()==""){
      return false;
    }
    if(honesty.Name==this.tempHonesty.Name&&honesty.IsEnabled==this.tempHonesty.IsEnabled){
      return false;
    }
    var similarhonsty = this.helperService.GetComplitlySimilarWithAnotherId(honesty,this.service.honestyall);
    if(similarhonsty!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.honesty.Name = this.tempHonesty.Name;
    this.service.honesty.IsEnabled = this.tempHonesty.IsEnabled;
    this.service.honesty = new Honesty;
    this.service.honesty.IsEnabled =true
  } 

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getHonesty().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.honestyall= response as Honesty[]);
      this.CheckArrayIsNull(this.service.honestyall,"امانات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
    ,
      err => {
        this.hideSpinner();
      });
    this.displayedColumns = ['Name', 'IsEnabled','City', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateHonesty() || !this.roleservic.CanDeleteHonesty()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.honesty=new Honesty;
    this.service.honesty.IsEnabled=true
    this.semelar=null
  }
  submit(form:NgForm){
    this.showSpinner();
      this.service.postHonesty().subscribe(res=>{
        this.hideSpinner();
       this.helperService.add();   this.resetForm(form);
       this.get();
        this.service.honesty=new Honesty;
        this.service.honesty.IsEnabled=true
        this.semelar=null
        this.CheckArrayIsNull(this.service.honestyall,"امانات")
      },
      err=>{
       
         this.hideSpinner();
      })
  
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putHonesty().subscribe(res=>{
          this.hideSpinner();
          this.get();
          this.service.honesty.IsEnabled=true
          this.semelar=null
           this.helperService.edit();
      this.resetForm(form);
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }
  fillData(item){
    this.service.honesty=item;
    this.tempHonesty= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deleteHonesty(id).subscribe(res=>{
      this.hideSpinner();
    var honesty = this.service.honestyall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(honesty);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.honestyall,"امانات")
    },err=>{
      this.hideSpinner();
    });
  }

  
 
  
}
