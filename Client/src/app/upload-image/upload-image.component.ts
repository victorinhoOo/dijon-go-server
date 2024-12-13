import { Component, EventEmitter, Output } from '@angular/core';

const FIRST_FILE_INDEX = 0;

@Component({
  selector: 'app-upload-image',
  standalone: true,
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
/**
 * Composant pour télécharger une image
 */
export class UploadImageComponent {
  
  // Variable privée pour stocker l'image téléchargée 
  private _uploadedImage: any;

  // EventEmitter utilisé pour notifier le parent quand une image est sélectionnée
  @Output() imageSelected = new EventEmitter<File>();

  /**
   * Getter pour récupérer l'image téléchargée
   * @returns l'image téléchargée
   */
  get uploadedImage(): any {
    return this._uploadedImage;
  }

  /**
   * Setter pour définir l'image téléchargée
   * @param value l'image téléchargée à stocker
   */
  set uploadedImage(value: any) {
    this._uploadedImage = value;
  }

  /**
   * Méthode appelée lors d'un changement de fichier dans l'input
   * Elle convertit le fichier sélectionné en URL pour l'afficher et émet l'image brute
   * @param e Événement déclenché lors du changement de fichier (Event)
   */
  public fileChange(e: any) {
    // Vérifie si un fichier a bien été sélectionné
    if (e.target.files && e.target.files[FIRST_FILE_INDEX]) {
      const file = e.target.files[FIRST_FILE_INDEX];
      // Génère une URL pour prévisualiser l'image
      this.uploadedImage = URL.createObjectURL(file); // Utilisation du setter
      // Émet le fichier sélectionné brut via EventEmitter
      this.imageSelected.emit(file);  
    }
  }
}
