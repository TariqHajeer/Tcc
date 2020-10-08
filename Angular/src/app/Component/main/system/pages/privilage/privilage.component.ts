import { Component, OnInit ,ViewChild} from '@angular/core';
import { PrivlageService } from 'src/app/Service/privlage.service';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { AuthService } from 'src/app/Service/auth.service';
import { Privlage } from 'src/app/Model/privlage.model';
import { RoleService } from 'src/app/Help/role.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HelpService } from 'src/app/Help/help.service';
@Component({
  selector: 'app-privilage',
  templateUrl: './privilage.component.html',
  styleUrls: ['./privilage.component.css']
})
export class PrivilageComponent extends AppComponent implements OnInit {
  displayedColumns: string[] = ['Description'];
  dataSource 
 
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  constructor(public service:PrivlageService
    ,public roleservic:RoleService
    ,public spinner: NgxSpinnerService,
   public helperservic:HelpService) {
    super( spinner,helperservic);}

  ngOnInit(): void {
    this.showSpinner();
    this.service.getPrivlage().subscribe(response => {
      this.hideSpinner();
      this.dataSource = new MatTableDataSource(this.service.Privilage= response as Privlage[]);
     
      this.dataSource.paginator = this.paginator;
    });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
 
}
 