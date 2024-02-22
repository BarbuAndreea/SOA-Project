import { Component, OnInit } from '@angular/core';
import { AppointmentStatus, IAppointment } from 'src/app/models/IAppointment';
import { AppointmentService } from '../appointment.service';
import { MatDialog } from '@angular/material/dialog';
import { AddAppointmentComponent } from '../add-appointment/add-appointment.component';
import { Router } from '@angular/router';
import { PatientService } from 'src/app/patients/service/patient.service';
import { RoomService } from 'src/app/rooms/room.service';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { UserRole } from 'src/app/models/IUser';

@Component({
  selector: 'app-appointments-by-date',
  templateUrl: './appointments-by-date.component.html',
  styleUrls: ['./appointments-by-date.component.css']
})
export class AppointmentsByDateComponent implements OnInit {
  selectedDate: Date = new Date()
  selectedStartTime: Date = new Date()
  selectedEndTime: Date = new Date()
  appointments: IAppointment[] = []
  error = '';
  datePickerVisible = true

  constructor(private appointmentService: AppointmentService, private authenticationService: AuthenticationService, public dialog: MatDialog, private router: Router, private patientService: PatientService, private roomService: RoomService) {
  }

  ngOnInit(): void {
    if (this.router.url == "/home") {
      this.datePickerVisible = false;
    }
    this.getAppoinmentsByDate(this.selectedDate);
  }

  openDialog(id: number | undefined): void {
    let dialogRef;
    if (id == undefined) {
      dialogRef = this.dialog.open(AddAppointmentComponent, {
        width: '500px',
        data: { appointment: null }
      });
    } else {
      var selectedAppointment = this.appointments.find(a => a.id == id);
      dialogRef = this.dialog.open(AddAppointmentComponent, {
        width: '500px',
        data: { appointment: selectedAppointment }
      });
    }

    dialogRef.afterClosed().subscribe(result => {
      this.getAppoinmentsByDate(this.selectedDate);
      if (this.router.url == "/home") {
        window.location.reload()
      }
    });
  }

  dateChangedWithoutConfirm(): void {

    this.getAppoinmentsByDate(this.selectedDate);
  }

  getAppoinmentsByDate(date: Date): void {
    this.appointments = [];
    this.error = '';
    var currentUser = this.authenticationService.currentUserValue;
    var id = currentUser?.id != undefined ? currentUser.id : -1;
    this.appointmentService.displayAppointmentsByDate(date).subscribe({
      next: (res) => {
        this.appointments = res;
        this.appointments.forEach(ap => {
          this.patientService.getPatientById(ap.patientId).subscribe({
            next: (res) => {
              ap.patient = res;
            }
          })
          this.roomService.getRoom(ap.roomId).subscribe({
            next: (res) => {
              ap.roomName = res.name;
            }
          })
        });
      },
      error: (err) => {
        this.error = err;
      }
    })
  }

  clickMethod(appointment: IAppointment) {
    if (confirm("Are you sure to delete this appointment?")) {
      this.appointmentService.deleteAppointmentById(appointment.id).subscribe({
        next: () => {
          this.getAppoinmentsByDate(this.selectedDate);
        },
        error: (err) => {
          console.log(err);
        }
      });
    }
  }

  public get AppointmentStatus() {
    return AppointmentStatus;
  }
}
