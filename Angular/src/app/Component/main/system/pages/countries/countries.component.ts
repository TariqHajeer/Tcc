import { Component, OnInit, ViewChild } from '@angular/core';
import { CountriesService } from 'src/app/Service/countries.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { MaincountryService } from 'src/app/Service/maincountry.service';
import { Countries } from 'src/app/Model/countries.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css']
})
export class CountriesComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  semelar: Countries;
  tempCountries: Countries;
  errorMessage: string;
  constructor(public service: CountriesService, public mainservic: MaincountryService,
    public roleservic: RoleService, public helperService: HelpService, public spinner: NgxSpinnerService) {
    super(spinner, helperService);
  }

  ngOnInit(): void {
    this.get();
    this.mainservic.getEnabled();
    this.service.country.IsEnabled = true
    this.semelar = null
    console.log(this.service.country.MainCountry)
  }

  GetSimilarForUpdate(country: Countries) {
    if (country.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(country, this.service.countryall);
  }

  GetSimilarForAdd(country: Countries) {
    if (country.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(country, this.service.countryall);
  }

  error(): boolean {
    if (this.service.country.Name.trim() == ''||this.service.country.MainCountry==null) {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    // var similarCountry = this.helperService.GetComplitlySimilarWithAnotherId(this.service.country, this.service.countryall);
    // if (similarCountry != null&&this.service.country.MainCountry) {
    //   this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
    //  return true;
   // }
   var findcountry=this.service.countryall.find(c=>c.Name==this.service.country.Name&&c.MainCountry==this.service.country.MainCountry)
   if(findcountry!=null||findcountry!=undefined){
    this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
   }else
   { this.errorMessage = "";
    return false;}
  }
  UpdateValidation(country: Countries): boolean {
    if (country == null || country.Id == 0) {
      return false;
    }
    if (country.Name.trim() == "") {
      return false;
    }
    if(country.Name==this.tempCountries.Name&&country.IsEnabled==this.tempCountries.IsEnabled){
      return false;}
    var similarCountry = this.helperService.GetComplitlySimilarWithAnotherId(country, this.service.countryall);
    if (similarCountry != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage = "";
    this.error();
    this.service.country.Name = this.tempCountries.Name;
    this.service.country.IsEnabled = this.tempCountries.IsEnabled;
    this.service.country = new Countries;
    this.service.country.IsEnabled=true;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  get() {
    this.showSpinner();
    this.service.getcountry().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.countryall = response as Countries[]);
      this.CheckArrayIsNull(this.service.countryall,"محافظات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    this.displayedColumns = ['Name', 'IsEnabled', 'Country', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    if (!this.roleservic.CanUpdatecity() || !this.roleservic.CanDeletecity()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.country = new Countries;
  }
  submit(form: NgForm) {
    this.showSpinner();
    this.service.postcountry().subscribe(res => {
      this.get();
      this.hideSpinner();
      this.helperService.add(); this.resetForm(form);
      this.service.country = new Countries;
      this.service.country.IsEnabled = true
      this.semelar = null
      this.CheckArrayIsNull(this.service.countryall,"محافظات")
    },
      err => {
       
        this.hideSpinner();
      })

  }
  edit(form:NgForm) {
    this.errorMessage = " ";
    this.showSpinner();
    this.service.putcountry().subscribe(res => {
      this.get();this.resetForm(form);
      this.hideSpinner();
       this.helperService.edit();
      this.resetForm(form);
      this.service.country.IsEnabled = true
      this.semelar = null
    },
      err => {
        this.hideSpinner();
        this.closeModal();
      })

  }
  fillData(item) {
    this.service.country = item;
    this.tempCountries = Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.deletecountry(id).subscribe(res => {
      this.hideSpinner();
      var country = this.service.countryall.filter(c => c.Id == id)[0];
      var index = this.dataSource.data.indexOf(country);
      this.dataSource.data.splice(index, 1);
      this.dataSource._updateChangeSubscription();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.countryall,"محافظات")
    },
    err=>{
      this.hideSpinner();
     
    });
  }


}
