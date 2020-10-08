import { Component, OnInit,ViewChild } from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UserService } from 'src/app/Service/user.service';
import { GroupService } from 'src/app/Service/group.service';
import { Group } from 'src/app/Model/group.model';
import { User } from 'src/app/Model/user.model';
import { RoleService } from 'src/app/Help/role.service';
import { HelpService } from 'src/app/Help/help.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent extends AppComponent implements OnInit {
 title:string;
  displayedColumns: string[];
    dataSource 
  fiteredPriv;
  semelar: User;
  temp: User;
  errorMessage:string;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  constructor(public service:UserService,public groupservice:GroupService,
    public roleservic:RoleService,public helperService: HelpService,public spinner: NgxSpinnerService
    ,   public router: Router,
    public activatedRoute: ActivatedRoute,
    public titleService: Title) {
      super( spinner,helperService); }
  public isCollapsed = false;
  ngOnInit(): void {
    this.get();
    this.groupservice.getGroup().subscribe(res=>{this.groupservice.groupall=res as Group[]});
    this.title="المستخدمون"
    localStorage.setItem("componentname",this.title);
   //get route title
//    this.router.events
//    .filter((event) => event instanceof NavigationEnd)
//    .map(() => this.activatedRoute)
//    .map((route) => {
//      while (route.firstChild) route = route.firstChild;
//      return route;
//    })
//    .filter((route) => route.outlet === 'primary')
//    .mergeMap((route) => route.data)
//    .subscribe((event) => {this.titleService.setTitle(event['title'])
//  console.log(event['title'])});
  }
  GetSimilarForUpdate(user:User){
    if(user.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(user, this.service.userall);  
  }
  errorUserName:User
  errorMessageUserName=""
  CheckUserName(user:User){
    if(user.Username==""){
      this.errorUserName=null;
      return;
    }
    this.errorUserName = this.service.userall.find(c=>c.Username==user.Username);
  }
  GetSimilarForAdd(user: User) {
    if(user.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(user, this.service.userall);
  }

  error():boolean {
    if (this.service.user.Name.trim() == ''||this.service.user.Username==''||this.service.user.Password==''
    ||this.service.user.PasswordVerification=='') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similaruser= this.helperService.GetComplitlySimilarWithAnotherId(this.service.user,this.service.userall);
    
     if(this.errorUserName!=null){
      this.errorMessageUserName="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
      }
       if(this.errorUserName!=null){
        this.errorMessageUserName="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
        return true;
        }
    this.errorMessage="";
    this.errorMessageUserName=""
    return false;
  }
  UpdateValidation(user:User):boolean{
    if(user==null||user.Id==0){
      return false;
    }
    if(user.Name.trim()==""){
      return false;
    }
    if(user.Name==this.temp.Name&&user.IsEnabled==this.temp.IsEnabled){
      return false;
    }
    var similaruser = this.helperService.GetComplitlySimilarWithAnotherId(user,this.service.userall);
    if(similaruser!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal(form:NgForm) {
    this.errorMessage=" ";
    this.error();
    this.service.user.Name = this.temp.Name;
    this.service.user.IsEnabled = this.temp.IsEnabled;
    this.resetForm(form);
    this.service.user = new User;
    this.service.user.IsEnabled = true
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  get()
  {  this.service.user = new User();
    this.showSpinner();
    this.service.getUser().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.userall= response as User[] );
      this.CheckArrayIsNull(this.service.userall,"مستخدمون")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      
    },
    err => {
      this.hideSpinner();
    });
   
    this.displayedColumns = ['Name','Username', 'IsEnabled','Created', 'CreatedBy','Modified','ModifiedBy','More'];
    if (!this.roleservic.CanUpdateUser() || !this.roleservic.CanDeleteUser()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
   
    }
    this.service.user.IsEnabled=true
    this.semelar=null
  }
  submit(form:NgForm) {
this.showSpinner();
    this.service.postUser().subscribe(res => {
      this.hideSpinner();
    //  this.service.userall.push(res as User);
    this.get()
      this.dataSource._updateChangeSubscription();
      this.CheckArrayIsNull(this.service.userall,"مستخدمون")
      this.helperService.add();   this.resetForm(form);
      this.service.user = new User();
    this.PasswordMessage=" "
    this.service.user.IsEnabled=true
    this.semelar=null

    },
      err => {
         this.hideSpinner();
       
        this.hideSpinner();
      })

  }
 

  AddGroup(user: User) {
    this.showSpinner();
    this.service.Addgroup(user.Id, user.groupid)
      .subscribe(res => {
        this.hideSpinner();
        this.helperService.add();  
        var newPrivelige= this.groupservice.groupall.find(c=>c.Id==user.groupid);
        user.Group.push(newPrivelige);
        this.filter(user);
        user.GroupCount=user.GroupCount+1;
        
      },
        err => {
           this.hideSpinner();
         
        })
  }
  deleteGroup(group: Group, user: User) {
    // validation
    this.showSpinner();
    this.service.deletegroup(user.Id, group.Id).subscribe(res => {
      this.hideSpinner();
      this.fiteredPriv.push(group);
      this.service.user.Group = this.service.user.Group.filter(c => c.Id != group.Id);
      this.helperService.delete();
      user.GroupCount=user.GroupCount-1;
    },err=>{
      this.hideSpinner();
    })
  }
  UpdateUser(form:NgForm){
    this.showSpinner();
    this.service.PutUser().subscribe(res=>{
      this.hideSpinner();
      this.get(); this.resetForm(form);
       this.helperService.edit();
      this.resetForm(form);
      this.closeModal(form);
      this.errorMessage=" ";

    },
    err=>{
       this.hideSpinner();
      this.hideSpinner();
    })
  }
  delete(id){
    this.showSpinner();
    this.service.DeleteUser(id).subscribe(res=>{
      this.hideSpinner();
      var typeofregister = this.service.userall.filter(c=>c.Id==id)[0];    
      var index= this.dataSource.data.indexOf(typeofregister);
      this.dataSource.data.splice(index,1);
      this.dataSource._updateChangeSubscription();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.userall,"مستخدمون")
      },err=>{
        this.hideSpinner();
      });

  }
  fillData(item) {
    this.service.user = item;
     this.temp= Object.assign({}, item);
  }

  filter(user: User) {
    this.fiteredPriv = this.groupservice.groupall.filter(({ Name: id1 }) => !user.Group.some(({ Name: id2 }) => id2 === id1));
    this.fillData(user);
  }
  PasswordValidation:boolean=null;
  PasswordMessage:string="";
  PasswordVerifications(){
    this.PasswordMessage="";
    if(this.service.user.Password!=this.service.user.PasswordVerification){
      this.PasswordMessage="كلمة السر غير متطابقة";
this.PasswordValidation=true;
    }else{
    this.PasswordValidation=false;
    this.PasswordMessage="كلمة السر  متطابقة";
    }
  }
  PasswordLength:boolean=null
  PasswordLengthMessage:string=""
  changePassword(){
    if(this.service.user.Password.length<6){
this.PasswordLength=true;
this.PasswordLengthMessage="لايمكن ان تكون كلمة السر اقل من 6 محارف"

    }
    else{
      this.PasswordLength=false;
      this.PasswordLengthMessage=""
    }
  }
  } 

 

