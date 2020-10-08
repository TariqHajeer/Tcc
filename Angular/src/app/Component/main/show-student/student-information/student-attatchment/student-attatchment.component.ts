import { Component, OnInit, ViewChild } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { StudentAttachment } from 'src/app/Model/student-attachment.model';

@Component({
  selector: 'app-student-attatchment',
  templateUrl: './student-attatchment.component.html',
  styleUrls: ['./student-attatchment.component.css']
})
export class StudentAttatchmentComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }
  AddStudentAttachment: StudentAttachment;
  ngOnInit(): void {
    this.AddStudentAttachment = new StudentAttachment;
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.GetStudentAttachment, "مرفقات")
this.StudentInformation.GetStudentAttachment.forEach(item=>{

})
  }
  AddStudentAttachments() {

    this.AddStudentAttachment.AttachmentId = this.AddStudentAttachment.getAttachment.Id;

    this.StudentInformation.GetStudentAttachment.push(this.AddStudentAttachment as StudentAttachment);
    this.AddStudentAttachment = new StudentAttachment;
  }
  DeleteStudentAttachments(item) {
    this.StudentInformation.GetStudentAttachment = this.StudentInformation.GetStudentAttachment.filter(c => c != item);
  }

  @ViewChild('fileInput') fileInput;
  public stageFile(): void {
    this.AddStudentAttachment.Attachemnt = this.fileInput.nativeElement.files[0];
  }
}
