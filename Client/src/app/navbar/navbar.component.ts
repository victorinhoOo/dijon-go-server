import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Output,EventEmitter } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule,CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {

   // Émettre un événement pour notifier la fermeture
   @Output() closeNavbar = new EventEmitter<void>();

  // Attribut privé pour la visibilité de la navbar
  private isNavbarVisible: boolean = true;

  // Getter pour obtenir l'état de la navbar
  public get IsNavbarVisible(): boolean {
    return this.isNavbarVisible;
  }

  // Setter pour modifier l'état de la navbar
  public set SetIsNavbarVisible(value: boolean) {
    this.isNavbarVisible = value;
  }
  
  private lightState: string;

  public constructor() {
    this.lightState = 'light';
    this.isNavbarVisible = true;
  }

  close(): void {
    this.isNavbarVisible = false;
    this.closeNavbar.emit(); // Émettre l'événement pour signaler la fermeture
  }

  toggleNavbar(): void {
    this.isNavbarVisible = !this.isNavbarVisible;
  }

 
}