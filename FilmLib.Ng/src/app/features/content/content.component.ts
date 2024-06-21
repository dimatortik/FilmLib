// content.component.ts
import { Component, OnInit } from '@angular/core';
import { PaginationModel } from '../../core/models/pagination.model';
import { FilmService } from './services/film.service/movie.service';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MovieCardComponent } from '../../shared/post-card-view/post-card-view.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { NgForOf, TitleCasePipe } from '@angular/common';
import { IMovie } from './interfaces/movie.interface';
import { Swiper, SwiperOptions, SwiperModule } from 'swiper/types';

@Component({
  selector: 'app-movies',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss'],
  imports: [
    MatPaginatorModule,
    MovieCardComponent,
    MatButtonModule,
    MatCardModule,
    TitleCasePipe,
    NgForOf,
  ],
  standalone: true,
})
export class ContentComponent implements OnInit {
  sortBy = 'title';
  sortOrder = 'asc';
  films: Array<IMovie> = [];
  pageSize = 8;
  page = 0;
  searchTerm: string = '';

  config: SwiperOptions = {
    watchSlidesProgress: true,
    breakpoints: {
      992: {
        slidesPerView: 6.3,
        spaceBetween: 20,
        slidesOffsetBefore: 0,
        slidesOffsetAfter: 0,
      },
      768: {
        slidesPerView: 4.3,
        spaceBetween: 15,
        slidesOffsetBefore: 0,
        slidesOffsetAfter: 0,
      },
      576: {
        slidesPerView: 3.3,
        spaceBetween: 15,
        slidesOffsetBefore: 0,
        slidesOffsetAfter: 0,
      },
      320: {
        slidesPerView: 2.3,
        spaceBetween: 10,
        slidesOffsetBefore: 10,
        slidesOffsetAfter: 10,
      },
    },
  };

  totalResults: any;

  constructor(private filmService: FilmService, private router: Router) {}

  ngOnInit() {
    this.getMovies();
  }

  getMovies() {
    this.filmService
      .getFilms(
        this.page + 1,
        this.pageSize,
        this.searchTerm,
        this.sortBy,
        this.sortOrder
      )
      .pipe(take(1))
      .subscribe((response: PaginationModel<IMovie>) => {
        this.films = response.items;
        this.totalResults = response.totalCount;
      });
  }
  setSortBy(sortBy: string): void {
    this.sortBy = sortBy;
    this.getMovies();
  }

  setSortOrder(sortOrder: string): void {
    this.sortOrder = sortOrder;
    this.getMovies();
  }

  changePage(event: PageEvent) {
    this.page = event.pageIndex;
    this.pageSize = event.pageSize;
    this.getMovies();
  }
}
