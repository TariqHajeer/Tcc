import { filter } from 'rxjs/operators';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import { Component, ViewChildren, OnChanges, Input } from '@angular/core';
import { NgxSpinnerService } from "ngx-spinner";
import { Router, ActivatedRoute, NavigationStart,NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { HelpService } from './Help/help.service';
import { NgForm } from '@angular/forms';
import { InputRangeDirective } from './Custom/Directives/input-range.directive';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export  class AppComponent implements OnChanges {
  title = 'TCC';
 // @ViewChildren(InputRangeDirective) inputRange;
  constructor(public spinner: NgxSpinnerService,
public helperService:HelpService
   ) {}
  
  testcount=1;
 DisabledToster=[]
  ngOnChanges(){
this.testcount
  }
  ngOnInit(): void {
  
    
  }
 
  showSpinner(){
    this.spinner.show();
   /* setTimeout(() => {
      this.spinner.hide();
    }, 5000);*/
  }
  hideSpinner(){
    this.spinner.hide();
  }
resetForm(form:NgForm){
  form.form.markAsPristine();
  form.form.markAsUntouched();
  form.form.updateValueAndValidity();
}
NoDataFound
HiddenMessage=false
CheckArrayIsNull(array,name){
  this.NoDataFound= this.helperService.CheckArrayIsNull(array,name)
  if(this.NoDataFound=="")
  this.HiddenMessage=false
  else
  this.HiddenMessage=true
}
//filter name and isEnabled in all component
/*errorMessage
semelar
GetSimilarForUpdate(object,arrayofobject) {
  if (object.Name == "") {
    this.semelar = null;
    return;
  }
  this.semelar = this.helperService.GetSimilarWithAnotherId(object,arrayofobject);
}

GetSimilarForAdd(object,arrayofobject) {
  if (object.Name == "") {
    this.semelar = null;
    return;
  }
  this.semelar = this.helperService.GetSimilar(object, arrayofobject);
}
error(object,arrayofobject): boolean {
  if (object.Name.trim() == '') {
    // this.errorMessage="يجب تعبئة الأسم";
    return true;
  }
  var similarobject = this.helperService.GetComplitlySimilarWithAnotherId(object, arrayofobject);
  if (similarobject != null) {
    //this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
    return true;
  }
  this.errorMessage = "";
  return false;
}
UpdateValidation(object,arrayofobject,temp): boolean {
  if (object == null || object.Id == 0) {
    return false;
  }
  if (object.Name.trim() == "") {
    return false;
  }
  if(object.Name==temp.Name&&object.IsEnabled==temp.IsEnabled){
    return false;
  }
  var similarobject = this.helperService.GetComplitlySimilarWithAnotherId(object, arrayofobject);
  if (similarobject != null) {
    this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
    return false;
  }
  return true;
}
*/
}
