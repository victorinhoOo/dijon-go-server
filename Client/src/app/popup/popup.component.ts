import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { HttpClientModule } from '@angular/common/http'; 

@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.css']
})
export class PopupComponent {

  // Attributs privés
  private _popupMessage: string = '';
  private _show: boolean = false;
  private _title: string = ''; 
  
  // Getter pour popupMessage
  get popupMessage(): string {
    return this._popupMessage;
  }

  // Setter pour popupMessage
  @Input()
  set popupMessage(value: string) {
    this._popupMessage = value;
  }

  // Getter pour show
  get show(): boolean {
    return this._show;
  }

  // Setter pour show
  @Input()
  set show(value: boolean) {
    this._show = value;
  }

  // Getter pour le titre
  get title(): string {
    return this._title;
  }

  // Setter pour le titre
  @Input()
  set title(value: string) {
    this._title = value;
  }

  // Méthode pour fermer la popup
  closePopup() {
    //on ferme la popup et on reinitialise le message
    this.show = false; 
    this.popupMessage ='';
    window.location.reload();  
  }
}
