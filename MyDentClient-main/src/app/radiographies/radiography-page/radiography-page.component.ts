import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/login/service/authentication.service';
import { IPatient } from 'src/app/models/IPatient';
import { IUser, UserRole } from 'src/app/models/IUser';
import { PatientService } from 'src/app/patients/service/patient.service';

@Component({
  selector: 'app-radiography-page',
  templateUrl: './radiography-page.component.html',
  styleUrls: ['./radiography-page.component.css']
})
export class RadiographyPageComponent implements OnInit {
  patientId: number | undefined;
  patient!: IPatient;
  selectedFile: File | undefined
  public progress: number | undefined;
  public message: string | undefined;
  public radiographies: string[] = [];
  selectedRadiography: string = '';
  warningMessage: string = '';
  showImage: boolean = false;
  @Output() public onUploadFinished = new EventEmitter();
  public image: string | undefined;
  user!: IUser;
  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, private patientService: PatientService, private authenticationService: AuthenticationService) { }
  
  ngOnInit() {
    this.user = this.authenticationService.currentUserValue;
    if( this.user.role == UserRole.Medic){
      this.patientId = Number(this.route.snapshot.paramMap.get('id'));
      this.patientService.getPatientById(this.patientId).subscribe({
        next: (res) => {
          this.patient = res;
          this.patient.radiographies.forEach(element => {
            this.radiographies.push('data:image/png;base64,'+ element.image64);
          });
          if (this.radiographies === [])
          {
            this.warningMessage = "No radiography since now!";
          }
        }
      }
      );
    }
    else if( this.user.role == UserRole.Patient ){
      this.patientService.getPatientByUserId(this.user.id).subscribe({
        next: (res) => {
          this.patient = res;
          this.patient.radiographies.forEach(element => {
            this.radiographies.push('data:image/png;base64,'+ element.image64);
          });
          if (this.radiographies.length == 0)
          {
            this.warningMessage = "No radiography since now!";
          }
        },
        error: (err) => {
          console.log(err);
        }
      });
    }
  }

  public uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post('http://localhost:3646/medic/upload/'+ this.patientId, formData, {reportProgress: true, observe: 'events'})
      .subscribe(event => {
        if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
  
  public onSelectRadiography(imagePath: string){
    var corectPath = 'data:image/png;base64,'+ imagePath;
    if(this.selectedRadiography == corectPath){
      this.showImage = !this.showImage;
    }
    else{
      this.showImage = true;
    }
    this.selectedRadiography = 'data:image/png;base64,'+ imagePath;
  }

  public get UserRole() {
    return UserRole;
  }

  backToPatientFile(): void {
    this.router.navigateByUrl('/patient_info/'+this.patientId);
  }
}
