import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './login/helpers/jwt.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RegisterComponent } from './register/register.component';
import { InterventionsComponent } from './interventions/interventions.component';
import { PatientsListComponent } from './patients/components/patients-list/patients-list.component';
import { PatientInfoComponent } from './patients/components/patient-info/patient-info.component';
import { UsersListComponent } from './users/components/users-list/users-list.component';
import { AddUserComponent } from './users/components/add-user/add-user.component';
import { InterventionsListComponent } from './interventions/components/interventions-list/interventions-list.component';
import { AddInterventionComponent } from './interventions/components/add-intervention/add-intervention.component';
import { ErrorInterceptor } from './login/helpers/error.interceptor';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { AppointmentsByDateComponent } from './appointment/appointments-by-date/appointments-by-date.component';
import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { AddAppointmentComponent } from './appointment/add-appointment/add-appointment.component';
import { MatDialogModule } from '@angular/material/dialog';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { MatSelectModule } from '@angular/material/select';
import { AppointmentsByPatientComponent } from './appointment/appointments-by-patient/appointments-by-patient.component';
import { MedicInfoComponent } from './medic/medic-info/medic-info.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AddHolidayComponent } from './holiday/add-holiday/add-holiday.component';
import { EditInfoMedicComponent } from './medic/edit-info-medic/edit-info-medic.component';
import { MatButtonModule } from '@angular/material/button';
import { AddRoomComponent } from './rooms/add-room/add-room.component';
import { MatIconModule } from '@angular/material/icon';
import { AddClinicComponent } from './clinic/components/add-clinic/add-clinic.component';
import { RadiographyPageComponent } from './radiographies/radiography-page/radiography-page.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { GoogleMapsModule } from '@angular/google-maps';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AppointmentHistoryPatientComponent } from './appointment/appointment-history-patient/appointment-history-patient.component';
import { SafePipe } from './pipes/safe.pipe'

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    InterventionsComponent,
    PatientsListComponent,
    PatientInfoComponent,
    AddUserComponent,
    InterventionsListComponent,
    AddInterventionComponent,
    AppointmentsByDateComponent,
    AddAppointmentComponent,
    AppointmentsByPatientComponent,
    MedicInfoComponent,
    ChangePasswordComponent,
    AddHolidayComponent,
    EditInfoMedicComponent,
    AddRoomComponent,
    AddClinicComponent,
    RadiographyPageComponent,
    UsersListComponent,
    AppointmentHistoryPatientComponent,
    SafePipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgxMatNativeDateModule,
    NgxMaterialTimepickerModule,
    MatDialogModule,
    Ng2SearchPipeModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatExpansionModule,
    MatGridListModule,
    MatCardModule,
    GoogleMapsModule,
    MatSnackBarModule
  ],
  exports: [
    LoginComponent,
    MatMomentDateModule,
    SafePipe
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
