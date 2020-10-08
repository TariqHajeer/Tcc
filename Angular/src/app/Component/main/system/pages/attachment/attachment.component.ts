import { Component, OnInit, ViewChild } from '@angular/core';


import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AttachmentService } from 'src/app/Service/attachment.service';
import { Attachment } from 'src/app/Model/attachment.model';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { Degree } from 'src/app/Model/degree.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppComponent } from 'src/app/app.component';
import { NgForm } from '@angular/forms';
import { FocusMonitor } from '@angular/cdk/a11y';
import { from } from 'rxjs';

@Component({
  selector: 'app-attachment',
  templateUrl: './attachment.component.html',
  styleUrls: ['./attachment.component.css']
})
export class AttachmentComponent extends AppComponent implements OnInit {
  displayedColumns: string[];
  dataSource
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  semelar: Attachment;
  tempAttatchment: Attachment;
  errorMessage: string;

  constructor(public service: AttachmentService, public roleservic: RoleService,
    public helperService: HelpService,
    public spinner: NgxSpinnerService) {
    super(spinner, helperService);
    this.service.attachment = new Attachment;

  }

  ngOnInit(): void {
    this.get();
    this.service.attachment = new Attachment;
    this.service.attachment.IsEnabled = true

  }


  GetSimilarForUpdate(attachment: Attachment) {
    if (attachment.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilarWithAnotherId(attachment, this.service.attachmentall);
  }

  GetSimilarForAdd(attachment: Attachment) {

    if (attachment.Name == "") {
      this.semelar = null;
      return;
    }
    this.semelar = this.helperService.GetSimilar(attachment, this.service.attachmentall);
  }

  error(): boolean {
    if (this.service.attachment.Name.trim() == ' ') {
      // this.errorMessage="يجب تعبئة الأسم";
      return true;
    }
    var similarAttatchment = this.helperService.GetComplitlySimilarWithAnotherId(this.service.attachment, this.service.attachmentall);
    if (similarAttatchment != null) {
      this.errorMessage="يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return true;
    }
    this.errorMessage = "";
    return false;
  }
  UpdateValidation(attachment: Attachment): boolean {
    if (attachment == null || attachment.Id == 0) {
      return false;
    }
    if (attachment.Name.trim() == "") {
      return false;
    }
    if(attachment.Name==this.tempAttatchment.Name&&attachment.IsEnabled==this.tempAttatchment.IsEnabled){
      return false;
    }
    var similarAttatchment = this.helperService.GetComplitlySimilarWithAnotherId(attachment, this.service.attachmentall);
    if (similarAttatchment != null) {
      this.errorMessage = "يجب ان يكون الأسم مختلف على الأقل بحرف واحد";
      return false;
    }
    return true;
  }
  closeModal() {
    this.errorMessage = "";
    this.error();
    this.service.attachment.Name = this.tempAttatchment.Name;
    this.service.attachment.IsEnabled = this.tempAttatchment.IsEnabled;
    this.service.attachment = new Attachment;
    this.semelar = null
    this.service.attachment.IsEnabled = true
  }



  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  get() {
    this.showSpinner();
    this.service.getAttachment().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.attachmentall = response as Attachment[]);
      this.CheckArrayIsNull(this.service.attachmentall,"أنواع مرفقات")
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }, err => {
      this.hideSpinner();
    }); this.service.attachment = new Attachment;
    this.displayedColumns = ['Name', 'IsEnabled', 'Created', 'CreatedBy', 'Modified', 'ModifiedBy', 'More'];
    if (!this.roleservic.CanUpdateAttachment() || !this.roleservic.CanDeleteAttachment()) {
      this.displayedColumns = this.displayedColumns.filter(x => x != "More")
    }
  }
  submit(form: NgForm) {
    this.showSpinner();
    this.service.postAttachment().subscribe(res => {
      this.hideSpinner();
      this.service.attachmentall.push(res as Attachment);
      this.dataSource._updateChangeSubscription();
     this.resetForm(form);
      this.service.attachment = new Attachment();
      this.semelar = null
      this.service.attachment.IsEnabled = true;
      this.CheckArrayIsNull(this.service.attachmentall,"أنواع مرفقات")
      this.helperService.add();
    },
      err => {
        this.hideSpinner();
      })
  }
  edit(form:NgForm) {
    this.errorMessage = " ";
    this.showSpinner();
    this.service.putAttachment().subscribe(res => {
      this.hideSpinner();
      this.get();
      this.helperService.edit();
      this.resetForm(form);
      this.semelar = null
      this.service.attachment.IsEnabled = true
    },
      err => {
        this.hideSpinner();
        this.closeModal();
      })

  }

  fillData(item) {
    this.service.attachment = item;
    this.tempAttatchment = Object.assign({}, item);
  }

  delete(id) {
    this.showSpinner();
    this.service.deleteAttachment(id).subscribe(res => {
      this.hideSpinner();
      var attachment = this.service.attachmentall.filter(c => c.Id == id)[0];
      var index = this.dataSource.data.indexOf(attachment);
      this.dataSource.data.splice(index, 1);
      this.dataSource._updateChangeSubscription();
      this.CheckArrayIsNull(this.service.attachmentall,"أنواع مرفقات")
      this.helperService.delete();
    }, err => {
      this.hideSpinner();
    });

  }

}
