import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FilmService } from '../content/services/film.service/movie.service';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { IMovie } from '../content/interfaces/movie.interface';
import { DatePipe, NgForOf, NgIf } from '@angular/common';
import { MatProgressBar } from '@angular/material/progress-bar';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
import { MovieCardComponent } from '../../shared/post-card-view/post-card-view.component';
import { ImgMissingDirective } from '../../shared/directives/img-missing.directive';
import { MatIcon } from '@angular/material/icon';
import { MatDialogContent, MatDialogTitle } from '@angular/material/dialog';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  MatCard,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { forkJoin } from 'rxjs';
import { IGenre } from '../content/interfaces/genre.interface';
import { GenreService } from '../content/services/genre.service/genre.service';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../auth/services/auth.service/auth.service';

@Component({
  selector: 'app-genres',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    DatePipe,
    CdkDrag,
    CdkDragHandle,
    MovieCardComponent,
    ImgMissingDirective,
    MatProgressBar,
    MatIcon,
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
    MatLabel,
    MatButtonModule,
  ],
  templateUrl: './genres.component.html',
  styleUrl: './genres.component.scss',
})
export class GenresComponent {
  contentType = '';
  genreId!: number;
  content: IGenre | null = null;
  movies: IMovie[] = [];
  isLoading = true;
  isAdmin = false;
  editMode = false;
  genreForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: AuthService,
    private filmService: FilmService,
    private genreService: GenreService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.contentType = this.router.url.split('/')[1];

    this.genreForm = this.fb.group({
      title: [''],
      description: [''],
    });

    this.userService
      .getUserDetails()
      .subscribe((userDetails) => (this.isAdmin = userDetails.role == 'Admin'));
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const id = params['url'];
      this.genreId = id;
      this.getGenre(id);
    });
  }

  populateForm() {
    if (this.content) {
      this.genreForm.patchValue({
        title: this.content.title,
        description: this.content.description,
      });
    }
  }

  getGenre(id: number) {
    this.isLoading = true;
    forkJoin({
      movies: this.filmService.geFilmsByGenreId(id).pipe(take(1)),
      genre: this.genreService.getGenre(id).pipe(take(1)),
    }).subscribe(({ movies, genre }) => {
      this.content = genre;
      this.movies = movies;
      this.isLoading = false;
    });
  }

  onSubmit(): void {
    if (this.genreForm.valid) {
      this.isLoading = true;
      const formData = new FormData();
      Object.keys(this.genreForm.controls).forEach((key) => {
        this.genreForm.get(key)?.value;
      });
      this.genreService.updateGenre(this.genreId, formData).subscribe(
        () => {
          this.isLoading = false;
          alert('Successful');
          this.genreForm.reset();
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

  toggleEditMode() {
    this.editMode = !this.editMode;
    this.populateForm();
  }

  deleteGenre() {
    if (this.content) {
      this.genreService.deleteGenre(this.genreId).subscribe(() => {
        this.router.navigate(['/home']);
      });
    }
  }
}
