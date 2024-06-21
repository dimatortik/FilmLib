import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FilmService } from '../../content/services/film.service/movie.service';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { SeoService } from '../../../core/services/seo.service';
import { take } from 'rxjs/operators';
import { IMovie } from '../../content/interfaces/movie.interface';
import { DatePipe, NgForOf, NgIf } from '@angular/common';
import { MatProgressBar } from '@angular/material/progress-bar';
import { CdkDrag, CdkDragHandle } from '@angular/cdk/drag-drop';
import { MovieCardComponent } from '../../../shared/post-card-view/post-card-view.component';
import { ImgMissingDirective } from '../../../shared/directives/img-missing.directive';
import { MatIcon } from '@angular/material/icon';
import { MatButton, MatButtonModule } from '@angular/material/button';
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
import { ActorService } from '../../content/services/actor.service/actor.service';
import { IActor } from '../../content/interfaces/actor.interface';
import { forkJoin } from 'rxjs';
import { AuthService } from '../../auth/services/auth.service/auth.service';

@Component({
  selector: 'app-person',
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
    MatButtonModule,
    MatLabel,
  ],
  templateUrl: './person.component.html',
  styleUrl: './person.component.scss',
})
export class PersonComponent {
  actorId = '';
  editMode = false;
  isAdmin = false;
  contentType = '';
  content: IActor | null = null;
  movies: IMovie[] = [];
  actorForm!: FormGroup;
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private filmService: FilmService,
    private userService: AuthService,
    private actorService: ActorService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.contentType = this.router.url.split('/')[1];

    this.actorForm = this.fb.group({
      actorName: [''],
      actorDescription: [''],
      actorImage: [null],
    });

    this.userService
      .getUserDetails()
      .subscribe((userDetails) => (this.isAdmin = userDetails.role == 'Admin'));
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const id = params['url'];
      this.actorId = id;
      this.getActor(id);
    });
  }

  populateForm() {
    if (this.content) {
      this.actorForm.patchValue({
        actorName: this.content.name,
        actorDescription: this.content.description,
      });
    }
  }

  onPosterFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.actorForm.patchValue({ actorImage: file });
    }
  }

  getActor(id: string) {
    this.isLoading = true;
    forkJoin({
      movies: this.filmService.getFilmsByActorId(id).pipe(take(1)),
      actor: this.actorService.getActorById(id).pipe(take(1)),
    }).subscribe(({ movies, actor }) => {
      this.content = actor;
      this.movies = movies;
      this.isLoading = false;
    });
  }

  onSubmit(): void {
    if (this.actorForm.valid) {
      this.isLoading = true;
      const formData = new FormData();
      Object.keys(this.actorForm.controls).forEach((key) => {
        this.actorForm.get(key)?.value;
      });
      this.actorService.updateActor(this.actorId, formData).subscribe(
        () => {
          this.isLoading = false;
          alert('Successful');
          this.actorForm.reset();
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

  deleteActor() {
    if (this.content) {
      this.actorService.deleteActor(this.actorId).subscribe(() => {
        this.router.navigate(['/home']);
      });
    }
  }
}
