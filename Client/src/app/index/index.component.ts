import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../Model/UserCookieService';
import { ActivatedRoute, Router } from '@angular/router';
import { GameDAO } from '../Model/DAO/GameDAO';
import { HttpClient } from '@angular/common/http';
import { GameInfoDTO } from '../Model/DTO/GameInfoDTO';
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
  private elo: number;
  private gameDAO: GameDAO;
  private userDAO: UserDAO;

    /**
   * Getter pour l'affichage de l'elo
   */
  public get Elo(): number
  {
    return this.elo;
  }
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
    this.elo = this.userCookieService.getUser()!.Elo;
    this.userRank = this.userCookieService.getUser()!.Rank;
    this.userDAO = new UserDAO(httpClient);
  }

  /**
   * Initialise les informations utilisateurs, le leaderboard, et gère la création de parties
   */
  public async ngOnInit() {
    if (this.route.snapshot.paramMap.get('id') != null) {
      let id = this.route.snapshot.paramMap.get('id');
      await this.websocketService.connectWebsocket();
      this.websocketService.joinGame(Number(id));
      this.router.navigate(['game']);
    } else {
      this.websocketService.disconnectWebsocket();
    }
    this.userCookieService.getUserObservable().subscribe((user: User | null) => {
      if (user) {
        this.userPseudo = user.Username;
        this.elo = user.Elo;
        this.userRank = user.Rank;
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
      } )
    }
  }

  /**
   * Initialise le popup de liste des partie avec les différentes parties disponibles
   */
  private initializeJoinGamePopupContent() {
    this.gameDAO.GetAvailableGames().subscribe({
      next: (games: GameInfoDTO[]) => {
        let content = '';
        games.forEach((game) => {
          content += `<div class="game-choice"><i class="fas fa-play"></i><a href="/${game["id"]}">${game["title"]} ${game["size"]}x${game["size"]}</a></div><br>`;
        });
        Swal.fire({
          title: 'Parties disponibles',
          html: content,
          showCloseButton: true,
          focusConfirm: false,
          confirmButtonText: 'Fermer',
          customClass: {
            confirmButton: 'custom-ok-button'
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
            <option value="chinoises">Chinoises</option>
            <option value="japonaises">Japonaises</option>
          </select>
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
        return { gridSize, rules };
      },
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { gridSize, rules } = result.value!;

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
          this.websocketService.createGame();
          Swal.close(); // Ferme le chargement
          this.router.navigate(['game']);
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
    this.gameDAO.GetAvailableGames().subscribe({
      next: (games: GameInfoDTO[]) => {
        // Affichez un chargement avant la connexion
        Swal.fire({
          title: 'Recherche en cours...',
          text: 'Veuillez patienter pendant que nous recherchons un adversaire à votre niveau...',
          showCloseButton: true,
          didOpen: () => {
            Swal.showLoading();
            //todo : recherche de partie
          },
          willClose: () => {
            // todo : Arrêtez la recherche de partie si l'utilisateur ferme le popup
          }
        });
      }
    })
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
