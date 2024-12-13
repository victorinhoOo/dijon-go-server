import { log } from "console";
import { Game } from "./Game";
import { WebsocketService } from "./services/websocket.service";
import { IStrategy } from "./Strategy/IStrategy";
import { EndOfGameStrategy } from "./Strategy/EndOfGameStrategy";
import { InitIdGameStrategy } from "./Strategy/InitIdGameStrategy";
import { MatchmakingStrategy } from "./Strategy/MatchmakingStrategy";
import { SkipStrategy } from "./Strategy/SkipStrategy";
import { StartStrategy } from "./Strategy/StartStrategy";
import { TimeoutStrategy } from "./Strategy/TimeoutStrategy";
import { UpdateTurnStrategy } from "./Strategy/UpdateTurnStrategy";
import { CancelStrategy } from "./Strategy/CancelStrategy";
import { IObserver } from "./Observer/IObserver";
import { Observable } from "./Observer/Observable";
import { UserListStrategy } from "./Strategy/UserListStrategy";
import { ConnectedUsersService } from "./services/connected-users.service";
import { ChatService } from "./services/chat.service";
import { ChatStrategy } from "./Strategy/ChatStrategy";
import { UserCookieService } from "./services/UserCookieService";
import { HttpClient } from "@angular/common/http";

const ACTION_POSITION = 1


/**
 * Classe qui interprete les messages envoyés par le serveur websocket
 */
export class Interpreter implements IObserver{
  private idGame: any;
  private game: Game;

  private strategies : Map<string, IStrategy>;



  /**
   * Récupère l'id de jeu
   * @returns l'id du jeu
   */
  public getIdGame(): string {
    return this.idGame.value;
  }

  /**
   * Récupère la stratégie de matchmaking
   * @returns la stratégie de matchmaking
   */
  public getMatchMakingStrategy(){
    return (this.strategies.get("Join") as MatchmakingStrategy);
  }




  /**
   * Constructeur de la classe
   */
  constructor(game:Game,private http:HttpClient, private websocketService: WebsocketService, private connectedUsersService: ConnectedUsersService, private chatService: ChatService, private userCookieService: UserCookieService) {
    this.idGame = {value: ''};
    this.game = game;
    this.game.register(this);
    let matchmakingStrategy = new MatchmakingStrategy(this.websocketService);
    this.strategies = new Map<string, IStrategy>();
    this.strategies.set("EndOfGame", new EndOfGameStrategy(this.userCookieService, this.http));
    this.strategies.set("Init", new InitIdGameStrategy());
    this.strategies.set("Create", matchmakingStrategy);
    this.strategies.set("Join", matchmakingStrategy);
    this.strategies.set("Skipped", new SkipStrategy());
    this.strategies.set("Start", new StartStrategy());
    this.strategies.set("Timeout", new TimeoutStrategy());
    this.strategies.set("Stone", new UpdateTurnStrategy());
    this.strategies.set("Cancelled", new CancelStrategy());
    this.strategies.set("Retry", matchmakingStrategy);
    this.strategies.set("UserList", new UserListStrategy(this.connectedUsersService));
    this.strategies.set("Chat", new ChatStrategy(this.chatService, this.userCookieService));
  }

  /**
   * Appelé par l'observable pour mettre à jour l'objet
   * @param object mise à jour de l'objet
   */
  public update(object: Observable): void {
    this.game = object as Game;
  }

  

  /**
   * Interprete le message envoyé par le serveur websocket
   * @param message message envoyé par le serveur websocket
   * @param state définit l'état de la partie (en cours ou terminée)
   */
  public interpret(message: string): void {
    let data = message.split('-');
    let action = data[ACTION_POSITION];
    if (message.length <= 3) {
      action = "Init";
    } 
    this.strategies.get(action)?.execute(data, this.idGame, this.game);
  }
}
