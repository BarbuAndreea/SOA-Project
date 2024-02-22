import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditInfoMedicComponent } from './edit-info-medic.component';

describe('EditInfoMedicComponent', () => {
  let component: EditInfoMedicComponent;
  let fixture: ComponentFixture<EditInfoMedicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditInfoMedicComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditInfoMedicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
