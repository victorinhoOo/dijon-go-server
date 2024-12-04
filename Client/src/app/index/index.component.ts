import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../Model/UserCookieService';
import { ActivatedRoute, Router } from '@angular/router';
import { GameDAO } from '../Model/DAO/GameDAO';
import { HttpClient } from '@angular/common/http';
import { GameInfoDTO } from '../Model/DTO/AvailableGameInfoDTO';
import { WebsocketService } from '../websocket.service';
import { HttpClientModule } from '@angular/common/http';
import Swal from 'sweetalert2';
import { User } from '../Model/User';
import { UserDAO } from '../Model/DAO/UserDAO';
import { RankprogressComponent } from "../rankprogress/rankprogress.component";

@Component({
  selector: 'app-index',
  standalone: true,
  imports: [NavbarComponent, MatIcon, HttpClientModule, RankprogressComponent],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css'],
})
export class IndexComponent implements OnInit {
  private token: string;
  private userPseudo: string;
  private avatar: string;
  private userRank: string;
  private gameDAO: GameDAO;
  private userDAO: UserDAO;

  /**
 * Getter pour le lien d'affichage de l'avatar
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
    private route: ActivatedRoute
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
    this.websocketService.disconnectWebsocket();
    this.userCookieService.getUserObservable().subscribe((user: User | null) => {
      if (user) {
        this.userPseudo = user.Username;
        this.userRank = this.userCookieService.getUser()!.Rank;
        this.avatar = `https://localhost:7065/profile-pics/${this.userPseudo}`;
      }
    });

    // Vérifiez et gérez la connexion
    this.token = this.userCookieService.getToken();
    if (!this.token) {
      this.router.navigate(['/login']);
    }
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
  }


  /**
   * Initialise le popup de liste des parties avec les différentes parties disponibles
   */
  private initializeJoinGamePopupContent() {
    this.gameDAO.GetAvailableGames().subscribe({
      next: async (games: GameInfoDTO[]) => {
        let content = '';

        if (games.length === 0) {
          content = '<p>Aucune partie disponible pour le moment...</p>';
        } else {
          await this.websocketService.connectWebsocket();
          games.forEach((game, index) => {
            let stringRule = game["rule"] == "j" 
              ? `<img class="flag" src="japan.svg"/>` 
              : `<img class="flag" src="china.svg"/>`;
            content += `<div class="game-choice">
              <i class="fas fa-play"></i>
              <button id="game-${index}"> <span id="gameName">${game["name"]}</span> - ${game["size"]}x${game["size"]} - Créateur : ${game["creatorName"]} ${stringRule}</button>
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
        console.error("Erreur lors de la récupération des parties :", err);
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
          <input id="game-name" name="game-name" type="text" class="swal2-input" placeholder="Nom de la partie" required>
          <br>

          <label for="grid-size">Taille de la grille :</label>
          <select id="grid-size" name="grid-size" class="swal2-select">
            <option value="9">9x9</option>
            <option value="10">10x10</option>
            <option value="11">11x11</option>
            <option value="12">12x12</option>
            <option value="13">13x13</option>
            <option value="14">14x14</option>
            <option value="15">15x15</option>
            <option value="16">16x16</option>
            <option value="17">17x17</option>
            <option value="18">18x18</option>
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
        const handicap = 7;
        return { gridSize, rules, name, komi, handicap };
      },
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { gridSize, rules, name, komi , handicap } = result.value!;

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
          // todo: envoyer le choix des règles au serveur
          await this.websocketService.connectWebsocket();
          this.websocketService.createPersonalizeGame(gridSize, rules, "custom", komi, name, handicap);
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

    Swal.fire({
      title: 'Recherche en cours...',
      text: 'Veuillez patienter pendant que nous recherchons un adversaire à votre niveau...',
      showCloseButton: false,
      allowOutsideClick: false,
      showCancelButton: false,
      didOpen: async () => {
        Swal.showLoading();
        try {
          await this.websocketService.connectWebsocket();
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
      // Accès à l'élément DOM pour le leaderboard
      const leaderboardElement = document.querySelector('.leaderboard');
    
      if (!leaderboardElement) {
        console.error('Leaderboard DOM introuvable.');
        return;
      }
    
      // Vidage du contenu actuel du leaderboard
      leaderboardElement.innerHTML = '';
      //remplissage d'un tableau
      const topPlayers = Object.entries(leaderboard);
      
      //affichage de chaque joueur du leaderboard
      topPlayers.forEach(([name, elo], index) => {
        let userTop = new User(name,"",Number(elo)); //creation d'un user pour obtenir son rang
        const rankString = `${index + 1}) ${name} - ${userTop.Rank}`;
        const p = document.createElement('p');
        p.textContent = rankString;
        leaderboardElement.appendChild(p);
      });
    },
    error: (err) => {
      console.error('Erreur lors de la récupération du leaderboard:', err);
    }
  });
}

  
}
