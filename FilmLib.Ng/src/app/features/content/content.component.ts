// content.component.ts
import {Component, OnInit} from '@angular/core';
import {PaginationModel} from '../../core/models/pagination.model';
import { FilmService } from './services/movie.service/movie.service';
import {take} from 'rxjs/operators';
import {Router} from "@angular/router";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MovieCardComponent} from "../../shared/post-card-view/post-card-view.component";
import {MatButtonModule} from "@angular/material/button";
import {MatCardModule} from "@angular/material/card";
import {NgForOf, TitleCasePipe} from "@angular/common";
import { IMovie } from './interfaces/movie.interface';

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
    NgForOf
  ],
  standalone: true
})
export class ContentComponent implements OnInit {

  sortBy = 'title';
  sortOrder = 'asc';
  films: Array<IMovie> = [];
  pageSize = 20;
  page = 1;

  totalResults: any;

  constructor(
    private filmService: FilmService,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.getMovies();

  }

  getMovies() {
    this.filmService.getFilms(this.page, this.pageSize, this.sortBy, this.sortOrder).pipe(take(1)).subscribe((response: PaginationModel<IMovie>) => {
      this.films = response.items;
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

  changePage(event : any) {
    this.page = event.page + 1;
    this.pageSize = event.pageSize;
    this.getMovies(); // Remove the arguments event.pageIndex and event.pageSize
  }

}