import { Component, OnInit, ViewChild } from '@angular/core';
//import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { YearService } from 'src/app/Service/year.service';
import { YearSystemService } from 'src/app/Service/year-system.service';
import { ExampSystemService } from 'src/app/Service/examp-system.service';
import { NgForm } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { ResponseYear, UpdateYearDTO, Year } from 'src/app/Model/year.model';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
@Component({
  selector: 'app-year',
  templateUrl: './year.component.html',
  styleUrls: ['./year.component.css']
})
export class YearComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
  dataSource
  erorrMessage: string;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(/*private _formBuilder: FormBuilder*/public service: YearService,
    public yearsystemservic: YearSystemService,
    public exampsystemservic: ExampSystemService
    , public roleservic: RoleService,
    public helperService: HelpService,
    public spinner: NgxSpinnerService,
    public route: Router) {
    super(spinner, helperService);
  }
  /*  isLinear = false;
    firstFormGroup: FormGroup;
    secondFormGroup: FormGroup;*/
  ngOnInit(): void {
    /* this.firstFormGroup = this._formBuilder.group({
       firstCtrl: ['', Validators.required]
     });
     this.secondFormGroup = this._formBuilder.group({
       secondCtrl: ['', Validators.required]
     });*/
    this.get();
    this.exampsystemservic.getExampSystem();
    this.yearsystemservic.getYearSystemAll();
  }
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  calcSecoundYear(): void {
    if (this.service.year.FirstYear == 0 || this.service.year.FirstYear == undefined) {
      this.service.year.SecondYear = null;
    }
    this.service.year.SecondYear = +this.service.year.FirstYear + 1;
    if (this.service.year.SecondYear == 1)
      this.service.year.SecondYear = undefined;
  }
  Erorr(): boolean {
    this.erorrMessage = "";
    if (this.service.year.SecondYear == undefined) {
      return true;
    }
    var firstYear = this.service.year.FirstYear;
    if (firstYear.toString().length < 4) {
      return true;
    }
    var similarYear = this.service.years.filter(c => c.FirstYear == this.service.year.FirstYear)[0];
    if (similarYear != null) {
      this.erorrMessage = "السنة موجودة من قبل";
      //  this.helperService.toastr.error("",this.erorrMessage);
      return true;
    }
    return false;
  }
  years:ResponseYear[]=[]
  get() {
    this.showSpinner();
    this.service.getYear().subscribe(response => {
      this.hideSpinner();
      this.years = response as ResponseYear[]
      console.log(response)
      if (this.years.length == 1)
        this.years[0].Updatepla = true
      this.dataSource = new MatTableDataSource(this.years);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
      err => {
        this.hideSpinner();
      });
    this.displayedColumns = ['Year', 'Created', 'CreatedBy', 'Yearystem', 'ExamSystem', 'More'];
    if (!this.roleservic.CanUpdateYear() || !this.roleservic.CanDeleteYear())
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")

    this.service.year = new Year;
  }
  submit(form: NgForm) {
    if (this.service.year.FirstYear < 1998) {
      this.helperService.toastr.error("", "لايمكن اضافة سنة قبل ال1999")
    }
    //     else if(this.service.year.FirstYear>2020){
    //       var prevYear= this.service.yearall[this.service.yearall.length-1]
    //       if(!prevYear.Blocked){
    // this.helperService.toastr.error("","يجب قفل السنة السابقة قبل الاضافة");
    //       }

    //     }
    else {
      this.showSpinner();
      this.service.postYear().subscribe(res => {
        this.hideSpinner();
        //this.service.yearall.push(res as Year);
        this.get()
        this.dataSource._updateChangeSubscription();
        this.helperService.add(); this.resetForm(form);
        this.service.year = new Year;
      },
        err => {

          this.hideSpinner();
        })

    }

  }
  LockedYear(Id) {
    this.route.navigate(['/home/system/blockedyear', Id])
  }
  EditYear: UpdateYearDTO = new UpdateYearDTO()
  edit(form: NgForm) {
    this.EditYear.ExamSystem=Number(this.EditYear.ExamSystem)
    console.log(this.EditYear)
    this.showSpinner()
    this.service.UpdateYear(this.service.year.Id, this.EditYear).subscribe(res => {
      this.resetForm(form);
      this.hideSpinner()
      this.get()
      this.helperService.edit()
    }, err => {
      this.hideSpinner()
    })
  }
  fillData(item: ResponseYear) {
    this.service.year.SecondYear=item.SecondYear
    this.service.year.FirstYear=item.FirstYear
    this.service.year.Id=item.Id
    this.EditYear.ExamSystem=Number(item.ExamSystem.Id)
    this.EditYear.YearSystem=Number(item.Yearystem.Id)
  }
  DeleteYear(element) {
    this.showSpinner()
    this.service.DeleteYear(element).subscribe(res => {
      this.helperService.delete()
      this.hideSpinner()
      this.get()
    }, err => {
      this.hideSpinner()
    })
  }
}
