import { AfterViewInit, Component, OnInit, ComponentFactoryResolver, ApplicationRef, Injector, EmbeddedViewRef, ComponentRef } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { ActivatedRoute, Router } from '@angular/router';
import { GameDAO } from '../../../Model/DAO/GameDAO';
import { HttpClient } from '@angular/common/http';
import { AvailableGameInfoDTO } from '../../../Model/DTO/AvailableGameInfoDTO';
import { WebsocketService } from '../../../Model/services/websocket.service';
import { HttpClientModule } from '@angular/common/http';
import Swal from 'sweetalert2';
import { User } from '../../../Model/User';
import { UserDAO } from '../../../Model/DAO/UserDAO';
import { RankprogressComponent } from "../rankprogress/rankprogress.component";
import { CommonModule } from '@angular/common';
import { PlayerListComponent } from '../player-list/player-list.component';
import { firstValueFrom } from 'rxjs';

/**
 * Composant de la page d'accueil
 */
@Component({
  selector: 'app-index',
  standalone: true,
  imports: [
    NavbarComponent, 
    MatIcon, 
    HttpClientModule, 
    RankprogressComponent, 
    CommonModule,
    PlayerListComponent
  ],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css'],
})
export class IndexComponent implements OnInit, AfterViewInit {
  private token: string;
  private userPseudo: string;
  private avatar: string;
  private userRank: string;
  private gameDAO: GameDAO;
  private userDAO: UserDAO;


  /**
   * Getter pour l'avatar de l'utilisateur
   */
  public get Avatar(): string {
    return this.avatar;
  }
  /**
   * Getter pour le pseudo de l'utilisateur
   */
  public get UserPseudo(): string {
    return this.userPseudo;
  }

  /**
   * Getter pour le rang de l'utilisateur
   */
  public get UserRank(): string {
    return this.userRank;
  }

  constructor(
    private userCookieService: UserCookieService,
    private router: Router,
    private httpClient: HttpClient,
    private websocketService: WebsocketService,
    private route: ActivatedRoute,
    private componentFactoryResolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector
  ) {
    this.avatar = 'https://localhost:7065/profile-pics/';
    this.token = '';
    this.userPseudo = '';
    this.gameDAO = new GameDAO(httpClient);
    this.userRank = '';
    this.userDAO = new UserDAO(httpClient);
  }

  /**
   * Initialise les informations utilisateurs, le leaderboard, et gère la création de parties
   */
  public async ngOnInit() {
    // Vérifiez et gérez la connexion
    this.token = this.userCookieService.getToken();
    if (!this.token) {
      this.router.navigate(['/login']);
    }
    if(!this.router.url.includes('cancelled')){ // si une recherche n'a pas été annulée, on se déco / reco en arrivant sur la page d'accueil
      if(!this.websocketService.isWebsocketConnected()){
        this.websocketService.disconnectWebsocket();
        this.websocketService.connectWebsocket();
      }
    }
    this.userCookieService.getUserObservable().subscribe((user: User | null) => {
      if (user) {
        this.userPseudo = user.Username;
        this.userRank = user.getRank();
        this.avatar = `https://localhost:7065/profile-pics/${this.userPseudo}`;
      }
    });
    this.populateLeaderboard();
  }

  /**
   * Gère la création des boutons
   */
  public ngAfterViewInit() {
    const joinGamesLink = document.getElementById('joinGames');
    if (joinGamesLink) {
      joinGamesLink.addEventListener('click', () => {
        this.initializeJoinGamePopupContent();
      });
    }

    const createGameLink = document.getElementById('create-game');
    if (createGameLink) {
      createGameLink.addEventListener('click', async () => {
        this.initializeCreateGamePopupContent();
      });
    }

    const joinMatchmakingLink = document.getElementById('join-matchmaking');
    if (joinMatchmakingLink) {
      joinMatchmakingLink.addEventListener('click', () => {
        this.initializeJoinMatchmakingPopup();
      })
    }
    if(this.router.url.includes('cancelled')){
      document.getElementById('join-matchmaking')!.click();
    }
  }




  /**
   * Initialise le popup de liste des parties avec les différentes parties disponibles
   */
  private initializeJoinGamePopupContent() {
    this.gameDAO.GetAvailableGames().subscribe({
      next: async (games: AvailableGameInfoDTO[]) => {
        let content = '';

        if (games.length === 0) {
          content = '<p>Aucune partie disponible pour le moment...</p>';
        } else {
          games.forEach((game, index) => {
            let stringRule = game["rule"] == "j" 
              ? `<img class="flag" src="japan.svg"/>` 
              : `<img class="flag" src="china.svg"/>`;
            content += `<div class="game-choice">
              <i class="fas fa-play"></i>
              <button id="game-${index}"> 
              <span id="gameName">${game["name"]}</span> - ${game["size"]}x${game["size"]} - Créateur : ${game["creatorName"]}
              <div class="game-info">
                <div class="grid-column">
                  <div class="komi">Komi : ${game["komi"]}</div>
                  <div class="handicap">Handicap : ${game["handicap"]} <img src="${game["handicapColor"]}.png" id="stone-${game["handicapColor"]}"></div>
                </div>
                ${stringRule}
              </div>
              </button>
            </div><br>`;
          });
        }

        // Afficher le popup
        Swal.fire({
          title: 'Parties disponibles',
          html: content,

          showCloseButton: true,
          focusConfirm: false,
          confirmButtonText: 'Fermer',
          customClass: {
            confirmButton: 'custom-ok-button',
          },
          width: '800px',
          didOpen: () => {
            // Ajouter les event listeners après que le contenu soit injecté
            games.forEach((game, index) => {
              const button = document.getElementById(`game-${index}`);
              if (button) {
                button.addEventListener("click", () => {
                  this.websocketService.joinGame(game["id"], "custom", game["rule"], game["size"]);
                  Swal.close();
                });
              }
            });
          }
        });
      },
      error: (err) => {
        Swal.fire({
          title: 'Erreur',
          text: 'Impossible de récupérer les parties disponibles.',
          icon: 'error',
          confirmButtonText: 'Fermer',
          customClass: {
            confirmButton: 'custom-ok-button',
          },
        });
      },
    });
  }


  /**
   * Initialise le popup de création de partie avec les différentes options
   * Une fois le choix fait, affiche un chargement en attendant la réponse du serveur
   * Lorsque la partie est crée, redirige l'utilisateur vers celle-ci et ferme le chargement
   */
  private initializeCreateGamePopupContent() {
    Swal.fire({
      title: 'Créer une partie',
      html: `
        <form id="create-game-form">
         <label for="game-name">Nom de la partie :</label>
          <input id="game-name" name="game-name" type="text" class="swal2-input" placeholder="Nom de la partie">
          <br>

          <label for="grid-size">Taille de la grille :</label>
          <select id="grid-size" name="grid-size" class="swal2-select">
            <option value="9">9x9</option>
            <option value="13">13x13</option>
            <option value="19" selected>19x19</option>
          </select>
          
          <label for="rules">Règles du jeu :</label>
          <select id="rules" name="rules" class="swal2-select">
            <option value="c">Chinoises</option>
            <option value="j">Japonaises</option>
          </select>

          <label for="komi">Choix du komi :</label>
          <input id="komi" name="komi" type="text" class="swal2-input" value="6.5" required>
          <br>

          <label for="number">Choix du handicap :</label>
          <input type="number" id="handicap" name="handicap" min="0" max="9" class="swal2-input" value="0"/>
          <div class="radio-container">
            <input type="radio" class="demo3" id="black" name="demoGroup">
            <label for="black">Noir</label>

            <input type="radio" class="demo3" id="white" name="demoGroup" checked>
            <label for="white">Blanc</label>
          </div>
        </form>
      `,
      confirmButtonText: 'Créer',
      showCloseButton: true,
      customClass: {
        confirmButton: 'custom-ok-button'
      },
      preConfirm: () => {
        const gridSize = (document.getElementById('grid-size') as HTMLSelectElement).value;
        const rules = (document.getElementById('rules') as HTMLSelectElement).value;
        const name = (document.getElementById('game-name') as HTMLSelectElement).value;
        const komi = (document.getElementById('komi') as HTMLSelectElement).value;
        const handicap = (document.getElementById('handicap') as HTMLSelectElement).value;
        const selectedColor = (document.querySelector('input[name="demoGroup"]:checked') as HTMLInputElement)?.id;

        return { gridSize, rules, name, komi, handicap, selectedColor };
      },
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { gridSize, rules, name, komi, handicap, selectedColor } = result.value!;

        // Affichez un chargement avant la connexion
        Swal.fire({
          title: 'Connexion en cours...',
          text: 'Veuillez patienter pendant la création de la partie...',
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          },
        });

        try {
          this.websocketService.createPersonalizeGame(gridSize, rules, komi, name, handicap, selectedColor);
          Swal.close(); // Ferme le chargement
        } catch (error) {
          Swal.close(); // Ferme le chargement en cas d'erreur
          Swal.fire('Erreur', 'La connexion a échoué. Veuillez réessayer.', 'error');
        }
      }
    });
  }

  /**
 * Initialise le popup de recherche de partie avec matchmaking en affichant un chargement en attendant l'attribution d'un adversaire
 * Lorsque la partie est crée, redirige l'utilisateur vers celle-ci et ferme le chargement
 */
  private initializeJoinMatchmakingPopup() {
    let matchFound = false;
    let content;
    if(this.router.url.includes('cancelled')){
      content = "Votre adversaire n'a pas rejoint la partie, veuillez patienter pendant que nous vous trouvons un autre adversaire...";
    }
    else{
      content = 'Veuillez patienter pendant que nous recherchons un adversaire à votre niveau...'
    }

    Swal.fire({
      title: 'Recherche en cours...',
      text: content,
      showCloseButton: false,
      allowOutsideClick: false,
      showCancelButton: false,
      didOpen: async () => {
        Swal.showLoading();
        try {
          await this.websocketService.joinMatchmaking();
          Swal.close();
        } catch(error) {
          Swal.fire({
            title: 'Erreur', 
            text: 'La recherche de partie a échoué. Veuillez réessayer.',
            icon: 'error'
          });
        }
      },
    });
  }
  /**
 * Remplit le leaderboard avec les 5 joueurs ayant le meilleur Elo du serveur.
 */
private populateLeaderboard(): void {
  this.userDAO.GetLeaderboard().subscribe({
    next: (leaderboard: any) => {
      const leaderboardElement = document.querySelector('.leaderboard');
      const topPlayers = Object.entries(leaderboard);
      
      topPlayers.forEach(([name, elo], index) => {
        let userTop = new User(name, "", Number(elo));
        const playerDiv = document.createElement('div');
        playerDiv.className = 'leaderboard-item';
        
        playerDiv.innerHTML = `
          <span class="leaderboard-rank">${index + 1}</span>
          <img src="https://localhost:7065/profile-pics/${name}" 
               alt="${name}'s avatar" 
               class="leaderboard-avatar">
          <div class="leaderboard-info">
            <span class="leaderboard-name">${name}</span>
            <span class="leaderboard-rank-info">${userTop.getRank()}</span>
          </div>
        `;
        
        leaderboardElement!.appendChild(playerDiv);
      });
    },
  });
}

/**
 * Appel à l'API pour rejouer la dernière partie
 */
public async replayLastGame():Promise<void>{
  let token = this.userCookieService.getToken();
  let id = await firstValueFrom(this.gameDAO.GetLastGameId(token));
  let game = await firstValueFrom(this.gameDAO.GetGameById(id));
  let size = game["size"];
  await this.router.navigate(['/replay', id, size]);
  window.location.reload();
}

  
}
