import { Component, OnInit, HostListener, Input, ElementRef } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private ref: ElementRef) { }

  ngOnInit(): void {
  }
  // @Input('maxValue') maxValue;
  // @Input('minValue') minValue;
  // @HostListener('keypress', ['$event']) onKeydownHandler(evt: KeyboardEvent) {
  //   // var newNumber = String.fromCharCode(evt.charCode);
  //   var charCode = (evt.which) ? evt.which : evt.keyCode;
  //   if (charCode > 31 && (charCode < 48 || charCode > 57)) {
  //     return false;
  //   }
  //   var newNumber = String.fromCharCode(evt.charCode);
  //   var number = Number(this.ref.nativeElement.value + newNumber);
  //   if (number > Number(this.maxValue)) {
  //     return false;
  //   }
  //   if(this.ref.nativeElement.value=="0"){
  //     this.ref.nativeElement.value="";
  //   }
  // }
  // @HostListener('keyup', ['$event']) onKeyUpHandler(evt: KeyboardEvent) {
  //   if (this.minValue != undefined) {
  //     if (evt.keyCode == 8) {
        
  //       if (this.ref.nativeElement.value==""||(Number( this.ref.nativeElement.value) < this.minValue)){
  //         this.ref.nativeElement.value=this.minValue;
  //       }
  //     }
  //   }
  // }
}
