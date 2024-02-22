import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddHolidayComponent } from 'src/app/holiday/add-holiday/add-holiday.component';
import { HolidayService } from 'src/app/holiday/holiday.service';
import { IMedic, Specializations } from 'src/app/models/IMedic';
import { EditInfoMedicComponent } from '../edit-info-medic/edit-info-medic.component';
import { MedicService } from '../medic.service';

@Component({
  selector: 'app-medic-info',
  templateUrl: './medic-info.component.html',
  styleUrls: ['./medic-info.component.css']
})
export class MedicInfoComponent implements OnInit {
  medic!: IMedic;
  dateUpdate: string ='';

  constructor(private medicService: MedicService, private holidayService: HolidayService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getMedicByUserId()
  }

  openDialog(id: number | undefined, editInfo: boolean): void {
    let dialogRef;
    if (id == undefined && editInfo == false) {
      dialogRef = this.dialog.open(AddHolidayComponent, {
        width: '500px',
        data: { holiday: null }
      });
    } else if (id != undefined && editInfo == false){
      var selectedHoliday = this.medic?.holidays?.find(a => a.id == id);
      dialogRef = this.dialog.open(AddHolidayComponent, {
        width: '500px',
        data: { holiday: selectedHoliday }
      });
    } else {
        dialogRef = this.dialog.open(EditInfoMedicComponent, {
          width: '500px',
          data: { medic: this.medic }
        });
      }

    dialogRef.afterClosed().subscribe(result => {
      this.getMedicByUserId();
    });
  }

  getMedicByUserId(): void {
    this.medicService.getMedicByUserId().subscribe({
      next: (res) => {
        this.medic = res;
      },
      error: (err) => {
        console.log(err);
      }
    }
    );
  }

  onDeleteHoliday(id: number | undefined) {
    if (confirm("Are you sure to delete this clinic?")) {
      this.holidayService.deleteHoliday(id)
        .subscribe({
          next: () => {
            this.getMedicByUserId();
          },
          error: (e) => {
            console.log(e)
          }
        })
    }
  }

  public get Specializations() {
    return Specializations;
  }

}
