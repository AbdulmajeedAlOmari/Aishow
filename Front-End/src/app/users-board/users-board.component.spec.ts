import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersBoardComponent } from './users-board.component';

describe('UsersBoardComponent', () => {
  let component: UsersBoardComponent;
  let fixture: ComponentFixture<UsersBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersBoardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
