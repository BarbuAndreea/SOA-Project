import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { MedicService } from 'src/app/medic/medic.service';
import { IIntervention, ToothEnum } from 'src/app/models/IIntervention';
import { UserRole } from 'src/app/models/IUser';
import { PatientService } from 'src/app/patients/service/patient.service';
import { InterventionsComponent } from '../../interventions.component';
import { InterventionService } from '../../service/intervention.service';

@Component({
  selector: 'app-interventions-list',
  templateUrl: './interventions-list.component.html',
  styleUrls: ['./interventions-list.component.css']
})
export class InterventionsListComponent implements OnInit {
  interventionList: IIntervention[] = [];
  patientId!: number;
  currentUserRole: UserRole = UserRole.Medic;
  currentUserId: number | undefined;
  error = '';
  totalCost: number = 0;

  constructor(private interventionService: InterventionService, private patientService: PatientService, private medicService: MedicService, public dialog: MatDialog, private route: ActivatedRoute, private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.route.params.subscribe(
      (params) => {
        this.patientId = params['id'];
      });
  
    this.currentUserRole = this.authenticationService.currentUserValue.role;
    this.currentUserId = this.authenticationService.currentUserValue.id;
    if (this.currentUserRole == UserRole.Medic || this.currentUserRole == UserRole.ClinicAdmin) {
      this.getInterventionsByPatientId(this.patientId);
    }
    if (this.currentUserRole == UserRole.Patient)
    {
      this.getInterventionsByPatient();
    }
  }
  
  public get UserRole() {
    return UserRole;
  }
  
  public get ToothEnum() {
    return ToothEnum;
  }

  openDialog(intervention: IIntervention | undefined): void {
    let dialogRef;
    if (intervention == undefined) {
      dialogRef = this.dialog.open(InterventionsComponent, {
        width: '500px',
        data: { intervention: null, patientId: this.patientId }
      });
    } else {
      var selectedIntervention = this.interventionList.find(a => a.id == intervention.id);
      dialogRef = this.dialog.open(InterventionsComponent, {
        width: '500px',
        data: { intervention: selectedIntervention }
      });
    }

    dialogRef.afterClosed().subscribe(result => {
      this.getInterventionsByPatientId(this.patientId);
    });
  }

  getInterventionsByPatientId(patientId: number): void {
    this.interventionService.getInterventionsByPatientId(patientId).subscribe({
      next: (res) => {
        this.interventionList = res;
        console.log(this.interventionList);
        if (this.interventionList.length == 0)
        {
          this.error = "There is no intervention for this patient!";
        }
        this.interventionList.forEach(interv => {
          this.medicService.getMedicById(interv.medicId).subscribe({
            next: (res) => {
              interv.medic = res;
            }
          })
        });
      },
      error: (err) => {
        console.log(err);
      }
    }
    );
  }
  
  public get userRole() {
    return UserRole;
  }

  getInterventionsByPatient(): void {
    this.interventionService.getInterventionsByPatient().subscribe({
      next: (res) => {
        this.interventionList = res;
        this.interventionList.forEach(interv => {
          this.medicService.getMedicById(interv.medicId).subscribe({
            next: (res) => {
              this.totalCost = this.totalCost + interv.price;
              interv.medic = res;
            }
          })
        });
      },
      error: (err) => {
        this.error = err;
      }
    }
    );
  }
}
