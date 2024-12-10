import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplayScreenComponent } from './replay-screen.component';

describe('ReplayScreenComponent', () => {
  let component: ReplayScreenComponent;
  let fixture: ComponentFixture<ReplayScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReplayScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReplayScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
