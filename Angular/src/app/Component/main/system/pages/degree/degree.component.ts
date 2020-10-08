import { Component, OnInit, ViewChild } from '@angular/core';
import { DegreeService } from 'src/app/Service/degree.service';
import { NgForm } from '@angular/forms';
import { Degree } from 'src/app/Model/degree.model';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';



@Component({
  selector: 'app-degree',
  templateUrl: './degree.component.html',
  styleUrls: ['./degree.component.css']
})
export class DegreeComponent extends AppComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  displayedColumns: string[];
  dataSource = new MatTableDataSource<Degree>(this.service.degreeall);
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  semelar: Degree;
  tempDegree: Degree;
  errorMessage: string;
  constructor(public service: DegreeService, public helperService: HelpService,
    public roleservic: RoleService, public spinner: NgxSpinnerService) {
    super(spinner,helperService);
    this.service.degree = new Degree();
  }


  ngOnInit(): void {
    this.service.degree = new Degree;
    this.get();
    this.service.degree.IsEnabled=true

  }
  GetSimilarForUpdate(degree: Degree) {
    if (degree.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(degree, this.service.degreeall);
  }

  GetSimilarForAdd(degree: Degree) {
    if (degree.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(degree, this.service.degreeall);
  }
  error(): boolean {
    if (this.service.degree.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarDegree = this.helperService.GetComplitlySimilarWithAnotherId(this.service.degree, this.service.degreeall);
    if (similarDegree != null) {
      //this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }
    this.errorMessage = "";
    return false;
  }
  UpdateValidation(degree: Degree): boolean {
    if (degree == null || degree.Id == 0) {
      return false;
    }
    if (degree.Name.trim() == "") {
      return false;
    }
    if(degree.Name==this.tempDegree.Name&&degree.IsEnabled==this.tempDegree.IsEnabled){
      return false;
    }
    var similarDegree = this.helperService.GetComplitlySimilarWithAnotherId(degree, this.service.degreeall);
    if (similarDegree != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage = "";
    this.error();
    this.service.degree.Name = this.tempDegree.Name;
    this.service.degree.IsEnabled = this.tempDegree.IsEnabled;
    this.service.degree = new Degree;
    this.semelar=null
      this.service.degree.IsEnabled=true
  }
  get() {
    this.showSpinner();
    this.displayedColumns  = ['Name', 'IsEnabled', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    this.service.getDegree().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.degreeall = response as Degree[]);
      this.CheckArrayIsNull(this.service.degreeall,"شهادات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },err=>{
      this.hideSpinner();
    });
    if (!this.roleservic.CanUpdateDegree() || !this.roleservic.CanDeleteDegree()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  submit(form:NgForm) {
    this.showSpinner();
    this.service.postDegree().subscribe(res => {
      this.hideSpinner();
      this.service.degreeall.push(res as Degree);
      this.errorMessage="";
      this.dataSource._updateChangeSubscription();
      this.resetForm(form);
      this.service.degree = new Degree();
      this.helperService.add();
      this.semelar=null
      this.service.degree.IsEnabled=true
      this.CheckArrayIsNull(this.service.degreeall,"شهادات")
    },
      err => {
        this.hideSpinner();
      })



  }
  edit(form:NgForm) {  
    this.errorMessage = " ";
    this.showSpinner();
    this.service.putDegree().subscribe(res => {
      this.hideSpinner();
      this.get();
      this.service.degree = new Degree();
       this.helperService.edit();
      this.resetForm(form);
      this.tempDegree = null;
      this.semelar=null
      this.service.degree.IsEnabled=true
    },
      err => {
        this.hideSpinner();
        this.closeModal();
      })

  }



  fillData(item:Degree) {
    this.errorMessage = "";
    this.service.degree = item;
    this.tempDegree = Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.deleteDegree(id).subscribe(res => {
      this.hideSpinner();
      var degree = this.service.degreeall.filter(c => c.Id == id)[0];
      var index = this.dataSource.data.indexOf(degree);
      this.dataSource.data.splice(index, 1);
      this.dataSource._updateChangeSubscription();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.degreeall,"شهادات")
    },err=>{
      this.hideSpinner();
    });
  }

}


