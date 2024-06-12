import {Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {PaginationModel} from '../../../core/models/pagination.model';
import {FilmService} from '../services/movie.service/movie.service'; // замените на ваш сервис
import {ActivatedRoute, Router} from '@angular/router';
import {DomSanitizer} from '@angular/platform-browser';
import {SeoService} from '../../../core/services/seo.service';
import {take} from 'rxjs/operators';
import {IMovie} from "../../content/interfaces/movie.interface";
import {IContent} from "../../content/interfaces/content.interface";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {MatProgressBar} from "@angular/material/progress-bar";
import {CdkDrag, CdkDragHandle} from "@angular/cdk/drag-drop";
import {MovieCardComponent} from "../../../shared/post-card-view/post-card-view.component";
import {ImgMissingDirective} from "../../../shared/directives/img-missing.directive";
import {MatIcon} from "@angular/material/icon";
import {MatButton} from "@angular/material/button";
import {MatDialog, MatDialogContent, MatDialogTitle} from "@angular/material/dialog";

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss'],
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
    MatDialogTitle
  ],
  standalone: true
})
export class DetailComponent implements OnInit {

  contentType = '';
  content: IMovie | null = null;
  isLoading = true;

  @ViewChild('matTrailerDialog') matTrailerDialog!: TemplateRef<any>;

  constructor(
    private filmService: FilmService, 
    private route: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer,
    private seo: SeoService,
    public trailerDialog: MatDialog
  ) {
    this.contentType = this.router.url.split('/')[1];
  }

  ngOnInit() {
    this.route.params.subscribe(
      params => {
        const id = params['url'];

        if (this.contentType === 'movies') {
          this.getMovie(id);
        }
      }
    );
  }

  getMovie(id: string) {
    this.isLoading = true;

    this.filmService.getFilm(id).pipe(take(1)).subscribe(
      movie => {
        this.content = movie;
        this.seo.generateTags({
          title: movie.title,
          description: movie.description,
          image: movie.titleImageLink
        });
        
        this.isLoading = false;
      }
    );
  }

  openDialog(): void {
    const dialogRef = this.trailerDialog.open(this.matTrailerDialog, {});
    dialogRef.disableClose = false;
  }
}