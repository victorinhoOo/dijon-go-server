import { Component, AfterViewInit, OnInit } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { WebsocketService } from '../websocket.service';
import { UserCookieService } from '../Model/UserCookieService';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css',
})
/**
 * Composant de la grille de jeu
 */
export class GridComponent implements AfterViewInit, OnInit {
  private size: number;
  private playerAvatar: string;
  private playerPseudo: string;

  public get PlayerAvatar() {
    return this.playerAvatar;
  }

  public get PlayerPseudo() {
    return this.playerPseudo;
  }

  public constructor(
    private websocketService: WebsocketService,
    private userCookieService: UserCookieService
  ) {
    this.size = 0;
    this.playerPseudo = this.userCookieService.getUser()!.Username; // Récupère le nom d'utilisateur et l'avatar pour l'afficher sur la page
    this.playerAvatar =
      'https://localhost:7065/profile-pics/' + this.playerPseudo;
  }

  /**
   * Renvoi la taille de la grille
   * @returns La taille de la grille
   */
  public getSize(): number {
    return this.size - 1;
  }

  /**
   * Initialisation du composant
   */
  public ngOnInit(): void {
    this.size = 19;
  }

  /**
   * Mise en place des écouteurs d'événements sur les boutons, après l'initialisation complète de la page
   */
  public ngAfterViewInit(): void {
    let stones = document.getElementsByClassName('stone');
    let stonesArray = Array.from(stones);
    stonesArray.forEach((stone) => {
      stone.addEventListener('click', () => {
        this.click(stone);
      });
    });

    let passButton = document.getElementById('pass');
    passButton?.addEventListener('click', () => {
      this.skipTurn();
    });
  }

  /**
   * Gère le clique sur les intersections de la grille
   * @param stone emplacement concerné par le clic
   */
  public click(stone: any): void {
    this.websocketService.placeStone(stone.id);
  }
  
  /**
   * Gère le clic sur le bouton "Passer", passe le tour du joueur
   */
/**
 * Gère le clic sur le bouton "Passer", passe le tour du joueur après confirmation
 */
public skipTurn() {
  Swal.fire({
    title: 'Voulez-vous vraiment passer votre tour ?',
    text: 'Un tour passé peut être synonyme de fin de partie.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Oui',
    cancelButtonText: 'Non',
    customClass: {
      confirmButton: 'custom-yes-button',
      cancelButton: 'custom-no-button'
    },
  }).then((result) => {
    if (result.isConfirmed) {
      // Passe le tour du joueur
      this.websocketService.skipTurn();

      // Affiche le message "Tour passé"
      Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 2000,
        icon: 'success'
      }).fire({
        title: 'Tour passé'
      });
    }
  });
}

}
