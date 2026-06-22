import { Component, OnInit } from '@angular/core';
import { Medicine } from '../models/medicine';
import { PharmacyApiService } from '../services/pharmacy-api-service';

@Component({
  selector: 'app-medicine-list',
  templateUrl: './medicine-list.component.html',
  styleUrls: ['./medicine-list.component.css']
})
export class MedicineListComponent implements OnInit {
  medicines: Medicine[] = [];
  searchText = '';

  constructor(private api: PharmacyApiService) {}

  ngOnInit(): void {
    this.loadMedicines();
  }

  loadMedicines(): void {
    this.api.getMedicines().subscribe(data => {
      this.medicines = data;
    });
  }

  get filteredMedicines(): Medicine[] {
    return this.medicines.filter(x =>
      x.fullName.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }

  isExpiringSoon(expiryDate: string): boolean {
    const today = new Date();
    const expiry = new Date(expiryDate);
    const diff = expiry.getTime() - today.getTime();
    const days = diff / (1000 * 60 * 60 * 24);
    return days >= 0 && days < 30;
  }

  isLowStock(quantity: number): boolean {
    return quantity < 10;
  }

  getRowClass(medicine: Medicine): string {
    if (this.isExpiringSoon(medicine.expiryDate)) return 'expiring';
    if (this.isLowStock(medicine.quantity)) return 'low-stock';
    return '';
  }
}