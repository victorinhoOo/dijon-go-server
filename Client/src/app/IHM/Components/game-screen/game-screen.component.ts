import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { GridComponent } from '../grid/grid.component';
import { Game } from '../../../Model/Game';
import { WebsocketService } from '../../../Model/services/websocket.service';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { ActivatedRoute } from '@angular/router';
import {IObserver} from '../../../Model/Observer/IObserver';
import {Observable} from '../../../Model/Observer/Observable';
import {environment} from '../../../environment';
import Swal from 'sweetalert2';
import { IGameBoardDrawer } from '../../IGameBoardDrawer';
import { GameBoardDrawer } from '../../GameBoardDrawer';



const BLACK_STONES_CAPTURED_POSITION = 0
const WHITE_STONES_CAPTURED_POSITION = 1
const PROFILE_PIC_URL = environment.apiUrl + '/profile-pics/';


@Component({
  selector: 'app-game-screen',
  standalone: true,
  imports: [GridComponent],
  templateUrl: './game-screen.component.html',
  styleUrl: './game-screen.component.css'
})
/**
 * Composant de l'écran de jeu
 */
export class GameScreenComponent implements IObserver, AfterViewInit, OnDestroy {
  @ViewChild(GridComponent) gridComponent!: GridComponent;

  private rule: string;
  private playerAvatar: string;
  private playerPseudo: string;
  private game: Game;
  private size: number;
  private boardDrawer: IGameBoardDrawer;

  /**
   * Initialisation des attributs du composant
   * @param websocketService Service de gestion de la websocket
   * @param userCookieService Service de gestion des cookies utilisateur
   * @param route Service de gestion des routes
   */
  public constructor(private websocketService: WebsocketService,private userCookieService: UserCookieService,private route: ActivatedRoute){
    this.size = 0;
    this.playerPseudo = this.userCookieService.getUser()!.Username; // Récupère le nom d'utilisateur et l'avatar pour l'afficher sur la page
    this.playerAvatar = PROFILE_PIC_URL + this.playerPseudo;
    this.rule = '';
    this.game = this.websocketService.getGame();
    this.game.register(this);
    this.boardDrawer = new GameBoardDrawer();
  }

  /**
   * Destruction du composant
   */
  public ngOnDestroy(): void {
    this.game.leaveGame();
  }

  /**
   * Mise en place des écouteurs d'événements sur les boutons, après l'initialisation complète de la page
   */
  public ngAfterViewInit(): void {
    let stones = document.querySelectorAll('.stone, .bigger-stone');
    let stonesArray = Array.from(stones);

    stonesArray.forEach((stone) => {
      // Gestion du clic
      stone.addEventListener('click', () => {
        this.click(stone);
      });
    
      // Gestion de la touche "Entrée"
      stone.addEventListener('keydown', (event) => {
        if (event instanceof KeyboardEvent && event.key === 'Enter') {
          this.click(stone);
        }
      });
    });

    this.initializeRulesInfo();

    let passButton = document.getElementById('pass');
    passButton?.addEventListener('click', () => {
      this.skipTurn();
    });
  }

  /**
   * Initialisation du composant
   */
  public ngOnInit(): void {
    this.size = Number(this.route.snapshot.paramMap.get('size'));
    this.rule = String(this.route.snapshot.paramMap.get('rule'));
    let playerPseudoContainer = document.getElementById('player-pseudo-text');
    let playerAvatarContainer = document.getElementById('player-pic') as HTMLImageElement;
    if(playerPseudoContainer != undefined && playerAvatarContainer != undefined){
      playerPseudoContainer.innerText = this.playerPseudo;
      playerAvatarContainer.src = `${PROFILE_PIC_URL}${this.playerPseudo}`;
    }
  }

  /**
   * Méthode appelée lors de la mise à jour de l'observable game
   * @param object Nouvel état du jeu
   */
  public update(object: Observable): void {
    this.game = object as Game;
    let playerTimerContainer = document.getElementById('player-timer');
    let opponentTimerContainer = document.getElementById('opponent-timer');
    if(playerTimerContainer != undefined && opponentTimerContainer != undefined){
      this.updateTimers(playerTimerContainer, opponentTimerContainer);
    }
    let globalContainer = document.getElementById('global-container');
    if(globalContainer != undefined){
      this.updateHover(globalContainer);
      this.updateBoard();
    }

    let opponentPseudoContainer = document.getElementById('pseudo-text');
    let opponentAvatarContainer = document.getElementById('opponent-pic');
    if(opponentPseudoContainer != undefined && opponentAvatarContainer != undefined){
      this.updateOpponentPseudo(opponentPseudoContainer, opponentAvatarContainer as HTMLImageElement);
    }

    let playerCapturesContainer = document.getElementById('player-score-value');
    let opponentCapturesContainer = document.getElementById('opponent-score-value');
    if(playerCapturesContainer != undefined && opponentCapturesContainer != undefined){
      let captures = this.game.getCaptures();
      if(captures != ""){
        this.updateCaptures(captures, playerCapturesContainer, opponentCapturesContainer);
      }
    }
  }

  private updateOpponentPseudo(opponentPseudoContainer:HTMLElement, opponentAvatarContainer:HTMLImageElement):void{
    let opponentPseudo = this.game.getOpponentPseudo();
    opponentPseudoContainer.innerText = opponentPseudo;
    opponentAvatarContainer.src = `${PROFILE_PIC_URL}${opponentPseudo}`;
  }

  private updateTimers(playerTimerContainer:HTMLElement, opponentTimerContainer:HTMLElement):void{
    let palyerMs = this.game.getPlayerMs();
    let opponentMs = this.game.getOpponentMs();
    let stringPlayerMs = palyerMs.toString();
    let stringOpponentMs = opponentMs.toString();
    let playerTimer = this.msToTimer(stringPlayerMs);
    let opponentTimer = this.msToTimer(stringOpponentMs);
    playerTimerContainer.innerText = playerTimer;
    opponentTimerContainer.innerText = opponentTimer;
  }

  private updateHover(container:HTMLElement):void{
    let stones = document.querySelectorAll(".stone, .bigger-stone");
    let stonesArray = Array.from(stones);
    if(this.game.isPlayerTurn()){
      container.style.cursor = "pointer";
      stonesArray.forEach((stone)=>{
        stone.classList.add("active");
      })
    }else{
      container.style.cursor = "not-allowed";
      stonesArray.forEach((stone)=>{
        stone.classList.remove("active");
      })
    }
  }

  private msToTimer(ms:string):string{
    let totalMs = Number(ms);
    let totalSeconds = Math.floor(totalMs/1000);
    let minutes = Math.floor(totalSeconds/60);
    let seconds = totalSeconds % 60;
    let stringMiniutes = minutes.toString().padStart(2,'0');
    let stringSeconds = seconds.toString().padStart(2,'0');
    let result = `${stringMiniutes}:${stringSeconds}`;
    return result;
  }

  private initializeRulesInfo(): void {
    let ruleText = document.getElementById('rule-text');
    if (this.rule == 'c') {
      ruleText!.append('chinoises');
    }
    if (this.rule == 'j') {
      ruleText!.append('japonaises');
    }
    let ruleButton = document.getElementById('rule-icon') as HTMLButtonElement;

    ruleButton.addEventListener('click', () => {
      if (this.rule === 'c') {
        Swal.fire({
          title: 'Règles Chinoises',
          html: `
            <p>Le calcul du score final pour les règles chinoises compte les territoires et les pierres posées.</p>
            <a href="https://fr.wikipedia.org/wiki/R%C3%A8gles_du_go#R%C3%A8gle_chinoise" target="_blank" style="color: #007bff;">Plus d'informations</a>
          `,
          icon: 'info',
          confirmButtonText: 'Ok',
          customClass: {
            confirmButton: 'custom-ok-button',
          },
        });
      } else if (this.rule === 'j') {
        Swal.fire({
          title: 'Règles Japonaises',
          html: `
            <p>Le calcul du score final pour les règles japonaises compte les territoires et les pierres capturées.</p>
            <a href="https://fr.wikipedia.org/wiki/R%C3%A8gles_du_go#R%C3%A8gle_japonaise" target="_blank" style="color: #007bff;">Plus d'informations</a>
          `,
          icon: 'info',
          confirmButtonText: 'Ok',
          customClass: {
            confirmButton: 'custom-ok-button',
          },
        });
      }
    });
  }

  /**
   * Gère le clic ou l'appui sur "Entrée" sur une intersection de la grille
   * @param stone emplacement concerné par l'action
   */
  public click(stone: any): void {
    this.websocketService.placeStone(stone.id);
  }

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
        cancelButton: 'custom-no-button',
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
          icon: 'success',
        }).fire({
          title: 'Tour passé',
        });
      }
    });
  }

  private updateBoard() {
    let board = this.game.getBoard();
    this.boardDrawer.drawBoardState(board);
  }

  private updateCaptures(captures: string, playerCapturesContainer: HTMLElement, opponentCapturesContainer: HTMLElement): void {
    let playerCaptures;
    let opponentCaptures;
    if (this.game.getPlayerColor() == 'black') { 
      playerCaptures = captures.split(';')[WHITE_STONES_CAPTURED_POSITION];
      opponentCaptures = captures.split(';')[BLACK_STONES_CAPTURED_POSITION];
    } else {
      playerCaptures = captures.split(';')[BLACK_STONES_CAPTURED_POSITION];
      opponentCaptures = captures.split(';')[WHITE_STONES_CAPTURED_POSITION];
    }

    document.getElementById('opponent-score-value')!.innerHTML =
      'Prises : ' + opponentCaptures;
    document.getElementById('player-score-value')!.innerHTML =
      'Prises : ' + playerCaptures;
  }
}
