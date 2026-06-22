import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PharmacyApiService } from '../services/pharmacy-api-service';
import { Medicine } from '../models/medicine';

@Component({
  selector: 'app-sales-entry',
  templateUrl: './sales-entry.component.html',
  styleUrls: ['./sales-entry.component.css']
})
export class SalesEntryComponent implements OnInit {
  medicines: Medicine[] = [];
  success = '';
  error = '';

  saleForm = this.fb.group({
    medicineId: ['', Validators.required],
    soldQuantity: [1, [Validators.required, Validators.min(1)]]
  });

  constructor(private fb: FormBuilder, private api: PharmacyApiService) {}

  ngOnInit(): void {
    this.api.getMedicines().subscribe(data => this.medicines = data);
  }

  save(): void {
    this.success = '';
    this.error = '';

    if (this.saleForm.invalid) return;

    this.api.addSale({
      medicineId: Number(this.saleForm.value.medicineId),
      soldQuantity: Number(this.saleForm.value.soldQuantity)
    }).subscribe({
      next: () => {
        this.success = 'Sale recorded successfully.';
        this.saleForm.patchValue({ soldQuantity: 1, medicineId: '' });
        this.api.getMedicines().subscribe(data => this.medicines = data);
      },
      error: err => this.error = err.error || 'Unable to record sale.'
    });
  }
}