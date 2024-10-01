import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; // ReactiveFormsModule
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

  profileForm!: FormGroup;

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<ProfileSettingsComponent>) {}

  selectedImage : any; //stock l'image upload

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: ['', Validators.required],
      pwd: ['',Validators.required],
      img:[null]
      
    });
    
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      
      console.log(this.profileForm.value);  
    }
    this.dialogRef.close();
  }
  
  //recupere l'image upload
  onImageSelected(image: any) {
    this.selectedImage = image; 
    this.profileForm.patchValue({ img: this.selectedImage }); //update dans le formulaire
  }
  
}
