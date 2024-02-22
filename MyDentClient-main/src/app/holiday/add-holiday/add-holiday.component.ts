import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { MedicService } from 'src/app/medic/medic.service';
import { IHoliday } from 'src/app/models/IHoliday';
import { UserRole } from 'src/app/models/IUser';
import { HolidayService } from '../holiday.service';

@Component({
  selector: 'app-add-holiday',
  templateUrl: './add-holiday.component.html',
  styleUrls: ['./add-holiday.component.css']
})
export class AddHolidayComponent implements OnInit {
  isEditMode: Boolean = false;
  selectedStartDate: Date = new Date();
  selectedEndDate: Date = new Date();
  newHoliday!: IHoliday;
  backendErrorMessage: string = ''

  constructor(public dialogRef: MatDialogRef<AddHolidayComponent>, private holidayService: HolidayService, @Inject(MAT_DIALOG_DATA) public data: { holiday: IHoliday }) { }

  ngOnInit(): void {
    this.isEditMode = this.data.holiday != null;

    if (this.isEditMode == true) {
      this.selectedStartDate = this.data.holiday.startDate;
      this.selectedEndDate = this.data.holiday.endDate;
    }
  }

  onSubmit() {
    this.newHoliday = {
      medicId: 0,
      startDate: this.selectedStartDate,
      endDate: this.selectedEndDate
    };

    if (this.isEditMode == false) {
      this.holidayService.addHoliday(this.newHoliday).subscribe({
        next: () => {
          this.backendErrorMessage = '';
          this.dialogRef.close();
        },
        error: (err) => {
          this.backendErrorMessage = err;
        }
      });;
    }
    else {
      this.newHoliday.id = this.data.holiday.id;
      this.holidayService.updateHoliday(this.newHoliday)
        .subscribe({
          next: (res) => {
            this.backendErrorMessage = '';
            this.dialogRef.close();
          },
          error: (err) => {
            this.backendErrorMessage = err;
          }
        });
    }
  }
}
