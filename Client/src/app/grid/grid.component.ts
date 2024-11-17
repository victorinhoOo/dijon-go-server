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

  private rule: string;
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
    private userCookieService: UserCookieService,
    private route: ActivatedRoute
  ) {
    this.size = 0;
    this.playerPseudo = this.userCookieService.getUser().Username; // Récupère le nom d'utilisateur et l'avatar pour l'afficher sur la page
    this.playerAvatar =
      'https://localhost:7065/profile-pics/' + this.playerPseudo;

    this.rule = '';
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
    this.size = Number(this.route.snapshot.paramMap.get('size'));
    this.rule = String(this.route.snapshot.paramMap.get('rule'));
  }

  /**
   * Mise en place des écouteurs d'événements sur les boutons, après l'initialisation complète de la page
   */
  public ngAfterViewInit(): void {
    if(this.size < 11){
      let cells = document.querySelectorAll('.cell, .cell-bottom');
      let stones = document.getElementsByClassName('stone');
      let arrayCells = Array.from(cells);
      let arrayStones = Array.from(stones);
      arrayCells.forEach((cell) => {
        cell.classList.remove(cell.classList[0]);
        cell.classList.add('bigger-cell');
      });
      arrayStones.forEach((stone) => {
        stone.classList.remove('stone');
        stone.classList.add('bigger-stone');
      }); 
    }
    let stones = document.querySelectorAll('.stone, .bigger-stone');
    let stonesArray = Array.from(stones);
    stonesArray.forEach((stone) => {
      stone.addEventListener('click', () => {
        this.click(stone);
      });
    });

    this.initializeRulesInfo();
    

    let passButton = document.getElementById('pass');
    passButton?.addEventListener('click', () => {
      this.skipTurn();
    });
  }

  private initializeRulesInfo(): void{
    let ruleText = document.getElementById('rule-text');
    if(this.rule == 'c'){
      ruleText!.append('chinoises')
    }
    if(this.rule == 'j'){
      ruleText!.append('japonaises')
    }
    let ruleButton = document.getElementById('rule-icon') as HTMLButtonElement;

    ruleButton.addEventListener('click', () => {
      if (this.rule === 'c') {
        Swal.fire({
          title: 'Règles Chinoises',
          text: 'Les règles chinoises se basent sur le territoire et les pierres posées.',
          icon: 'info',
          confirmButtonText: 'Ok',
          customClass: {
            confirmButton: 'custom-ok-button'
          },
        });
      } else if (this.rule === 'j') {
        Swal.fire({
          title: 'Règles Japonaises',
          text: 'Les règles japonaises comptent les territoires et les pierres capturées.',
          icon: 'info',
          confirmButtonText: 'Ok',
          customClass: {
            confirmButton: 'custom-ok-button'
          },
        });
      }
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
  public skipTurn() {
    this.websocketService.skipTurn();
  }
}
