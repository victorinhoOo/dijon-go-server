import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-hamburger-btn',
  standalone: true,
  imports: [CommonModule, NavbarComponent],
  templateUrl: './hamburger-btn.component.html',
  styleUrls: ['./hamburger-btn.component.css']
})

/**
 * Composant du bouton hamburger de la navbar
 */
export class HamburgerBtnComponent {
  @Output() toggleNavbar: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() isNavbar= false; //recoit la valeur de la property isNavbarVisible de app


  /**
   * Evenement qui permet de déclencher l'ouverture/fermeture de la navbar
   */
  public onToggleNavbar(): void {
    this.toggleNavbar.emit(); 
  }
}
