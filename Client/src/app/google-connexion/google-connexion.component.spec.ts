import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoogleConnexionComponent } from './google-connexion.component';

describe('GoogleConnexionComponent', () => {
  let component: GoogleConnexionComponent;
  let fixture: ComponentFixture<GoogleConnexionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GoogleConnexionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GoogleConnexionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
