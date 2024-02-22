import { Component, OnInit } from '@angular/core';
import { MedicService } from 'src/app/medic/medic.service';
import { AppointmentStatus, IAppointment } from 'src/app/models/IAppointment';
import { RoomService } from 'src/app/rooms/room.service';
import { AppointmentService } from '../appointment.service';

@Component({
  selector: 'app-appointments-by-patient',
  templateUrl: './appointments-by-patient.component.html',
  styleUrls: ['./appointments-by-patient.component.css']
})
export class AppointmentsByPatientComponent implements OnInit {
  appointmentsList: IAppointment[] = [];
  error: string = '';

  constructor(private appointmentsService: AppointmentService, private medicService: MedicService, private roomService: RoomService) { }

  ngOnInit(): void {
    this.getAppointmentsByPatient();
  }

  getAppointmentsByPatient(): void {
    this.appointmentsService.getNextAppointmentsByPatient().subscribe({
      next: (res) => {
        this.appointmentsList = res;
        this.appointmentsList.forEach(ap => {
          this.medicService.getMedicById(ap.medicId).subscribe({
            next: (res) => {
              ap.medic = res;
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
  
  public get AppointmentStatus() {
    return AppointmentStatus;
  }
}
