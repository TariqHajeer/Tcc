import { Component, OnInit, ViewChild } from '@angular/core';
import { GroupService } from 'src/app/Service/group.service';
import { PrivlageService } from 'src/app/Service/privlage.service';
import { NgForm } from '@angular/forms';

import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { HelpService } from 'src/app/Help/help.service';
import { Group } from 'src/app/Model/group.model';
import { Privlage } from 'src/app/Model/privlage.model';
import { RoleService } from 'src/app/Help/role.service';
import { element } from 'protractor';
import { type } from 'os';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
  fiteredPriv;
   dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(public service: GroupService, public privilegeservice: PrivlageService,
     public roleservic: RoleService, public helperService: HelpService,public spinner: NgxSpinnerService) {
      super( spinner,helperService); }
  public isCollapsed = false;
  results
  semelar: Group;
  tempGroup: Group;
  errorMessage:string;
  ngOnInit(): void {
    this.displayedColumns = ['Name', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    this.get();
    this.privilegeservice.getPrivlage().subscribe(res => { this.privilegeservice.Privilage = res as Privlage[] });
    this.service.group = new Group;

   if (!this.roleservic.CanDeleteGroup()||!this.roleservic.CanUpdateGroup()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
  }
  
  GetSimilarForUpdate(group:Group){
    if(group.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(group, this.service.groupall);  
  }

  GetSimilarForAdd(group: Group) {
    if(group.Name==""){
      this.semelar=null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(group, this.service.groupall);
  }

  error():boolean {
    if (this.service.group.Name.trim() == '') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarGroup= this.helperService.GetComplitlySimilarWithAnotherId(this.service.group,this.service.groupall);
     if( similarGroup!=null){
     this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
     return true;
     }
    this.errorMessage="";
    return false;
  }
  UpdateValidation(group:Group):boolean{
    if(group==null||group.Id==0){
      return false;
    }
    if(group.Name.trim()==""){
      return false;
    }
    if(group.Name==this.tempGroup.Name){
      return false;
    }
    var similarGroup = this.helperService.GetComplitlySimilarWithAnotherId(group,this.service.groupall);
    if(similarGroup!=null){
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage="";
    this.error();
    this.service.group.Name = this.tempGroup.Name;
    this.service.group = new Group;
  } 
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  get() {
    this.showSpinner();
    this.service.getGroup().subscribe(response => {
this.hideSpinner();
      this.service.groupall = response as Group[];
      // this.service.groupall.forEach(c=>c.Created = new Date(c.Created.getFullYear(),c.Created.getMonth(),c.Created.getDay()));
      // console.log(this.service.groupall);
      this.CheckArrayIsNull(this.service.groupall,"مجموعات ")
      this.dataSource = new MatTableDataSource(this.service.groupall);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },err=>{
      this.hideSpinner();
    });
  }
  submit(form:NgForm) {
this.showSpinner();
    this.service.postGroup().subscribe(res => {
      this.hideSpinner();
     // this.service.groupall.push(res as Group);
     this.get();
      this.dataSource._updateChangeSubscription();
      this.helperService.add();   this.resetForm(form);
      this.service.group = new Group();
      this.CheckArrayIsNull(this.service.groupall,"مجموعات ")
        this.semelar=null
    },
      err => {
         this.hideSpinner();
       
      })

  }
  update(form:NgForm) {
   
this.showSpinner();
      this.service.updateGroup(this.service.group.Id).subscribe(res => {
        this.hideSpinner();
        
        this.resetForm(form);
        this.get();
        this.service.group = new Group;
         this.helperService.edit();
     
        this.semelar=null
      },
        err => {
          this.get();
           this.hideSpinner();
         
          this.service.group.Name = this.tempGroup.Name;
          this.service.group = new Group;
        });
    
  }

  AddPriveleges(group: Group) {
    this.showSpinner();
    this.service.AddPriveleges(group.Id, group.privelegeId)
      .subscribe(res => {
        this.hideSpinner();
        this.helperService.add(); 
        var newPrivelige= this.privilegeservice.Privilage.find(c=>c.Id==group.privelegeId);
        group.Privilages.push(newPrivelige);
        this.filter(group);
        group.PrivilagesCount=group.PrivilagesCount+1;
        
      },
        err => {
           this.hideSpinner();
         
        })
  }
  deletePriveleges(privelege: Privlage, group: Group) {
    // validation
    this.showSpinner();
    this.service.deletePriveleges(group.Id, privelege.Id).subscribe(res => {
      this.hideSpinner();
      this.fiteredPriv.push(privelege);
      this.service.group.Privilages = this.service.group.Privilages.filter(c => c.Id != privelege.Id);
      this.helperService.delete();
      group.PrivilagesCount=group.PrivilagesCount-1;
    },err=>{
      this.hideSpinner()
    })
  }
  fillData(item) {
    this.service.group = item;
    this.tempGroup= Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.deleteGroup(id).subscribe(res => {
      this.hideSpinner();
      this.get();
      this.helperService.delete();
      this.CheckArrayIsNull(this.service.groupall,"مجموعات ")
    },
    err => {
      this.get();
      this.hideSpinner();
    })
  }

  filter(group: Group) {
    this.fiteredPriv = this.privilegeservice.Privilage.filter(({ Name: id1 }) => !group.Privilages.some(({ Name: id2 }) => id2 === id1));
    this.fillData(group);
  }
  
}