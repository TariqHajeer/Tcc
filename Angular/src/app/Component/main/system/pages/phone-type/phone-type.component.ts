import { Component, OnInit ,ViewChild} from '@angular/core';
import { PhoneTypeService } from 'src/app/Service/phone-type.service';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { PhoneType } from 'src/app/Model/phone-type.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-phone-type',
  templateUrl: './phone-type.component.html',
  styleUrls: ['./phone-type.component.css']
}) 
export class PhoneTypeComponent extends AppComponent implements OnInit {
  displayedColumns: string[] ;
    dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: PhoneType;
  tempphonetype: PhoneType;
  errorMessage:string;
  constructor(public service:PhoneTypeService,public roleservic:RoleService
    ,public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
   this.service.phonetype.IsEnabled=true
      this.semelar=null
  }
  GetSimilarForUpdate(phonetype:PhoneType){
    if(phonetype.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(phonetype, this.service.phonetypeall);  
  }

  GetSimilarForAdd(phonetype: PhoneType) {
    if(phonetype.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(phonetype, this.service.phonetypeall);
  }

  error():boolean {
    if (this.service.phonetype.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarPhonyType= this.helperService.GetComplitlySimilarWithAnotherId(this.service.phonetype,this.service.phonetypeall);
     if( similarPhonyType!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(phonetype:PhoneType):boolean{
    if(phonetype==null||phonetype.Id==0){
      return false;
    }
    if(phonetype.Name.trim()==""){
      return false;
    }
    if(phonetype.Name==this.tempphonetype.Name&&phonetype.IsEnabled==this.tempphonetype.IsEnabled){
      return false;
    }
    var similarPhonyType = this.helperService.GetComplitlySimilarWithAnotherId(phonetype,this.service.phonetypeall);
    if(similarPhonyType!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.phonetype.Name = this.tempphonetype.Name;
    this.service.phonetype.IsEnabled = this.tempphonetype.IsEnabled;
    this.service.phonetype = new PhoneType;
    this.service.phonetype.IsEnabled =true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  get()
  {
    this.showSpinner();
    this.service.getPhoneType().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.phonetypeall= response as PhoneType[]);
      this.CheckArrayIsNull(this.service.phonetypeall,"أنواع هاتف")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    this.displayedColumns= ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdatePhoneType() || !this.roleservic.CanDeletePhoneType()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.phonetype= new PhoneType;
  }
  
  submit(form:NgForm){
   
    this.showSpinner();
      this.service.postPhoneType().subscribe(res=>{
        this.hideSpinner();
        this.service.phonetypeall.push(res as PhoneType);
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
        this.service.phonetype = new PhoneType();
      this.service.phonetype.IsEnabled=true
      this.semelar=null
      this.CheckArrayIsNull(this.service.phonetypeall,"أنواع هاتف")
      },
      err=>{
         this.hideSpinner();
      })
   
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      
        this.service.putPhoneType().subscribe(res=>{
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.service.phonetype.IsEnabled=true
          this.semelar=null
        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }

    
  fillData(item){
    this.service.phonetype=item;
    this.tempphonetype= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deletePhoneType(id).subscribe(res=>{
      this.hideSpinner();
    var phonetype = this.service.phonetypeall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(phonetype);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
    this.helperService.delete();
    this.CheckArrayIsNull(this.service.phonetypeall,"أنواع هاتف")
  },
    err => {
      this.hideSpinner();
    });
  }
 
}
