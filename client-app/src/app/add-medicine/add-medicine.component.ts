import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { PharmacyApiService } from '../services/pharmacy-api-service';

@Component({
  selector: 'app-add-medicine',
  templateUrl: './add-medicine.component.html',
  styleUrls: ['./add-medicine.component.css']
})
export class AddMedicineComponent {
  submitted = false;
  error = '';

  medicineForm = this.fb.group({
    fullName: ['', Validators.required],
    notes: [''],
    expiryDate: ['', Validators.required],
    quantity: [0, [Validators.required, Validators.min(0)]],
    price: [0, [Validators.required, Validators.min(0)]],
    brand: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private api: PharmacyApiService,
    private router: Router
  ) {}
  @Output() medicineAdded = new EventEmitter<void>();
  save(): void {
    this.submitted = true;
    this.error = '';

    if (this.medicineForm.invalid) return;

    this.api.addMedicine(this.medicineForm.value as any).subscribe({
      next: () => {
        this.medicineAdded.emit();
        this.medicineForm.reset();
        this.submitted = false;
      },
      error: (err: HttpErrorResponse) => this.error = err.error || 'Unable to save medicine.'
    });
  }
}