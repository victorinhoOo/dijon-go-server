import { Injectable } from '@angular/core';
import { UserCookieService } from './Model/UserCookieService';
import { Interpreter } from './interpreter';
import { Game } from './Model/Game';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { environment } from './environment';
import { env } from 'process';
import { UserDAO } from './Model/DAO/UserDAO';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { User } from './Model/User';

@Injectable({
  providedIn: 'root',
})

/**
 * Service gérant la connexion au serveur websocket
 */
export class WebsocketService {
  private websocket: WebSocket | null;
  private game: Game;
  private interpreter: Interpreter;
  private userDAO: UserDAO;

  /**
   * Constructeur du service
   * @param userCookieService Service permettant de récupérer les informations de l'utilisateur
   */
  constructor(private userCookieService: UserCookieService, private router: Router, private httpclient: HttpClient) {
    this.websocket = null;
    this.game = new Game();
    this.interpreter = new Interpreter(this.game, this);
    this.userDAO = new UserDAO(httpclient);
  }


  /**
   * Fonction permettant de se connecter au serveur websocket
   */
  public connectWebsocket(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.websocket = new WebSocket(`ws:///${environment.websocketUrl}/`);
      this.websocket.onopen = () => {
        console.log('connected');
        resolve();
      };

      this.websocket.onmessage = (message) => {
        let state = { end: false, won: "false", player1score: '0', player2score: '0' };
        this.interpreter.interpret(message.data, state);
        if (state.end) {
          this.endGame(state.won, state.player1score, state.player2score);
        }
      };

      this.websocket.onclose = () => {
        console.log('disconnected');
      };
    });
  }

  /**
   * Gère la fin de partie en affichant un popup indiquant le gagnant et son score ainsi que le nouvel elo
   * @param won gagné ou non
   * @param player1score score du joueur 
   * @param player2score score de son adversaire
   */
  private endGame(won: string, player1score: string, player2score: string) {
    this.disconnectWebsocket();
    // On récupère les nouvelles informations utilisateurs car elles ont été modifiées (elo)
    let token = this.userCookieService.getToken();
    this.userDAO.GetUser(token).subscribe({
      next: (user: User) => {
        this.userCookieService.setUser(user);
        console.log(player1score);
        console.log(player2score);
        Swal.fire({
          title: won === "True" ? 'Victoire ! 🌸' : 'Défaite 👺',
          html: `
          <div class="game-result">
            <p>Score final : ${player1score} - ${player2score}</p>
            <div class="elo-message">
              Rang : ${user.Rank}
            </div>
          </div>
        `,
          icon: won === "True" ? 'success' : 'error',
          confirmButtonText: 'Fermer',
          customClass: {
            confirmButton: 'custom-ok-button',
          },
        }).then(() => {
          // Redirection vers l'index après la fermeture du popup
          this.router.navigate(['/index']);
        });
      }
    });
  }


  /**
   * Fonction permettant de se déconnecter du serveur websocket
   */
  public disconnectWebsocket() {
    this.websocket?.close(1000);
  }


  /**
   * Envoi un message de création de partie
   */
  public createGame(size: number, rule: string, type:string): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.setPlayerColor("black");
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`0-Create-${userToken}-${size}-${rule}-${type}`);
      this.interpreter.setColor('black');
      this.router.navigate(['game', size, rule]);
    } else {
      console.log('not connected');
    }
  }


  /**
   * Envoi un message de demande de rejoindre une partie
   * @param id Identifiant de la partie à rejoindre
   */
  public joinGame(id: number, type:string, rule:string, size:number): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.setPlayerColor("white");
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`${id}-Join-${userToken}-${type}`);
      this.interpreter.setColor('white');
      this.router.navigate(['game', size, rule]);
    } else {
      console.log('not connected');
    }
  }

  /**
   * Envoi un message de demande de matchmaking
   */
  public joinMatchmaking(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.websocket != null && this.websocket.OPEN) {
        let userToken = this.userCookieService.getToken();
        
        // Stocker la Promise resolve pour l'utiliser dans l'interpreteur
        (this.interpreter as any).matchmakingResolve = resolve;
        
        // Envoi de la demande de matchmaking
        this.websocket.send(`0-Matchmaking`);
      } else {
        reject(new Error('Non connecté au websocket'));
      }
    });
  }


  /**
   * Envoi un message de demande de skip de tour
   */
  public skipTurn(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      if (this.interpreter.getCurrentTurn() == this.interpreter.getPlayerColor()) {
        let idGame = this.interpreter.getIdGame();
        this.websocket.send(`${idGame}-Skip`);
      }
    } else {
      console.log('not connected');
    }
  }



  /**
   * Place une pierre sur le plateau
   * @param coordinates Coordonnées de la pierre à placer
   */
  public placeStone(coordinates: string) {
    if (this.websocket != null && this.websocket.OPEN) {
      if (this.interpreter.getCurrentTurn() == this.interpreter.getPlayerColor()) {
        let idGame = this.interpreter.getIdGame();
        this.websocket.send(`${idGame}-Stone-${coordinates}`);
        console.log(`${idGame}-Stone-${coordinates}`);
      }
    } else {
      console.log('not connected');
    }
  }

  public setPlayerColor(color: string) {
    this.game.setPlayerColor(color);
    this.interpreter.setGame(this.game);
  }
}
