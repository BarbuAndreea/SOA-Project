import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentsByPatientComponent } from './appointments-by-patient.component';

describe('AppointmentsByPatientComponent', () => {
  let component: AppointmentsByPatientComponent;
  let fixture: ComponentFixture<AppointmentsByPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointmentsByPatientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentsByPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
