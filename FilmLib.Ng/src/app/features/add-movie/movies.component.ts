import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ActorService } from '../content/services/actor.service/actor.service';
import { GenreService } from '../content/services/genre.service/genre.service';
import { FilmService } from '../content/services/film.service/movie.service';
import { ActorModel } from '../content/models/actor.model';
import { GenreModel } from '../content/models/genre.model';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddActorComponent } from './add-actor/add-actor.component';
import { AddGenreComponent } from './add-genre/add-genre.component';
import { SpinnerComponent } from '../../core/components/spinner/spinner.component';

@Component({
  selector: 'app-add-movie',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    NgxMatSelectSearchModule,
    MatDialogModule,
    SpinnerComponent,
  ],
})
export class AddMovieComponent implements OnInit {
  movieForm: FormGroup;
  actorSearchCtrl = new FormControl();
  genreSearchCtrl = new FormControl();
  filteredActors: Observable<ActorModel[]>;
  filteredGenres: Observable<GenreModel[]>;
  actors: ActorModel[] = [];
  genres: GenreModel[] = [];
  isLoading = false;
  constructor(
    private fb: FormBuilder,
    private actorService: ActorService,
    private genreService: GenreService,
    private filmService: FilmService,
    private dialog: MatDialog
  ) {
    this.movieForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      director: ['', Validators.required],
      year: ['', Validators.required],
      country: ['', Validators.required],
      titleImage: [null, Validators.required],
      filmVideo: [null, Validators.required],
      actors: [[], Validators.required],
      genres: [[], Validators.required],
    });

    this.filteredActors = this.actorSearchCtrl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterActors(value))
    );

    this.filteredGenres = this.genreSearchCtrl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterGenres(value))
    );
  }

  ngOnInit(): void {
    this.actorService.getActors().subscribe((actors) => {
      this.actors = actors;
      this.filteredActors = this.actorSearchCtrl.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterActors(value))
      );
    });

    this.genreService.getGenres().subscribe((genres) => {
      this.genres = genres;
      this.filteredGenres = this.genreSearchCtrl.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterGenres(value))
      );
    });
  }

  get actorNames(): string {
    const actors = this.movieForm.get('actors')?.value;
    return actors
      ? actors.map((actor: ActorModel) => actor.name).join(', ')
      : '';
  }

  get genreNames(): string {
    const genres = this.movieForm.get('genres')?.value;
    return genres
      ? genres.map((genre: GenreModel) => genre.title).join(', ')
      : '';
  }

  private _filterActors(value: string): ActorModel[] {
    const filterValue = value.toLowerCase();
    return this.actors.filter((actor) =>
      actor.name.toLowerCase().includes(filterValue)
    );
  }

  private _filterGenres(value: string): GenreModel[] {
    const filterValue = value.toLowerCase();
    return this.genres.filter((genre) =>
      genre.title.toLowerCase().includes(filterValue)
    );
  }

  onPosterFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.movieForm.patchValue({ titleImage: file });
    }
  }

  onMovieFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.movieForm.patchValue({ filmVideo: file });
    }
  }

  onSubmit(): void {
    if (this.movieForm.valid) {
      this.isLoading = true;
      const formData = new FormData();
      Object.keys(this.movieForm.controls).forEach((key) => {
        let controlValue = this.movieForm.get(key)?.value;
        if (key === 'actors') {
          controlValue = controlValue.map((actor: ActorModel) => actor.id);
          controlValue.forEach((value: string) => {
            formData.append(`${key}[]`, value);
          });
        } else if (key === 'genres') {
          controlValue = controlValue.map((genre: GenreModel) => genre.id);
          controlValue.forEach((value: string) => {
            formData.append(`${key}[]`, value);
          });
        } else {
          formData.append(key, controlValue);
        }
      });
      this.filmService.addFilm(formData).subscribe(
        () => {
          this.isLoading = false;
          alert('Successful');
          this.movieForm.reset();
        },
        (error) => {
          alert(error);
          this.isLoading = false;
        }
      );
    }
  }
  openAddActorDialog(): void {
    const dialogRef = this.dialog.open(AddActorComponent, {
      width: '400px',
      data: {},
    });

    dialogRef.afterClosed().subscribe((result: ActorModel | undefined) => {
      if (result) {
        this.actors.push(result);

        this.movieForm
          .get('actors')
          ?.setValue([...this.movieForm.get('actors')?.value, result]);

        this.actorService.getActors().subscribe((actors) => {
          this.actors = actors;
          this.filteredActors = this.actorSearchCtrl.valueChanges.pipe(
            startWith(''),
            map((value) => this._filterActors(value))
          );
        });
      }
    });
  }

  openAddGenreDialog(): void {
    const dialogRef = this.dialog.open(AddGenreComponent, {
      width: '400px',
      data: {},
    });

    dialogRef.afterClosed().subscribe((result: GenreModel | undefined) => {
      if (result) {
        this.genres.push(result);

        this.movieForm
          .get('genres')
          ?.setValue([...this.movieForm.get('genres')?.value, result]);

        this.genreService.getGenres().subscribe((genres) => {
          this.genres = genres;
          this.filteredGenres = this.genreSearchCtrl.valueChanges.pipe(
            startWith(''),
            map((value) => this._filterGenres(value))
          );
        });
      }
    });
  }
}
