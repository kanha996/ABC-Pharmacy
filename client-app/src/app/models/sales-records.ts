export interface SaleRecord {
  id: number;
  medicineId: number;
  medicineName: string;
  soldQuantity: number;
  unitPrice: number;
  totalAmount: number;
  soldOn: string;
}