import { Injectable } from '@angular/core';
import { UserCookieService } from './Model/UserCookieService';
import { Interpreter } from './interpreter';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
/**
 * Service gérant la connexion au serveur websocket
 */
export class WebsocketService {
  private websocket: WebSocket | null;

  private interpreter: Interpreter;

  /**
   * Constructeur du service
   * @param userCookieService Service permettant de récupérer les informations de l'utilisateur
   */
  constructor(private userCookieService: UserCookieService, private router: Router,) {
    this.websocket = null;;
    this.interpreter = new Interpreter();
  }


  /**
   * Fonction permettant de se connecter au serveur websocket
   */
  public connectWebsocket(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.websocket = new WebSocket('ws:///127.0.0.1:7000/'); //10.211.55.3
      this.websocket.onopen = () => {
        console.log('connected');
        resolve();
      };

      this.websocket.onmessage = (message) => {
        let state = { end: false, won: "false", player1score: '0', player2score: '0'};
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

  private endGame(won: string, player1score: string, player2score: string) {
    this.disconnectWebsocket(); 
    Swal.fire({
      title: won === "True" ? 'Victoire ! 🎉' : 'Défaite 😞',
      text: `Score final : ${player1score} - ${player2score}`,
      icon: won === "True" ? 'success' : 'error',
      confirmButtonText: 'Fermer',
      customClass: {
        confirmButton: 'custom-ok-button'
      },
    }).then(() => {
      // Redirection vers l'index après la fermeture du popup
      this.router.navigate(['/index']);
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
  public createGame(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`0/Create:${userToken}`);
      this.interpreter.setColor('black');
    } else {
      console.log('not connected');
    }
  }


  /**
   * Envoi un message de demande de rejoindre une partie
   * @param id Identifiant de la partie à rejoindre
   */
  public joinGame(id: number): void {
    if (this.websocket != null && this.websocket.OPEN) {
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`${id}/Join:${userToken}`);
      this.interpreter.setColor('white');
    } else {
      console.log('not connected');
    }
  }


  /**
   * Envoi un message de demande de skip de tour
   */
  public skipTurn(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      let idGame = this.interpreter.getIdGame();
      this.websocket.send(`${idGame}Skip:`);
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
      let idGame = this.interpreter.getIdGame();
      this.websocket.send(`${idGame}Stone:${coordinates}`);
      console.log(`${idGame}Stone:${coordinates}`);
    } else {
      console.log('not connected');
    }
  }
}
