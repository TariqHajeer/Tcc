import { Component, OnInit } from '@angular/core';
import { RoleService } from 'src/app/Help/role.service';

@Component({
  selector: 'app-system',
  templateUrl: './system.component.html',
  styleUrls: ['./system.component.css']
})
export class SystemComponent  implements OnInit {

  constructor(public roleservic:RoleService) { }
system:string="الاعدادات"
component=">>"+localStorage.getItem("componentname");
  ngOnInit(): void {
  //  localStorage.removeItem("componentname");
if(localStorage.getItem("componentname")==null||localStorage.getItem("componentname")==undefined)
{this.system="الاعدادات "}
else
    this.system="الاعدادات "+this.component
  }

}
