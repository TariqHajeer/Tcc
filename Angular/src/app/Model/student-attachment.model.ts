import { Attachment } from './attachment.model';

export class StudentAttachment {
    //add
    Ssn:string;
    AttachmentId:number;
    Attachemnt:File;
    Note:string;

    //get
    getAttachment:Attachment;
}
