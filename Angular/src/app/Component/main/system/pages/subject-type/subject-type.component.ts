import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { SubjectType } from 'src/app/Model/subject-type.model';
import { SubjectTypeService } from 'src/app/Service/subject-type.service';
import { YearService } from 'src/app/Service/year.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-subject-type',
  templateUrl: './subject-type.component.html',
  styleUrls: ['./subject-type.component.css']
})
export class SubjectTypeComponent extends AppComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  semelar: SubjectType;
  tempSubjectType: SubjectType;
  errorMessage: string;
  constructor(public service: SubjectTypeService, public helperService: HelpService,
    public roleservic: RoleService, public YearService: YearService, public spinner: NgxSpinnerService) {
    super(spinner,helperService);
    this.service.SubjectType = new SubjectType();
  }


  ngOnInit(): void {
    this.service.SubjectType = new SubjectType;
    this.get();

  }
  GetSimilarForUpdate(SubjectType: SubjectType) {
    if (SubjectType.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(SubjectType, this.service.SubjectTypes);
  }

  GetSimilarForAdd(SubjectType: SubjectType) {
    if (SubjectType.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(SubjectType, this.service.SubjectTypes);
  }
  error(): boolean {
    if (this.service.SubjectType.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarSubjectType = this.helperService.GetComplitlySimilarWithAnotherId(this.service.SubjectType, this.service.SubjectTypes);
    if (similarSubjectType != null) {
     this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage = "";
    return false;
  }
  UpdateValidation(SubjectType: SubjectType): boolean {
    if (SubjectType == null || SubjectType.Id == 0) {
      return false;
    }
    if (SubjectType.Name.trim() == "") {
      return false;
    }
    if(SubjectType.Name==this.tempSubjectType.Name&&SubjectType.IsEnabled==this.tempSubjectType.IsEnabled){
      return false;
    }
    var similarSubjectType = this.helperService.GetComplitlySimilarWithAnotherId(SubjectType, this.service.SubjectTypes);
    if (similarSubjectType != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage = "";
    this.error();
    this.service.SubjectType.Name = this.tempSubjectType.Name;
    this.service.SubjectType.IsEnabled = this.tempSubjectType.IsEnabled;
    this.service.SubjectType = new SubjectType;
    this.service.SubjectType.IsEnabled =true
  }
  get() {
    this.showSpinner();
    this.displayedColumns = ['Name', 'IsEnabled', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    this.service.GetSubjectType().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.SubjectTypes = response as SubjectType[]);
      this.CheckArrayIsNull(this.service.SubjectTypes,"أنواع مواد")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    if (!this.roleservic.CanUpdateSubjectType() || !this.roleservic.CanDeleteSubjectType()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
     
    }
    this.service.SubjectType.IsEnabled=true
    this.semelar=null
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  submit(form:NgForm) {
    this.showSpinner();
    this.service.postSubjectType().subscribe(res => {
      this.hideSpinner();
      this.service.SubjectTypes.push(res as SubjectType);
      this.dataSource._updateChangeSubscription();
      this.helperService.add();   this.resetForm(form);
      this.service.SubjectType = new SubjectType();
      this.service.SubjectType.IsEnabled=true
   this.semelar=null
   this.CheckArrayIsNull(this.service.SubjectTypes,"أنواع مواد")
    },
      err => {
        this.hideSpinner();
       
      });



  }
  edit(form:NgForm) {
    this.showSpinner();
    this.service.putSubjectType().subscribe(res => {
      this.hideSpinner();
      this.get();
      this.resetForm(form);
      this.service.SubjectType = new SubjectType();
       this.helperService.edit();
       this.service.SubjectType .IsEnabled=true;
      this.tempSubjectType = null;
    },
      err => {
        this.hideSpinner();
        this.closeModal();
      })

  }



  fillData(item) {
    this.errorMessage = "";
    this.service.SubjectType = item;
    this.tempSubjectType = Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.deleteSubjectType(id).subscribe(res => {
      this.hideSpinner();
      var SubjectType = this.service.SubjectTypes.filter(c => c.Id == id)[0];
      var index = this.dataSource.data.indexOf(SubjectType);
      this.dataSource.data.splice(index, 1);
      this.dataSource._updateChangeSubscription();
         this.helperService.delete();
         this.CheckArrayIsNull(this.service.SubjectTypes,"أنواع مواد")
    },err=>{
      this.hideSpinner();
    });

  }

}
