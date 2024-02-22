import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { MedicService } from 'src/app/medic/medic.service';
import { AppointmentStatus, IAppointment } from 'src/app/models/IAppointment';
import { IMedic } from 'src/app/models/IMedic';
import { IPatient } from 'src/app/models/IPatient';
import { IRoom } from 'src/app/models/IRoom';
import { UserRole } from 'src/app/models/IUser';
import { PatientService } from 'src/app/patients/service/patient.service';
import { RoomService } from 'src/app/rooms/room.service';
import { AppointmentService } from '../appointment.service';

@Component({
  selector: 'app-add-appointment',
  templateUrl: './add-appointment.component.html',
  styleUrls: ['./add-appointment.component.css']
})
export class AddAppointmentComponent implements OnInit {
  selectedStartTime: Date = new Date()
  selectedEndTime: Date = new Date()
  selectedDate: Date = new Date()
  newAppointment: IAppointment = { patientId: -1, roomId: -1, medicId: -1, name: "", startDate: new Date(), endDate: new Date(), status: AppointmentStatus.Unstarted }
  patientList: IPatient[] = []
  roomList: IRoom[] = []
  medicList: IMedic[] = []
  status: Array<string> = Object.keys(AppointmentStatus).filter(key => isNaN(+key));
  backendErrorMessage: string = ""
  errorMessage: string = ""
  appointmentForm = this.formBuilder.group({
    patientId: [null],
    roomId: [null, Validators.required],
    medicId: [null, Validators.required],
    name: ['', Validators.required],
    status: [null],
    searchPatient: [null]
  });
  isEditMode: Boolean = false;
  currentUserRole!: UserRole

  constructor(private appointmentService: AppointmentService, private patientService: PatientService, private roomService: RoomService, public dialogRef: MatDialogRef<AddAppointmentComponent>, private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService, private medicService: MedicService, @Inject(MAT_DIALOG_DATA) public data: { appointment: IAppointment }) {
  }

  ngOnInit(): void {
    this.currentUserRole = this.authenticationService.currentUserValue.role;
    this.isEditMode = this.data.appointment != null;
    if (this.isEditMode == true) {
      this.selectedDate = new Date(this.data.appointment.startDate);
      this.selectedStartTime = new Date(this.data.appointment.startDate);
      this.selectedEndTime = new Date(this.data.appointment.endDate);
      this.formFields['patientId'].setValue(this.data.appointment.patientId);
      this.formFields['roomId'].setValue(this.data.appointment.roomId);
      this.formFields['medicId'].setValue(this.data.appointment.medicId);
      this.formFields['name'].setValue(this.data.appointment.name);
      this.formFields['status'].setValue(this.data.appointment.status.toString());
    }
    this.getPatients();
    this.getRooms();
    if (this.currentUserRole == UserRole.ClinicAdmin) {
      this.getMedicsByClinic();
    }
  }

  get formFields() { return this.appointmentForm.controls; }

  getPatients(): void {
    this.patientService.getPatientsByClinic().subscribe({
      next: (res) => {
        this.patientList = res;
      },
      error: (err) => {
        console.log(err);
      }
    }
    )
  }

  getRooms(): void {
    this.roomService.getRoomsByClinic().subscribe({
      next: (res) => {
        this.roomList = res;
      },
      error: (err) => {
        console.log(err);
      }
    }
    )
  }

  getMedicsByClinic(): void {
    this.medicService.getMedicsByClinic().subscribe({
      next: (res) => {
        this.medicList = res;
      },
      error: (err) => {
        console.log(err);
      }
    }
    )
  }

  getSelectedDateTime(selectedTime: Date): Date {
    const correctDate = new Date(this.selectedDate);
    selectedTime.setDate(correctDate.getDate());
    selectedTime.setMonth(correctDate.getMonth());
    selectedTime.setFullYear(correctDate.getFullYear());
    selectedTime.setSeconds(0);
    return new Date(selectedTime);
  }

  addAppointment(appointment: IAppointment): void {
    this.appointmentService.addAppointment(appointment)
      .subscribe({
        next: (res) => {
          this.backendErrorMessage = '';
          this.dialogRef.close('succes');
        },
        error: (err) => {
          this.backendErrorMessage = err;
        }
      });
  }

  editAppointment(appointment: IAppointment): void {
    this.appointmentService.editAppointment(appointment)
      .subscribe({
        next: (res) => {
          this.backendErrorMessage = '';
          this.dialogRef.close('succes');
        },
        error: err => {
          this.backendErrorMessage = err;
        }
      });
  }

  onSubmit() {
    if(this.formFields['patientId'].value == null || this.formFields['roomId'].value == null || this.formFields['name'].value == null)
    {
      this.errorMessage = "Make sure you fill all the fields!";
    }
    this.newAppointment = {
      name: this.formFields['name'].value,
      startDate: this.getSelectedDateTime(this.selectedStartTime),
      endDate: this.getSelectedDateTime(this.selectedEndTime),
      patientId: this.formFields['patientId'].value,
      roomId: this.formFields['roomId'].value,
      status: AppointmentStatus.Unstarted,
      medicId: this.currentUserRole == UserRole.ClinicAdmin ? this.formFields['medicId'].value : 0
    };

    if (this.isEditMode == false) {
      this.addAppointment(this.newAppointment);
    }
    else {
      this.newAppointment.id = this.data.appointment.id;
      if(this.currentUserRole == UserRole.Medic)
      {
        this.newAppointment.medicId = this.data.appointment.medicId;
      }
      this.newAppointment.status = Number(this.formFields['status'].value);
      this.editAppointment(this.newAppointment);
    }

  }

  public get UserRole() {
    return UserRole;
  }
}
