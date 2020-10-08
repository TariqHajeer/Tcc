import { AbstractControl, ValidatorFn, FormControl } from '@angular/forms';
import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator } from '@angular/forms';

export class NumberValidators {

  static isNumberCheck(): ValidatorFn {
    return  (c: AbstractControl): {[key: string]: boolean} | null => {
      let number = /^[.\d]+$/.test(c.value) ? +c.value : NaN;
      if (number !== number) {
        return { 'value': true };
      }

      return null;
    };
    
  }
test(){
    var inputBox = document.getElementById("inputBox");

var invalidChars = [
  "-",
  "+",
  "e",
  "="
];

inputBox.addEventListener("keydown", function(e) {
  if (invalidChars.includes(e.key)) {
    e.preventDefault();
  }
});
}
  
  
}


//min value
@Directive({
  selector: '[customMin][formControlName],[customMin][formControl],[customMin][ngModel]',
  providers: [{provide: NG_VALIDATORS, useExisting: CustomMinDirective, multi: true}]
})
export class CustomMinDirective implements Validator {
  @Input()
  customMin: number;
  
  validate(c: FormControl): {[key: string]: any} {
      let v = c.value;
      return ( v < this.customMin)? {"customMin": true} : null;
  }
}
//max value
@Directive({
    selector: '[customMax][formControlName],[customMax][formControl],[customMax][ngModel]',
    providers: [{provide: NG_VALIDATORS, useExisting: CustomMaxDirective, multi: true}]
  })
  export class CustomMaxDirective implements Validator {
    @Input()
    customMax: number;
    
    validate(c: FormControl): {[key: string]: any} {
        let v = c.value;
        return ( v > this.customMax)? {"customMax": true} : null;
    }
  }