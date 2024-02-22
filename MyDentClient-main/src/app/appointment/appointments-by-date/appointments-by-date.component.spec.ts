import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentsByDateComponent } from './appointments-by-date.component';

describe('AppointmentsByDateComponent', () => {
  let component: AppointmentsByDateComponent;
  let fixture: ComponentFixture<AppointmentsByDateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointmentsByDateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentsByDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
