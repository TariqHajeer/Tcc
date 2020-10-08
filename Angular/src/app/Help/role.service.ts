import { Injectable } from '@angular/core';
import { AuthService } from '../Service/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(public authService:AuthService) { }
  checkRole(role:string):boolean
  {
   // return true;
  return this.authService.auth.Roles.indexOf(role)>-1;
  }
 //#region Degree
  CanAddDegree():boolean
  {
  return this.authService.auth.Roles.indexOf("AddDegree")>-1;
  }
  CanShowDegree():boolean
  {
  return this.authService.auth.Roles.indexOf("ShowDegree")>-1;
  }
  CanUpdateDegree():boolean
  {
  return this.authService.auth.Roles.indexOf("UpdateDegree")>-1;
  }
  CanDeleteDegree():boolean
  {
  return this.authService.auth.Roles.indexOf("RemoveDegree")>-1;
  }
  //#endregion
//#region Attachment
CanAddAttachment():boolean
{
return this.authService.auth.Roles.indexOf("AddAttachment")>-1;
}
CanShowAttachment():boolean
{
return this.authService.auth.Roles.indexOf("ShowAttachment")>-1;
}
CanUpdateAttachment():boolean
{
return this.authService.auth.Roles.indexOf("UpdateAttachment")>-1;
}
CanDeleteAttachment():boolean
{ 
return this.authService.auth.Roles.indexOf("RemoveAttachment")>-1;
}
//#endregion
//#region College
CanAddCollege():boolean
{
return this.authService.auth.Roles.indexOf("AddCollege")>-1;
}
CanShowCollege():boolean
{
return this.authService.auth.Roles.indexOf("ShowColleges")>-1;
}
CanUpdateCollege():boolean
{
return this.authService.auth.Roles.indexOf("UpdateCollege")>-1;
}
CanDeleteCollege():boolean
{
return this.authService.auth.Roles.indexOf("RemoveCollege")>-1;
}
//#endregion
//#region Constraint
CanAddConstraint():boolean
{
return this.authService.auth.Roles.indexOf("AddConstraint")>-1;
}
CanShowConstraint():boolean
{
return this.authService.auth.Roles.indexOf("ShowConstraint")>-1;
}
CanUpdateConstraint():boolean
{
return this.authService.auth.Roles.indexOf("UpdateConstraint")>-1;
}
CanDeleteConstraint():boolean
{
return this.authService.auth.Roles.indexOf("RemoveConstraint")>-1;
}
//#endregion
//#region city
CanAddcity():boolean
{
return this.authService.auth.Roles.indexOf("AddCity")>-1;
}
CanShowcity():boolean
{
return this.authService.auth.Roles.indexOf("ShowCities")>-1;
}
CanUpdatecity():boolean
{
return this.authService.auth.Roles.indexOf("UpdateCity")>-1;
}
CanDeletecity():boolean
{
return this.authService.auth.Roles.indexOf("RemoveCity")>-1;
}
//#endregion
//#region Group
CanAddGroup():boolean
{
return this.authService.auth.Roles.indexOf("CreateGroup")>-1;
}
CanShowGroup():boolean
{
return this.authService.auth.Roles.indexOf("ShowGroup")>-1;
}
CanUpdateGroup():boolean
{
return this.authService.auth.Roles.indexOf("UpdateGroup")>-1;
}
CanDeleteGroup():boolean
{
return this.authService.auth.Roles.indexOf("DeleteGroup")>-1;
}
//#endregion
//#region Honesty
CanAddHonesty():boolean
{
return this.authService.auth.Roles.indexOf("AddHonesty")>-1;
}
CanShowHonesty():boolean
{
return this.authService.auth.Roles.indexOf("ShowHonesty")>-1;
}
CanUpdateHonesty():boolean
{
return this.authService.auth.Roles.indexOf("UpdateHonesty")>-1;
}
CanDeleteHonesty():boolean
{
return this.authService.auth.Roles.indexOf("RemoveHonesty")>-1;
}
//#endregion
//#region Languages
CanAddLanguages():boolean
{
return this.authService.auth.Roles.indexOf("AddLanguage")>-1;
}
CanShowLanguages():boolean
{
return this.authService.auth.Roles.indexOf("ShowLanguages")>-1;
}
CanUpdateLanguages():boolean
{
return this.authService.auth.Roles.indexOf("UpdateLanguage")>-1;
}
CanDeleteLanguages():boolean
{
return this.authService.auth.Roles.indexOf("RemoveLanguage")>-1;
}
//#endregion
//#region Country
CanAddCountry():boolean
{
return this.authService.auth.Roles.indexOf("AddCountry")>-1;
}
CanShowCountry():boolean
{
return this.authService.auth.Roles.indexOf("ShowCountries")>-1;
}
CanUpdateCountry():boolean
{
return this.authService.auth.Roles.indexOf("UpdateCountry")>-1;
}
CanDeleteCountry():boolean
{
return this.authService.auth.Roles.indexOf("RemoveCountry")>-1;
}
//#endregion
//#region Nationality
CanAddNationality():boolean
{
return this.authService.auth.Roles.indexOf("AddNationality")>-1;
}
CanShowNationality():boolean
{
return this.authService.auth.Roles.indexOf("ShowNationality")>-1;
}
CanUpdateNationality():boolean
{
return this.authService.auth.Roles.indexOf("UpdateNationality")>-1;
}
CanDeleteNationality():boolean
{
return this.authService.auth.Roles.indexOf("RemoveNationality")>-1;
}
//#endregion
//#region PhoneType
CanAddPhoneType():boolean
{
return this.authService.auth.Roles.indexOf("AddPhoneType")>-1;
}
CanShowPhoneType():boolean
{
return this.authService.auth.Roles.indexOf("ShowPhoneType")>-1;
}
CanUpdatePhoneType():boolean
{
return this.authService.auth.Roles.indexOf("UpdatePhoneType")>-1;
}
CanDeletePhoneType():boolean
{
return this.authService.auth.Roles.indexOf("RemovePhoneType")>-1;
}
//#endregion
//#region Privilage

CanShowPrivilage():boolean
{
return this.authService.auth.Roles.indexOf("ShowPrivilage")>-1;
}

//#endregion
//#region SocialState
CanAddSocialState():boolean
{
return this.authService.auth.Roles.indexOf("AddSocialStates")>-1;
}
CanShowSocialState():boolean
{
return this.authService.auth.Roles.indexOf("ShowSocialStates")>-1;
}
CanUpdateSocialState():boolean
{
return this.authService.auth.Roles.indexOf("UpdateSocialStates")>-1;
}
CanDeleteSocialState():boolean
{
return this.authService.auth.Roles.indexOf("RemoveSocialStates")>-1;
}
//#endregion
//#region Specialization
CanAddSpecialization():boolean
{
return this.authService.auth.Roles.indexOf("AddSpecialization")>-1;
}
CanShowSpecialization():boolean
{
return this.authService.auth.Roles.indexOf("ShowSpecialization")>-1;
}
CanUpdateSpecialization():boolean
{
return this.authService.auth.Roles.indexOf("UpdateSpecialization")>-1;
}
CanDeleteSpecialization():boolean
{
return this.authService.auth.Roles.indexOf("RemoveSpecializations")>-1;
}
//#endregion
//#region AddTypeOfRegistar
CanAddTypeOfRegistar():boolean
{
return this.authService.auth.Roles.indexOf("AddTypeOfRegistar")>-1;
}
CanShowTypeOfRegistar():boolean
{
return this.authService.auth.Roles.indexOf("ShowTypeOfRegistar")>-1;
}
CanUpdateTypeOfRegistar():boolean
{
return this.authService.auth.Roles.indexOf("UpdateTypeOfRegistar")>-1;
}
CanDeleteTypeOfRegistar():boolean
{
return this.authService.auth.Roles.indexOf("RemoveTypeOfRegistar")>-1;
}
//#endregion
//#region User
CanAddUser():boolean
{
return this.authService.auth.Roles.indexOf("AddUser")>-1;
}
CanShowUser():boolean
{
return this.authService.auth.Roles.indexOf("ShowUser")>-1;
}
CanUpdateUser():boolean
{
return this.authService.auth.Roles.indexOf("UpdateUser")>-1;
}
CanDeleteUser():boolean
{
return this.authService.auth.Roles.indexOf("RemoveUser")>-1;
}
//#endregion

//#region Year
CanAddYear():boolean
{
return this.authService.auth.Roles.indexOf("AddYear")>-1;
//return true;
}
CanShowYear():boolean
{
return this.authService.auth.Roles.indexOf("ShowYears")>-1;
//return true;
}
CanUpdateYear():boolean
{
//return this.authService.auth.Roles.indexOf("")>-1;
return true;
}
CanDeleteYear():boolean
{
//return this.authService.auth.Roles.indexOf("")>-1;
return true;
}
//#endregion  

//#region YearSystem
CanAddYearSystem():boolean
{
return this.authService.auth.Roles.indexOf("AddYearSystem")>-1;
return true;
}
CanShowYearSystem():boolean
{
return this.authService.auth.Roles.indexOf("ShowYearSystem")>-1;
return true;
}
CanUpdateYearSystem():boolean
{
//return this.authService.auth.Roles.indexOf("UpdateYeaeSystem")>-1;
return true;
}
CanDeleteYearSystem():boolean
{
//return this.authService.auth.Roles.indexOf("RemoveYearSystem")>-1;
return true;
}
//#endregion  

//#region SubjectType
CanAddSubjectType():boolean
{
return this.authService.auth.Roles.indexOf("AddSubjectType")>-1;
//return true;
}
CanShowSubjectType():boolean
{
return this.authService.auth.Roles.indexOf("ShowSubjectType")>-1;
//return true;
}
CanUpdateSubjectType():boolean
{ 
return this.authService.auth.Roles.indexOf("UpdateSubjectType")>-1;
//return true;
}
CanDeleteSubjectType():boolean
{
return this.authService.auth.Roles.indexOf("RemoveSubjectType")>-1;
//return true;
}
//#endregion 

//#region Student
CanAddStudent():boolean
{
return this.authService.auth.Roles.indexOf("AddStudent")>-1;
}
CanShowStudent():boolean
{
return this.authService.auth.Roles.indexOf("ShowStudent")>-1;
}
CanUpdateStudent():boolean
{
return this.authService.auth.Roles.indexOf("UpdateStudent")>-1;
//return true;
}
CanDeleteStudent():boolean
{
return this.authService.auth.Roles.indexOf("RemoveStudent")>-1;
//return true;
}
CanUploadImage():boolean{
  return this.authService.auth.Roles.indexOf("UploadImage")>-1;
}
GetStudentPracticalDegree():boolean{
  return this.authService.auth.Roles.indexOf("GetStudentPracticalDegree")>-1;
}
SetStudentsDegreeBySubject():boolean{
  return this.authService.auth.Roles.indexOf("SetStudentsDegreeBySubject")>-1;
}
CanReset():boolean{
  return this.authService.auth.Roles.indexOf("CanReset")>-1;
}
Reset():boolean{
  return this.authService.auth.Roles.indexOf("Reset")>-1;
}
SetDegreeForTransformStudent():boolean{
  return this.authService.auth.Roles.indexOf("SetDegreeForTransformStudent")>-1;
}
StudentPreviousYearSetting():boolean{
  return this.authService.auth.Roles.indexOf("StudentPreviousYearSetting")>-1;
}
//#endregion  
//#region StudyPlan
CanAddStudyPlan():boolean
{
return this.authService.auth.Roles.indexOf("")>-1;
//return true;
}
CanShowStudyPlan():boolean
{
return this.authService.auth.Roles.indexOf("ShowStudyPlan")>-1;
//return true;
}
CanUpdateStudyPlan():boolean
{
return this.authService.auth.Roles.indexOf("UpdateStudyPlan")>-1;
//return true;
}
CanDeleteStudyPlan():boolean
{
return this.authService.auth.Roles.indexOf("RemoveStudyPlan")>-1;
//return true;
}
//#endregion  
//#region Subject
CanAddSubject():boolean
{
return this.authService.auth.Roles.indexOf("AddSubject")>-1;
//return true;
}
CanShowSubject():boolean
{
return this.authService.auth.Roles.indexOf("ShowSubject")>-1;
//return true;
}
CanUpdateSubject():boolean
{
return this.authService.auth.Roles.indexOf("UpdateSubject")>-1;
//return true;
}
CanDeleteSubject():boolean
{
return this.authService.auth.Roles.indexOf("RemoveSubject")>-1;
//return true;
}
//#endregion  
//#region StudyYear
CanShowStudyYear():boolean{
  return this.authService.auth.Roles.indexOf("ShowStudyYear")>-1;
}
CanAddStudyYear():boolean{
  return this.authService.auth.Roles.indexOf("")>-1;
}
CanUpdateStudyYear():boolean{
  return this.authService.auth.Roles.indexOf("")>-1;
}
CanDeleteStudyYear():boolean{
  return this.authService.auth.Roles.indexOf("")>-1;
}
//#endregion
}
