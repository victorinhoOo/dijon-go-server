import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { UploadImageComponent } from '../upload-image/upload-image.component';

@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImageComponent],
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent {

  private _profileForm!: FormGroup;
  selectedImage: any; // Stocke l'image upload

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<ProfileSettingsComponent>) {}

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: ['', Validators.required],
      pwd: ['', Validators.required],
      img: [null],
      email: ['', Validators.required]
    });
  }

  // Getter pour profileForm
  public get profileForm(): FormGroup {
    return this._profileForm;
  }

  // Setter pour profileForm
  public set profileForm(value: FormGroup) {
    this._profileForm = value;
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      console.log(this.profileForm.value);
    }
    this.dialogRef.close();
  }
  
  // Récupère l'image upload
  onImageSelected(image: any) {
    this.selectedImage = image; 
    this.profileForm.patchValue({ img: this.selectedImage }); // Met à jour le formulaire
  }
}
