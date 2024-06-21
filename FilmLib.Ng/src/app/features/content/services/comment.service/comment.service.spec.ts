import { TestBed } from '@angular/core/testing';

import { FilmCommentService } from './comment.service';

describe('CommentService', () => {
  let service: FilmCommentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FilmCommentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
