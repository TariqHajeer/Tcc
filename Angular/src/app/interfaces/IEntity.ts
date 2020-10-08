export interface IEntity {
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
}
export interface IEnabelableEntity extends IEntity {
    IsEnabled:boolean;   
}