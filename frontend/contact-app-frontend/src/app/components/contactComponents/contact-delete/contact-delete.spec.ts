import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactDelete } from './contact-delete';

describe('ContactDelete', () => {
  let component: ContactDelete;
  let fixture: ComponentFixture<ContactDelete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContactDelete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContactDelete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
