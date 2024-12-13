import { Injectable } from '@angular/core';
import { UserCookieService } from '../services/UserCookieService';
import { Interpreter } from '../interpreter';
import { Game } from '../Game';
import { Router } from '@angular/router';
import { environment } from '../../environment';
import { UserDAO } from '../DAO/UserDAO';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { IObserver } from '../Observer/IObserver';
import { Observable } from '../Observer/Observable';
import { ConnectedUsersService } from '../services/connected-users.service';
import { ChatService } from '../services/chat.service';
@Injectable({
  providedIn: 'root',
})

/**
 * Service gérant la connexion au serveur websocket
 */
export class WebsocketService implements IObserver {
  private websocket: WebSocket | null;
  private game: Game;
  private interpreter: Interpreter;
  private userDAO: UserDAO;

  /**
   * Constructeur du service
   * @param userCookieService Service permettant de récupérer les informations de l'utilisateur
   */
  constructor(private userCookieService: UserCookieService, private router: Router, private httpclient: HttpClient, private connectedUsersService: ConnectedUsersService, private chatService: ChatService) {
    this.websocket = null;
    this.game = new Game(this.router);
    this.game.register(this);
    this.interpreter = new Interpreter(this.game,this.httpclient, this, this.connectedUsersService, this.chatService, this.userCookieService);
    this.userDAO = new UserDAO(httpclient);
  }

  /**
   * Appelée par l'observable pour mettre à jour les informations du service
   * @param object nouvelle version de l'objet observé
   */
  public update(object: Observable):void{
    this.game = object as Game;
  }

  /**
   * Récupère la partie en cours
   * @returns la partie en cours
   */
  public getGame():Game{
    return this.game;
  }


  /**
   * Fonction permettant de se connecter au serveur websocket, transmet le token utilisateur pour l'authentification
   */
  public connectWebsocket(): Promise<void> {
    const userToken = this.userCookieService.getToken();
    return new Promise((resolve, reject) => {
      this.websocket = new WebSocket(`ws:///${environment.websocketUrl}/?token=${userToken}`);
      this.websocket.onopen = () => {
        resolve();
      };

      this.websocket.onmessage = (message) => {
        this.interpreter.interpret(message.data);
      };

      this.websocket.onclose = () => {
      };
    });
  }

  /**
   * Renvoi l'état de la connexion au websocket
   * @returns True si la connexion est établie, sinon false
   */
  public isWebsocketConnected(): boolean{
    return this.websocket?.OPEN ? true : false;
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
  public createMatchmakingGame(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.game.setPlayerColor("black");
      this.websocket.send(`0-Create-matchmaking`);
      this.router.navigate(['game', 19, "j"]);
    }
  }

    /**
     * Envoi un message de création de partie personalisée avec les paramètres choisis par le client
     */
    public createPersonalizeGame(size: number, rule: string, komi:string, name:string,handicap:number,colorHandicap: string): void {
      if (this.websocket != null && this.websocket.OPEN) {
        this.game.setPlayerColor("black");
        this.websocket.send(`0-Create-custom-${size}-${rule}-${komi}-${name}-${handicap}-${colorHandicap}`);
        this.router.navigate(['game', size, rule]);
      }
    }
    
  /**
   * Envoi un message de demande de rejoindre une partie
   * @param id l'id de la partie à rejoindre
   * @param type le type de partie que l'on souhaite rejoindre (matchmaking ou custom)
   * @param rule les règles de la partie
   * @param size la taille de la grille de jeu de la partie
   */
  public joinGame(id: number, type:string, rule:string, size:number): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.game.setPlayerColor("white");
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`${id}-Join-${type}`);
      this.router.navigate(['game', size, rule]);
    }
  }

  /**
   * Envoi un message de demande de matchmaking
   */
  public joinMatchmaking(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.websocket != null && this.websocket.OPEN) {
        
        // Stocker la Promise resolve pour l'utiliser dans l'interpreteur
        (this.interpreter.getMatchMakingStrategy() as any).matchmakingResolve = resolve;
        
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
      if (this.game.getCurrentTurn() == this.game.getPlayerColor()) {
        let idGame = this.interpreter.getIdGame();
        this.websocket.send(`${idGame}-Skip`);
      }
    }
  }



  /**
   * Place une pierre sur le plateau
   * @param coordinates Coordonnées de la pierre à placer
   */
  public placeStone(coordinates: string) {
    if (this.websocket != null && this.websocket.OPEN) {
      if (this.game.getCurrentTurn() == this.game.getPlayerColor()) {
        let idGame = this.interpreter.getIdGame();
        this.websocket.send(`${idGame}-Stone-${coordinates}`);
      }
    }
  }


  public cancelMatchmaking(idLobby:string) {
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send(`${idLobby}-Cancel`);
    }
  }

  public sendMessage(message: string, receiver: string) {
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send(`0-Chat-${receiver}-${message}`);
    }
  }
}
