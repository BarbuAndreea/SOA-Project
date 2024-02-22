import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../appointment/appointment.service';
import { ClinicService } from '../clinic/service/clinic.service';
import { AuthenticationService } from '../login/service/authentication.service';
import { AppointmentStatus, IAppointment } from '../models/IAppointment';
import { IClinic } from '../models/IClinic';
import { IPatient } from '../models/IPatient';
import { UserRole } from '../models/IUser';
import { PatientService } from '../patients/service/patient.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SafePipe } from '../pipes/safe.pipe'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentUserRole: UserRole = UserRole.Patient;
  currentUserName: string = '';
  backendErrorMessage: string = '';
  currentDate: Date = new Date();
  activeAppointments: IAppointment[] = []
  status: Array<string> = Object.keys(AppointmentStatus).filter(key => isNaN(+key));
  errorMessage = '';
  importError = '';
  patientCode: string = "";
  patient!: IPatient;
  clinics: IClinic[] = [];
  panelOpenState = false;
  state!: AppointmentStatus;
  warningMessage: string = '';
  embed : string = "&output=embed";

  constructor(private authenticationService: AuthenticationService, private snackBar: MatSnackBar, private appointmentService: AppointmentService, private clinicService: ClinicService, private patientService: PatientService) { }

  ngOnInit(): void {
    this.currentUserRole = this.authenticationService.currentUserValue.role;
    this.currentUserName = this.authenticationService.currentUserValue.lastName;
    if (this.currentUserRole == UserRole.Medic) {
      this.getAppointmnetsByStatus(AppointmentStatus.Active);
    }
    if (this.currentUserRole == UserRole.Patient) {
      this.getClinicsByPatient();
    }
  }

  public get AppointmentStatus() {
    return AppointmentStatus;
  }


  getClinicsByPatient(): void {
     this.clinicService.getClinicsByPatient().subscribe({
      next: (res) => {
        this.clinics = res;
      },
      error: (err) => {
        this.warningMessage = err;
      }
    })
  }

  getAppointmnetsByStatus(status: AppointmentStatus): void {
    this.appointmentService.getAppointmentsByStatus(status).subscribe({
      next: (res) => {
        this.activeAppointments = res;
        this.activeAppointments.forEach(ap => {
          this.patientService.getPatientById(ap.patientId).subscribe({
            next: (res) => {
              ap.patient = res;
            }
          })
        });
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = err;
      }
    })
  }

  onStatusChanged(newStatus: string, appointment: IAppointment): void {
    let updateAppointment: IAppointment = appointment;
    updateAppointment.status = Number(newStatus);

    this.appointmentService.editAppointmentStatus(updateAppointment)
      .subscribe({
        next: (res) => {
          this.backendErrorMessage = '';
          this.getAppointmnetsByStatus(AppointmentStatus.Active);
          location.reload();
        },
        error: (err) => {
          this.backendErrorMessage = "An error ocurred and the operation was blocked! Please try again!";
        }
      });
  }

  public get UserRole() {
    return UserRole;
  }

  importPatient(patientId: string) {
    this.patientService.importPatient(patientId)
      .subscribe({
        next: (res) => {
          this.importError = '';
          this.snackBar.open(`User ${res.userP.firstName} ${res.userP.lastName} was add with success in your clinic!`, '', { duration: 3000 });
        },
        error: (err) => {
          this.importError = err;
        }
      });
  }

}
