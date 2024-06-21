import { Component, Inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { GenreService } from '../../content/services/genre.service/genre.service';
import { error } from 'console';

@Component({
  selector: 'app-add-actor',
  templateUrl: './add-genre.component.html',
  styleUrls: ['./add-genre.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
})
export class AddGenreComponent {
  genreForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddGenreComponent>,
    private genreService: GenreService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.genreForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.genreForm.valid) {
      const formData = new FormData();
      Object.keys(this.genreForm.controls).forEach((key) => {
        formData.append(key, this.genreForm.get(key)?.value);
      });
      this.genreService.addGenre(formData).subscribe(
        () => {
          alert('Successful');
          this.dialogRef.close(this.genreForm.value);
        },
        (error) => alert(`${error.message}`)
      );
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
