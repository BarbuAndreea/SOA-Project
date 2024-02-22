import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentHistoryPatientComponent } from './appointment-history-patient.component';

describe('AppointmentHistoryPatientComponent', () => {
  let component: AppointmentHistoryPatientComponent;
  let fixture: ComponentFixture<AppointmentHistoryPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointmentHistoryPatientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentHistoryPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
