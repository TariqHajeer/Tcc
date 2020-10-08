import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HelpService } from 'src/app/Help/help.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { Specialization } from 'src/app/Model/specialization.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-specialization',
  templateUrl: './specialization.component.html',
  styleUrls: ['./specialization.component.css']
})
export class SpecializationComponent extends AppComponent implements OnInit {


  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  semelar: Specialization;
  tempSpecialization: Specialization = new Specialization();
  errorMessage: string;
  constructor(public service: SpecializationService, public roleservic: RoleService,
    public helperService: HelpService, public spinner: NgxSpinnerService) {
    super(spinner, helperService);
  }

  ngOnInit(): void {
    this.service.special = new Specialization
    this.get();

  }

  GetSimilarForUpdate(special: Specialization) {
    if (special.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(special, this.service.specialall);
  }

  GetSimilarForAdd(special: Specialization) {
    if (special.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(special, this.service.specialall);
  }
  error(): boolean {
    if (this.service.special.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarspecial = this.helperService.GetComplitlySimilarWithAnotherId(this.service.special, this.service.specialall);
    if (similarspecial != null) {
      //this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }
    this.errorMessage = "";
    return false;
  }
  UpdateValidation(special: Specialization): boolean {
    if (special == null || special.Id == '') {
      return false;
    }
    if (special.Name.trim() == "") {
      return false;
    }
    if (special.Name == this.tempSpecialization.Name && special.IsEnabled == this.tempSpecialization.IsEnabled) {
      return false;
    }
    var similarspecial = this.helperService.GetComplitlySimilarWithAnotherId(special, this.service.specialall);
    if (similarspecial != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  
  closeModal() {

    this.service.special.Name = this.tempSpecialization.Name;
    this.service.special.IsEnabled = this.tempSpecialization.IsEnabled;
    this.service.special = new Specialization;
    this.service.special.IsEnabled = true;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  get() {  
    this.showSpinner();
    this.service.getSpecialization().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.specialall = response as Specialization[]);
      this.CheckArrayIsNull(this.service.specialall, "اختصاص ")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
      err => {
        this.hideSpinner();
      });
    this.service.special = new Specialization;
    this.displayedColumns = ['Id', 'Name', 'IsEnabled', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    if (!this.roleservic.CanUpdateSpecialization() || !this.roleservic.CanDeleteSpecialization()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.special.IsEnabled = true
    this.semelar = null
  }

  submit(form: NgForm) {

    this.showSpinner();
    this.service.postSpecialization().subscribe(res => {
      this.hideSpinner();
      this.service.specialall.push(res as Specialization);
      this.dataSource._updateChangeSubscription();
      this.helperService.add(); this.resetForm(form);
      this.service.special = new Specialization();
      this.service.special.IsEnabled = true
      this.semelar = null
      this.CheckArrayIsNull(this.service.specialall, "اختصاص ")
    },
      err => {
        this.hideSpinner();
      })


  }
  edit(form: NgForm) {
    this.errorMessage = " ";
    this.showSpinner();
    this.service.putSpecialization().subscribe(res => {
      this.hideSpinner(); this.resetForm(form);
      this.get();
      this.helperService.edit();

    },
      err => {
        this.hideSpinner();
        this.closeModal();
      })

  }

  fillData(item) {
    this.service.special = item;
    this.tempSpecialization = Object.assign({}, item);

  }

  delete(id) {
    this.showSpinner();
    this.service.deleteSpecialization(id).subscribe(res => {
      this.hideSpinner();
      var special = this.service.specialall.filter(c => c.Id == id)[0];
      var index = this.dataSource.data.indexOf(special);
      this.dataSource.data.splice(index, 1);
      this.dataSource._updateChangeSubscription();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.specialall, "اختصاص ")
    },
      err => {
        this.hideSpinner();
      });
  }

}
