export class CreateProductRequest {
  productName: string = '';
  categoryName: string = '';
  brandName: string = '';
  genericName: string = '';
  formulationName: string = '';
  classificationName: string = '';
  typeName: string = '';
  sheetsPerBox: number = 0;
  piecesPerSheet: number = 0;
  boxPrice: number = 0;
  sheetPrice: number = 0;
  piecePrice: number = 0;
  reorderLevel: number = 0;
  unitMeasurement: string = '';
  piecePerBox: number = 0;
  costBoxPrice : number = 0;
  costSheetPrice : number = 0;
  costPiecePrice: number = 0;
}
