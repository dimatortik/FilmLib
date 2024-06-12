import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostCardViewComponent } from './post-card-view.component';

describe('PostCardViewComponent', () => {
  let component: PostCardViewComponent;
  let fixture: ComponentFixture<PostCardViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostCardViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PostCardViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
