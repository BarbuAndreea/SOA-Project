import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleComponent } from '@syncfusion/ej2-angular-schedule';
import { AddAppointmentComponent } from './appointment/add-appointment/add-appointment.component';
import { AppointmentHistoryPatientComponent } from './appointment/appointment-history-patient/appointment-history-patient.component';
import { AppointmentsByDateComponent } from './appointment/appointments-by-date/appointments-by-date.component';
import { AppointmentsByPatientComponent } from './appointment/appointments-by-patient/appointments-by-patient.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AddClinicComponent } from './clinic/components/add-clinic/add-clinic.component';
import { HomeComponent } from './home/home.component';
import { InterventionsListComponent } from './interventions/components/interventions-list/interventions-list.component';
import { InterventionsComponent } from './interventions/interventions.component';
import { AuthGuard } from './login/helpers/auth.guard';
import { NotAuthGuard } from './login/helpers/notauth.guard';
import { LoginComponent } from './login/login.component';
import { MedicInfoComponent } from './medic/medic-info/medic-info.component';
import { PatientInfoComponent } from './patients/components/patient-info/patient-info.component';
import { PatientsListComponent } from './patients/components/patients-list/patients-list.component';
import { RadiographyPageComponent } from './radiographies/radiography-page/radiography-page.component';
import { RegisterComponent } from './register/register.component';
import { AddRoomComponent } from './rooms/add-room/add-room.component';
import { AddUserComponent } from './users/components/add-user/add-user.component';
import { UsersListComponent } from './users/components/users-list/users-list.component';


const routes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [NotAuthGuard] },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'intervention/add', component: InterventionsComponent, canActivate: [AuthGuard] },
  { path: 'search_patient', component: PatientsListComponent, canActivate: [AuthGuard] },
  { path: 'search_user', component: UsersListComponent, canActivate: [AuthGuard] },
  { path: 'patient_info/:id', component: PatientInfoComponent, canActivate: [AuthGuard] },
  { path: 'personal_file', component: InterventionsListComponent, canActivate: [AuthGuard] },
  { path: 'my_appointments', component: AppointmentsByPatientComponent, canActivate: [AuthGuard] },
  { path: 'my_appointments_history', component: AppointmentHistoryPatientComponent, canActivate: [AuthGuard] },
  { path: 'add_intervention/:id', component: InterventionsComponent, canActivate: [AuthGuard] },
  { path: 'registration', component: RegisterComponent, canActivate: [NotAuthGuard] },
  { path: 'add_user', component: AddUserComponent, canActivate: [AuthGuard] },
  { path: 'intervention', component: InterventionsListComponent, canActivate: [AuthGuard] },
  { path: 'scheduel', component: ScheduleComponent, canActivate: [AuthGuard] },
  { path: 'medic/appointments_by_date', component: AppointmentsByDateComponent, canActivate: [AuthGuard] },
  { path: 'add_appointment', component: AddAppointmentComponent, canActivate: [AuthGuard] },
  { path: 'medic_info', component: MedicInfoComponent, canActivate: [AuthGuard] },
  { path: 'change_password', component: ChangePasswordComponent, canActivate: [AuthGuard] },
  { path: 'add_room', component: AddRoomComponent, canActivate: [AuthGuard] },
  { path: 'add_clinic', component: AddClinicComponent, canActivate: [AuthGuard] },
  { path: 'radiography/:id', component: RadiographyPageComponent, canActivate: [AuthGuard] },
  { path: 'patient_radiograpies', component: RadiographyPageComponent, canActivate: [AuthGuard] },
  // otherwise redirect to login
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
