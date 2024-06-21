import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FilmService } from '../services/film.service/movie.service';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { take } from 'rxjs/operators';
import { IMovie } from '../../content/interfaces/movie.interface';
import { CommonModule, DatePipe, NgForOf, NgIf } from '@angular/common';
import { MatProgressBar } from '@angular/material/progress-bar';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
import { MovieCardComponent } from '../../../shared/post-card-view/post-card-view.component';
import { ImgMissingDirective } from '../../../shared/directives/img-missing.directive';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import {
  MatDialog,
  MatDialogContent,
  MatDialogTitle,
} from '@angular/material/dialog';
import { GenreService } from '../services/genre.service/genre.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { FilmCommentService } from '../services/comment.service/comment.service';
import {
  MatCard,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { ActorService } from '../services/actor.service/actor.service';
import { forkJoin } from 'rxjs';
import { CommentModel } from '../models/comment.model';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { Observable } from 'rxjs';
import { ActorModel } from '../models/actor.model';
import { GenreModel } from '../models/genre.model';
import { startWith, map } from 'rxjs/operators';
import { AddActorComponent } from '../../add-movie/add-actor/add-actor.component';
import { AddGenreComponent } from '../../add-movie/add-genre/add-genre.component';
import { AuthService } from '../../auth/services/auth.service/auth.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss'],
  imports: [
    CommonModule,
    NgForOf,
    NgIf,
    DatePipe,
    CdkDrag,
    CdkDragHandle,
    MovieCardComponent,
    ImgMissingDirective,
    MatProgressBar,
    MatIcon,
    MatButton,
    MatDialogContent,
    MatDialogTitle,
    MatCard,
    MatCardTitle,
    MatCardSubtitle,
    MatCardContent,
    MatCardHeader,
    MatFormField,
    MatInput,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatLabel,
    MatOptionModule,
    MatSelectModule,
  ],
  standalone: true,
})
export class DetailComponent implements OnInit {
  contentType = '';
  content: IMovie | null = null;
  genres: GenreModel[] = [];
  comments: CommentModel[] = [];
  actors: ActorModel[] = [];
  isLoading = true;
  newCommentBody = new FormControl('');
  commentText = '';
  movieForm!: FormGroup;
  filmId: string = '';
  isAdmin = false;
  editMode = false;
  actorSearchCtrl = new FormControl();
  genreSearchCtrl = new FormControl();
  filteredActors!: Observable<ActorModel[]>;
  filteredGenres!: Observable<GenreModel[]>;
  allActors: ActorModel[] = [];
  allGenres: GenreModel[] = [];

  @ViewChild('matTrailerDialog') matTrailerDialog!: TemplateRef<any>;

  constructor(
    private fb: FormBuilder,
    private userService: AuthService,
    private filmService: FilmService,
    private genreService: GenreService,
    private actorService: ActorService,
    private commentService: FilmCommentService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog
  ) {
    this.contentType = this.router.url.split('/')[1];

    this.movieForm = this.fb.group({
      title: [''],
      description: [''],
      director: [''],
      year: [''],
      country: [''],
      titleImage: [null],
      filmVideo: [null],
      actors: [[]],
      genres: [[]],
    });

    this.filteredActors = this.actorSearchCtrl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterActors(value))
    );

    this.filteredGenres = this.genreSearchCtrl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterGenres(value))
    );

    this.userService
      .getUserDetails()
      .subscribe((userDetails) => (this.isAdmin = userDetails.role == 'Admin'));
  }

  ngOnInit(): any {
    this.route.params.subscribe((params) => {
      const id = params['url'];
      this.filmId = id;
      this.getMovie(id);
    });

    this.actorService.getActors().subscribe((actors) => {
      this.allActors = actors;
      this.filteredActors = this.actorSearchCtrl.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterActors(value))
      );
    });

    this.genreService.getGenres().subscribe((genres) => {
      this.allGenres = genres;
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

  private _filterActors(value: string): ActorModel[] {
    const filterValue = value.toLowerCase();
    return this.allActors.filter((actor) =>
      actor.name.toLowerCase().includes(filterValue)
    );
  }

  private _filterGenres(value: string): GenreModel[] {
    const filterValue = value.toLowerCase();
    return this.allGenres.filter((genre) =>
      genre.title.toLowerCase().includes(filterValue)
    );
  }

  get genreNames(): string {
    const genres = this.movieForm.get('genres')?.value;
    return genres
      ? genres.map((genre: GenreModel) => genre.title).join(', ')
      : '';
  }

  getMovie(id: string) {
    this.isLoading = true;

    forkJoin({
      movie: this.filmService.getFilm(id).pipe(take(1)),
      genres: this.genreService.getGenresByFilm(id).pipe(take(1)),
      comments: this.commentService.getComments(id).pipe(take(1)),
      actors: this.actorService.getActorsByFilmId(id).pipe(take(1)),
    }).subscribe(({ movie, genres, comments, actors }) => {
      this.content = movie;
      this.genres = genres;
      this.comments = comments;
      this.actors = actors;
      this.isLoading = false;
      this.populateForm();
    });
  }

  populateForm() {
    if (this.content) {
      this.movieForm.patchValue({
        title: this.content.title,
        description: this.content.description,
        year: this.content.year,
        director: this.content.director,
        country: this.content.country,
        genres: this.genres,
        actors: this.actors,
      });
    }
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
      this.filmService.updateFilm(this.filmId, formData).subscribe(
        () => {
          this.isLoading = false;
          alert('Successful');
          this.movieForm.reset();
          this.ngOnInit();
          this.editMode = false;
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
        this.allActors.push(result);

        this.movieForm
          .get('actors')
          ?.setValue([...this.movieForm.get('actors')?.value, result]);

        this.actorService.getActors().subscribe((actors) => {
          this.allActors = actors;
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
        this.allGenres.push(result);

        this.movieForm
          .get('genres')
          ?.setValue([...this.movieForm.get('genres')?.value, result]);

        this.genreService.getGenres().subscribe((genres) => {
          this.allGenres = genres;
          this.filteredGenres = this.genreSearchCtrl.valueChanges.pipe(
            startWith(''),
            map((value) => this._filterGenres(value))
          );
        });
      }
    });
  }

  toggleEditMode() {
    this.editMode = !this.editMode;
  }

  addComment() {
    const commentBody = this.newCommentBody.value;
    if (commentBody) {
      this.commentService
        .addComment(this.content!.id, commentBody)
        .subscribe(() => {
          this.commentService
            .getComments(this.content!.id)
            .pipe(take(1))
            .subscribe((comments) => {
              this.comments = comments;
            });
          this.newCommentBody.setValue('');
        });
    }
  }

  deleteMovie() {
    if (this.content) {
      this.filmService.deleteFilm(this.content.id).subscribe(() => {
        this.router.navigate(['/movies']);
      });
    }
  }
}
