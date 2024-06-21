import { Component, OnInit } from '@angular/core';
import { FilmService } from '../content/services/film.service/movie.service';
import { SeoService } from '../../core/services/seo.service';
import { take } from 'rxjs/operators';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
import { MovieCardComponent } from '../../shared/post-card-view/post-card-view.component';
import { RouterLink } from '@angular/router';
import { NgForOf, SlicePipe } from '@angular/common';
import { SwiperOptions } from 'swiper/types';
import { SwiperDirective } from '../../shared/directives/swiper.directive';
import { MatIcon } from '@angular/material/icon';
import { IMovie } from '../content/interfaces/movie.interface';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [
    MovieCardComponent,
    RouterLink,
    NgForOf,
    SwiperDirective,
    SlicePipe,
    MatTabGroup,
    MatTab,
    MatIcon,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  standalone: true,
})
export class HomeComponent implements OnInit {
  movieTabList = ['Rating', 'Title', 'Year'];
  moviesList: Array<IMovie> = [];
  selectedMovieTab = 0;
  sortOrder = 'asc';
  sortTerm = '';
  pageSize = 20;

  config: SwiperOptions = {
    watchSlidesProgress: true,
    slidesPerView: 7,
    spaceBetween: 30,
    breakpoints: {
      1200: {
        slidesPerView: 7,
        spaceBetween: 30,
      },
      992: {
        slidesPerView: 6.3,
        spaceBetween: 25,
      },
      768: {
        slidesPerView: 4.3,
        spaceBetween: 20,
      },
      576: {
        slidesPerView: 3.3,
        spaceBetween: 15,
      },
      320: {
        slidesPerView: 2.3,
        spaceBetween: 10,
      },
    },
  };

  constructor(private seo: SeoService, private FilmService: FilmService) {}

  ngOnInit() {
    this.seo.generateTags({
      title: 'Angular Movies',
      description: 'Movie Home Page',
    });

    this.getMovies('rating', 1);
  }

  getMovies(sortBy: string, page: number): void {
    this.FilmService.getFilms(
      page,
      this.pageSize,
      this.sortTerm,
      sortBy,
      this.sortOrder
    )
      .pipe(take(1))
      .subscribe((res) => {
        this.moviesList = res.items;
      });
  }

  tabMovieChange({ index }: { index: number }) {
    this.selectedMovieTab = index;
    const movieTypes = ['rating', 'title', 'year'];
    const selectedType = movieTypes[index];
    if (selectedType) {
      this.getMovies(selectedType, 1);
    }
  }

  trackByFn(index: number, item: any) {
    return index;
  }
}
