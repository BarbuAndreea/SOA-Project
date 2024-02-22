import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IClinic } from 'src/app/models/IClinic';
import { IRoom } from 'src/app/models/IRoom';
import { ClinicService } from '../../service/clinic.service';

@Component({
  selector: 'app-add-clinic',
  templateUrl: './add-clinic.component.html',
  styleUrls: ['./add-clinic.component.css']
})
export class AddClinicComponent implements OnInit {
  clinicForm!: FormGroup;
  errorMessage: string = '';
  clinics: IClinic[] = [];
  backendErrorMessage: string = '';
  isFullDescriptionVisibile: boolean = false;
  selectedClinic: number | undefined = -1;
  clinic!: IClinic;


  constructor(private clinicService: ClinicService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.getAllClinics();
    this.clinicForm = this.formBuilder.group({
      nameInput: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      address: [''],
      description: [''],
      mapsAddress: ['']
    })
  }

  toggleFullDescription() {
    this.isFullDescriptionVisibile = !this.isFullDescriptionVisibile;
  }

  getAllClinics(): void {
    this.clinicService.getClinics()
      .subscribe({
        next: (res) => {
          this.clinics = res;
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  get formFields() { return this.clinicForm.controls; }

  onSubmit() {
    if (this.clinicForm.invalid) {
      return;
    }

    this.clinic = {
      name: this.formFields['nameInput'].value,
      phoneNumber: this.formFields['phone'].value,
      email: this.formFields['email'].value,
      address: this.formFields['address'].value,
      description: this.formFields['description'].value,
      mapsAddress: this.formFields['mapsAddress'].value
    }

    if (this.selectedClinic == -1) {
      this.addClinic(this.clinic);
    }
    else {
      this.clinic.id = this.selectedClinic,
        this.clinicService.updateClinic(this.clinic)
          .subscribe({
            next: () => {
              this.errorMessage = '';
              this.getAllClinics();
              this.clinicForm.reset();
              this.selectedClinic = -1;
            },
            error: () => {
              this.errorMessage = 'An error occured and the office location could not be added! Please try again!';
            }
          })
    }
  }

  addClinic(clinic: IClinic): void {
    this.clinicService.addClinic(clinic)
      .subscribe({
        next: () => {
          this.errorMessage = '';
          this.getAllClinics();
          this.clinicForm.reset();
        },
        error: () => {
          this.errorMessage = 'An error occured and the office location could not be added! Please try again!';
        }
      })
  }

  onDeleteClinic(id: number | undefined) {
    this.clinicForm.reset();
    if (confirm("Are you sure to delete this clinic?")) {
      this.clinicService.deleteClinic(id)
        .subscribe({
          next: () => {
            this.getAllClinics();
          },
          error: (e) => {
            console.log(e)
          }
        })
    }
  }

  populateForm(clinic: IClinic) {
    this.selectedClinic = clinic.id;
    this.formFields['nameInput'].setValue(clinic.name);
    this.formFields['phone'].setValue(clinic.phoneNumber);
    this.formFields['email'].setValue(clinic.email);
    this.formFields['address'].setValue(clinic.address);
    this.formFields['description'].setValue(clinic.description);
    this.formFields['mapsAddress'].setValue(clinic.mapsAddress);
  }

  cancelAddClinic() {
    this.clinicForm.reset();
    this.selectedClinic = -1;
  }

}
