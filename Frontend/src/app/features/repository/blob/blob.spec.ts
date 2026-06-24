import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Blob } from './blob';

describe('Blob', () => {
  let component: Blob;
  let fixture: ComponentFixture<Blob>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Blob],
    }).compileComponents();

    fixture = TestBed.createComponent(Blob);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
