import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-upload-image',
  standalone: true,
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent {
  
  private _uploadedImage: any;

  @Output() imageSelected = new EventEmitter<File>();

  // Getter pour uploadedImage
  get uploadedImage(): any {
    return this._uploadedImage;
  }

  // Setter pour uploadedImage
  set uploadedImage(value: any) {
    this._uploadedImage = value;
  }

  fileChange(e: any) {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      this.uploadedImage = URL.createObjectURL(file); // Utilisation du setter
      this.imageSelected.emit(file);  // On envoie le fichier brut
    }
  }
}
