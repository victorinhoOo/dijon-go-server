import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-upload-image',
  standalone: true,
  imports: [],
  templateUrl: './upload-image.component.html',
  styleUrl: './upload-image.component.css'
})
export class UploadImageComponent {

  uploadedImage:any;

  @Output() imageSelected = new EventEmitter<any>();

  fileChange(e: any) {
    if (e.target.files && e.target.files[0]) {
      const reader = new FileReader();
      
      reader.onload = (event: any) => {
        this.uploadedImage = event.target.result;  // Conversion en base64
        this.imageSelected.emit(this.uploadedImage);  // envoye l'image a profile-setting
      };

      reader.readAsDataURL(e.target.files[0]);
    }
  }

}
