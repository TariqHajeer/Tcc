import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IName } from '../Interfaces/IName';
import { IModel } from '../interfaces/IModel';
import { IId } from '../interfaces/IId';
@Injectable({
  providedIn: 'root'
})
export class HelpService {
  url: string = "https://localhost:5001/api/";
  constructor(public toastr: ToastrService) { }

  add() {
    this.toastr.success("تمت الاضافة بنجاح", '');
  }
  edit() {
    this.toastr.success("تم التعديل بنجاح", '');
  }
  delete() {
    this.toastr.success("تم الحذف بنجاح", '');
  }
  CheckArrayIsNull(array: any[], name): string {
    if (array.length == 0)
      return "لايوجد" +" "+ name
      else 
      return ""
  }
  GetSimilar<T extends IName>(object: T, dataSet: T[]): T {
    var objects = dataSet.filter(c => c.Name.trim().startsWith(object.Name.trim()));
    if (objects.length == 0)
      return null;
    return objects[0];
  }
  GetSimilarWithAnotherId<T extends IModel,>(object: T, dataSet: T[]): T {
    var objects = dataSet.filter(c => c.Id != object.Id && c.Name.trim().startsWith(object.Name.trim()));
    if (objects.length == 0)
      return null;
    return objects[0];
  }
  GetComplitlySimilar<T extends IModel,>(object: T, dataSet: T[]): T {
    var objects = dataSet.filter(c => c.Name.trim() == object.Name.trim());
    if (objects.length == 0)
      return null;
    return objects[0];
  }
  GetComplitlySimilarWithAnotherId<T extends IModel,>(object: T, dataSet: T[]): T {
    ////debugger;
    var objects = dataSet.filter(c => c.Id != object.Id && c.Name.trim() == object.Name.trim());
    if (objects.length == 0)
      return null;
    return objects[0];
  }
  FilterObjectDependeOnId<T extends IId>(dataSet: T[]): T[] {
    return dataSet.filter((object, index, arr) => arr.findIndex(t => t.Id === object.Id) == index);
  }
  DifferenceTowArrayById<T extends IId>(array1: T[], array2: T[]): T[] {
    if (array2 == null || array2.length == 0) {
      return array1;
    }
    var result = array1.filter(({ Id: Id1 }) => !array2.some(({ Id: Id2 }) => Id1 === Id2));
    return result;
  }
}