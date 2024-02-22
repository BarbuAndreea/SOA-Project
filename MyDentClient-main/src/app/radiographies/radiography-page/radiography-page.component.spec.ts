import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RadiographyPageComponent } from './radiography-page.component';

describe('RadiographyPageComponent', () => {
  let component: RadiographyPageComponent;
  let fixture: ComponentFixture<RadiographyPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RadiographyPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RadiographyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
