import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommitDetails } from './commit-details';

describe('CommitDetails', () => {
  let component: CommitDetails;
  let fixture: ComponentFixture<CommitDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommitDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(CommitDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
