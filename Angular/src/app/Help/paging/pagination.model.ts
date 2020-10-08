
export class PagingDTO{
    selectItemsPerPage: number[] = [5, 10, 20,30];
    RowCount:number= this.selectItemsPerPage[0];
    Page:number=1
    allItemsLength = 0;
}
export class PagingDetalis
{
      TotalRows :number
      TotalPages :number
      CurrentPage :number
      HasNexPage :boolean
      HasPreviousPage :boolean
      NextUrl :string
      PreviousUrl :string
}