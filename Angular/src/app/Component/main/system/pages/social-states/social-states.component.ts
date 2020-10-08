import { Component, OnInit,ViewChild } from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { SocialStatesService } from 'src/app/Service/social-states.service';
import { SocialStates } from 'src/app/Model/social-states.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgForm } from '@angular/forms';
import { iif } from 'rxjs';
@Component({
  selector: 'app-social-states',
  templateUrl: './social-states.component.html',
  styleUrls: ['./social-states.component.css']
})
export class SocialStatesComponent extends AppComponent implements OnInit {


  displayedColumns: string[] ;
  dataSource 
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  semelar: SocialStates;
  tempsocialstate: SocialStates;
  errorMessage:string;
  constructor(public service:SocialStatesService,public roleservic:RoleService,
    public helperService:HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }

  ngOnInit(): void {
   this.get();
   this.service.socialstate.IsEnabled=true
   this.semelar=null
  }
  GetSimilarForUpdate(socialstate:SocialStates){
    if(socialstate.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(socialstate, this.service.socialstateall);  
  }

  GetSimilarForAdd(socialstate: SocialStates) {
    if(socialstate.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(socialstate, this.service.socialstateall);
  }

  error():boolean {
    if (this.service.socialstate.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarSocialState= this.helperService.GetComplitlySimilarWithAnotherId(this.service.socialstate,this.service.socialstateall);
     if( similarSocialState!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(socialstate:SocialStates):boolean{
    if(socialstate==null||socialstate.Id==0){
      return false;
    }
    if(socialstate.Name.trim()==""){
      return false;
    }
    if(socialstate.Name==this.tempsocialstate.Name&&socialstate.IsEnabled==this.tempsocialstate.IsEnabled){
      return false;
    }
    var similarSocialState = this.helperService.GetComplitlySimilarWithAnotherId(socialstate,this.service.socialstateall);
    if(similarSocialState!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.socialstate.Name = this.tempsocialstate.Name;
    this.service.socialstate.IsEnabled = this.tempsocialstate.IsEnabled;
    this.service.socialstate = new SocialStates;
    this.service.socialstate.IsEnabled = true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
 
  get()
  {
    this.showSpinner();
    this.service.getSocialStates().subscribe(response => {
      this.hideSpinner();
      this.service.socialstateall= response as SocialStates[]
      this.CheckArrayIsNull(this.service.socialstateall,"حالة اجتماعية");
    this.dataSource = new MatTableDataSource(this.service.socialstateall);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },
    err => {
      this.hideSpinner();
    });
    this.displayedColumns= ['Name', 'IsEnabled', 'Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateSocialState() || !this.roleservic.CanDeleteSocialState()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
    this.service.socialstate=new SocialStates;
  }
  
  submit(form:NgForm){
  this.showSpinner();
      this.service.postSocialStates().subscribe(res=>{
        this.hideSpinner();
        this.service.socialstateall.push(res as SocialStates);
        this.CheckArrayIsNull(this.service.socialstateall,"حالة اجتماعية");
        this.dataSource._updateChangeSubscription();
        this.helperService.add();   this.resetForm(form);
        this.service.socialstate = new SocialStates();
      this.service.socialstate.IsEnabled=true
   this.semelar=null
      },
      err=>{
         this.hideSpinner();
      })
   
    }
   edit(form:NgForm) {
    this.errorMessage=" ";
      this.showSpinner();
        this.service.putSocialStates().subscribe(res=>{
          this.hideSpinner();
          this.get();
           this.helperService.edit();
      this.resetForm(form);
          this.service.socialstate.IsEnabled=true
   this.semelar=null

        },
        err=>{
           this.hideSpinner();
          this.closeModal();
        })
      
    }

    
  fillData(item){
    this.service.socialstate=item;
    this.tempsocialstate= Object.assign({}, item);
  }

  delete(id){
    this.showSpinner();
    this.service.deleteSocialStates(id).subscribe(res=>{
      this.hideSpinner();
    var socialstate = this.service.socialstateall.filter(c=>c.Id==id)[0];    
    var index= this.dataSource.data.indexOf(socialstate);
    this.dataSource.data.splice(index,1);
    this.dataSource._updateChangeSubscription();
       this.helperService.delete();
       this.CheckArrayIsNull(this.service.socialstateall,"حالة اجتماعية");
    },err=>{
      this.hideSpinner();
    });
  }

}
