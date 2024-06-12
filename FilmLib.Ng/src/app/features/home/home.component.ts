
import {Component, OnInit} from '@angular/core';
import {FilmService} from '../content/services/movie.service/movie.service';
import {SeoService} from '../../core/services/seo.service';
import {take} from 'rxjs/operators';
import {MatTab, MatTabGroup} from "@angular/material/tabs";
import {MovieCardComponent} from "../../shared/post-card-view/post-card-view.component";
import {RouterLink} from "@angular/router";
import {NgForOf, SlicePipe} from "@angular/common";
import {SwiperOptions} from "";
import {SwiperDirective} from "../../shared/directives/swiper.directive";
import {MatIcon} from "@angular/material/icon";
import { IMovie } from '../content/interfaces/movie.interface';

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
  standalone: true
})

export class HomeComponent implements OnInit {

  config: SwiperOptions = {
    watchSlidesProgress: true,
    breakpoints: {
      992: {slidesPerView: 6.3, spaceBetween: 20, slidesOffsetBefore: 0, slidesOffsetAfter: 0},
      768: {slidesPerView: 4.3, spaceBetween: 15, slidesOffsetBefore: 0, slidesOffsetAfter: 0},
      576: {slidesPerView: 3.3, spaceBetween: 15, slidesOffsetBefore: 0, slidesOffsetAfter: 0},
      320: {slidesPerView: 2.3, spaceBetween: 10, slidesOffsetBefore: 10, slidesOffsetAfter: 10},
    }
  };

  movieTabList = ['Rating', 'Title', 'Year'];
  moviesList: Array<IMovie> = [];
  selectedMovieTab = 0;
  sortOrder = 'asc';
  sortTerm = '';
  pageSize = 20;


  constructor(
    private seo: SeoService,
    private FilmService: FilmService
  ) {}

  ngOnInit() {
    this.seo.generateTags({
      title: 'Angular Movies and Series',
      description: 'Movie and Series Home Page',
      image: 'https://jancobh.github.io/Angular-Movies/background-main.webp'
    });

    this.getMovies('rating', 1);
  }

  getMovies(sortBy: string, page: number): void {
    this.FilmService.getFilms(
          page, 
          this.pageSize, 
          this.sortTerm,
          sortBy, 
          this.sortOrder)
          .pipe(take(1))
          .subscribe(res => {
      this.moviesList = res.items;
    });
  }

  tabMovieChange({ index }: { index: number; }) {
    this.selectedMovieTab = index;
    const movieTypes = ['rating', 'title', 'year'];
    const selectedType = movieTypes[index];
    if (selectedType) {
      this.getMovies(selectedType, 1);
    }
  }

  // getTVShows(type: string, page: number): void {
  //   this.onTvService.getTVShows(type, page).subscribe(res => {
  //     this.tvShowsList = res.results;
  //   });
  // }

  // tabTVChange({ index }: { index: number; }) {
  //   this.selectedTVTab = index;
  //   const tvShowTypes = ['airing_today', 'on_the_air', 'popular'];
  //   const selectedType = tvShowTypes[index];
  //   if (selectedType) {
  //     this.getTVShows(selectedType, 1);
  //   }
  // }

}
