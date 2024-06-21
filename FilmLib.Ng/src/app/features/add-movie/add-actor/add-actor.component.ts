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
import { ActorService } from '../../content/services/actor.service/actor.service';
import { error } from 'console';

@Component({
  selector: 'app-add-actor',
  templateUrl: './add-actor.component.html',
  styleUrls: ['./add-actor.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
})
export class AddActorComponent {
  actorForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddActorComponent>,
    private ActorService: ActorService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.actorForm = this.fb.group({
      actorName: ['', Validators.required],
      actorDescription: ['', Validators.required],
      actorImage: [null, Validators.required],
    });
  }

  onPhotoFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.actorForm.patchValue({ actorImage: file });
    }
  }

  onSubmit(): void {
    if (this.actorForm.valid) {
      const formData = new FormData();
      Object.keys(this.actorForm.controls).forEach((key) => {
        formData.append(key, this.actorForm.get(key)?.value);
      });
      this.ActorService.addActor(formData).subscribe(
        () => {
          alert('Successful');
          this.dialogRef.close(this.actorForm.value);
        },
        (error) => alert(`${error.message}`)
      );
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
