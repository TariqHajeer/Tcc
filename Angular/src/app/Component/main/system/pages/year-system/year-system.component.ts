import { Component, OnInit, ViewChild } from '@angular/core';
import { YearSystemService } from 'src/app/Service/year-system.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { YearSystem } from 'src/app/Model/year-system.model';
import { NgForm } from '@angular/forms';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Setting } from 'src/app/Model/setting.model';
@Component({
  selector: 'app-year-system',
  templateUrl: './year-system.component.html',
  styleUrls: ['./year-system.component.css']
})
export class YearSystemComponent extends AppComponent implements OnInit {
  scrollToElement($element): void {
    $element.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
  }
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(public service: YearSystemService, public roleservic: RoleService,
     public helperService: HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService);
  }
  public isCollapsed = false;
  results
  semelar: YearSystem;
  tempyearsystem: YearSystem;
  errorMessage: string;
  ngOnInit(): void {

    this.get();
  }

  GetSimilarForUpdate(yearsystem: YearSystem) {
    if (yearsystem.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(yearsystem, this.service.yearSystems);
  }

  GetSimilarForAdd(yearsystem: YearSystem) {
    if (yearsystem.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(yearsystem, this.service.yearSystems);
  }

  error(): boolean {
    if (this.service.yearsystem.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similaryearsystem = this.helperService.GetComplitlySimilarWithAnotherId(this.service.yearsystem, this.service.yearSystems);
    if (similaryearsystem != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }
    this.errorMessage = "";
    return false;
  }
  UpdateValidation(yearsystem: YearSystem): boolean {
    if (yearsystem == null || yearsystem.Id == 0) {
      return false;
    }
    if (yearsystem.Name.trim() == "") {
      return false;
    }
    if(yearsystem.Name==this.tempyearsystem.Name&&yearsystem.IsMain==this.tempyearsystem.IsMain){
      return false;
    }
    var similaryearsystem = this.helperService.GetComplitlySimilarWithAnotherId(yearsystem, this.service.yearSystems);
    if (similaryearsystem != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  get() {
    this.showSpinner();
    this.service.getYearSystem().subscribe(response => {
      this.hideSpinner();
      this.service.yearSystems = response as YearSystem[];
      this.CheckArrayIsNull(this.service.yearSystems,"نظام سنة ")
      this.dataSource = new MatTableDataSource(this.service.yearSystems);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
     
    },
    err => {
      this.hideSpinner();
    });
    this.service.yearsystem = new YearSystem();
    this.service.FillSettinInYearSystem();
    this.displayedColumns = ['Name', 'IsMain', 'Note', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    if (!this.roleservic.CanUpdateYearSystem() || !this.roleservic.CanDeleteYearSystem()) 
    this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    this.service.yearsystem.IsMain=true
    this.semelar=null
  }
  chechAdministrativeleadership(item:Setting){
if(item.Id==1&&item.Count>10){
item.Count=10
}
else{return;}
  }
  submit(form:NgForm) {
    if (this.service.yearsystem.Id == 0) {
      
      this.showSpinner();
      this.service.postYearSystem().subscribe(res => {
        this.hideSpinner();
     //   this.service.yearSystems.push(res as YearSystem);
        this.dataSource._updateChangeSubscription();
        this.helperService.add();  
        this.get()
         this.resetForm(form);
        this.service.yearsystem = new YearSystem();
        this.service.FillSettinInYearSystem();
        this.service.yearsystem.IsMain=true
        this.semelar=null;
      })
    }
    else {
      this.helperService.edit();
      this.resetForm(form);
        this.RetreatEdit();
    }
  }
  delete(id) {
    this.showSpinner();
this.service.DeleteYearSystem(id).subscribe(res=>{
  this.get();
  this.hideSpinner();
},err=>{
 
  this.hideSpinner();
})
  }
Edit(form:NgForm,id){
  this.showSpinner()
this.service.UpdateYearSystem(id).subscribe(res=>{
this.hideSpinner()
this.resetForm(form);
},err=>{
  this.hideSpinner()
})
}

  fillData(element: YearSystem) {
    this.checkAdd=true;
    this.checkshow=true
    this.checkEdit=false;
    this.titleAddOrEdit="تعديل نظام السنة"
    this.service.yearsystem = element;
    document.getElementById('mat-tab-label-0-0').click();
  }

  checkEdit:boolean=true;
  checkAdd:boolean=false;
  checkshow:boolean=false
  titleAddOrEdit="اضافة نظام السنة"
  RetreatEdit(){
    this.checkAdd=false;
    this.checkshow=false
    this.checkEdit=true;
    this.titleAddOrEdit="اضافة نظام السنة"
    this.service.yearsystem = new YearSystem();
    this.service.FillSettinInYearSystem();
  }
  resetformadd(){
   if(this.checkshow==false){
    this.titleAddOrEdit="اضافة نظام السنة"
    this.checkAdd=false;
    this.checkEdit=true;
    this.service.yearsystem = new YearSystem();
    this.service.FillSettinInYearSystem();
   }
   if(this.checkshow==true)this.checkshow=false
  }
  UpdateName():boolean{
    if(this.service.yearsystem.Name=='')
    return true
    else
    return false
  }
  fillDataShow(element){
    this.service.yearsystem=element
  }
}
