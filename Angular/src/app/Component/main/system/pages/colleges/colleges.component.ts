import { Component, OnInit, ViewChild } from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { CollegesService } from 'src/app/Service/colleges.service';
import { CountriesService } from 'src/app/Service/countries.service';
import { Colleges } from 'src/app/Model/colleges.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { MaincountryService } from 'src/app/Service/maincountry.service';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-colleges',
  templateUrl: './colleges.component.html',
  styleUrls: ['./colleges.component.css']
})
export class CollegesComponent extends AppComponent implements OnInit {

  displayedColumns: string[];
    dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  temp: Colleges;
  semelar: Colleges;
  errorMessage:string;
  constructor(public service:CollegesService
    ,public CountriesService:MaincountryService
    ,public roleservic:RoleService,public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
    this.get();
    this.service.collage.IsEnabled=true
    this.semelar=null
      this.CountriesService.GetAllCountryAndCities();
  }
  GetSimilarForUpdate(collage:Colleges){
    if(collage.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(collage, this.service.collageall);  
  }

  GetSimilarForAdd(collage: Colleges) {
    if(collage.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(collage, this.service.collageall);
  }

  error():boolean {
    if (this.service.collage.Name.trim() == ''||this.service.collage.ProvinceId==null) {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var findhonesty=this.service.collageall.find(c=>c.Name==this.service.collage.Name&&c.ProvinceId==this.service.collage.ProvinceId)
    if(findhonesty!=null||findhonesty!=undefined){
     this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }else
   { this.errorMessage="";
    return false;
  }
  }
  UpdateValidation(collage:Colleges):boolean{
    if(collage==null||collage.Id==0){
      return false;
    }
    if(collage.Name==""){
      return false;
    }
    if(collage.Name==this.temp.Name&&collage.IsEnabled==this.temp.IsEnabled){
      return false;
    }
    var similarCollage = this.helperService.GetComplitlySimilarWithAnotherId(collage,this.service.collageall);
    if(similarCollage!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.collage.Name = this.temp.Name;
    this.service.collage.IsEnabled = this.temp.IsEnabled;
    this.service.collage = new Colleges;
    this.service.collage.IsEnabled=true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.displayedColumns=['Name', 'IsEnabled','City', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
   this.showSpinner();
    this.service.getColleges().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.collageall= response as Colleges[]);
      this.CheckArrayIsNull(this.service.collageall,"جامعات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },err=>{
      this.hideSpinner();
    });
    if (!this.roleservic.CanUpdateCollege() || !this.roleservic.CanDeleteCollege()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.collage = new Colleges;
  }
  submit(form:NgForm){
   this.showSpinner();
      this.service.postColleges().subscribe(res=>{
        this.hideSpinner();
        
        this.resetForm(form);
        this.get();
        this.service.collage=new Colleges;
        this.service.collage.IsEnabled=true;
        this.semelar=null;
        this.CheckArrayIsNull(this.service.collageall,"جامعات")
        this.helperService.add();  
        
      },
      err=>{
       
         this.hideSpinner();
      })
   
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
     this.showSpinner();
        this.service.putColleges().subscribe(res=>{
          this.hideSpinner();
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.service.collage.IsEnabled=true;
          this.semelar=null;
        },
        err=>{
           this.hideSpinner();
          this.get();
        
        })
      
    }
  fillData(item){
    this.service.collage=item;
    this.temp= Object.assign({}, item);

  }

  delete(id){
    this.showSpinner();
    this.service.deleteColleges(id).subscribe(res=>{
      this.hideSpinner();
    var collage = this.service.collageall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(collage);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
    this.CheckArrayIsNull(this.service.collageall,"جامعات")
    this.helperService.delete();}
    ,
      err => {
        this.hideSpinner();
      });
  } 
 

 
}
