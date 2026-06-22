import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddMedicineComponent } from './add-medicine/add-medicine.component';
import { SalesEntryComponent } from './sales-entry/sales-entry.component';
import { MedicineListComponent } from './medicine-list/medicine-list.component';


const routes: Routes = [
  { path: '', component: MedicineListComponent },
  { path: 'add-medicine', component: AddMedicineComponent },
  { path: 'sales', component: SalesEntryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
