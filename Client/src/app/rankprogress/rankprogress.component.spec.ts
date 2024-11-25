import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RankprogressComponent } from './rankprogress.component';

describe('RankprogressComponent', () => {
  let component: RankprogressComponent;
  let fixture: ComponentFixture<RankprogressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RankprogressComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RankprogressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
