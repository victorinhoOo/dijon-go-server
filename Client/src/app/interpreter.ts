import { log } from "console";
import { Game } from "./Model/Game";
import { WebsocketService } from "./websocket.service";
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


/**
 * Classe qui interprete les messages envoyés par le serveur websocket
 */
export class Interpreter implements IObserver{
  private idGame: any;
  private game: Game;

  private strategies : Map<string, IStrategy>;

  //#region Propriétés

  /**
   * Modifie la valeur de l'attribut game
   * @param game partie concernée
   */
  public setGame(game:Game){
    this.game = game;
  } 

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


  //#endregion



  /**
   * Constructeur de la classe
   */
  constructor(game:Game, private websocketService: WebsocketService) {
    this.idGame = {value: ''};
    this.game = game;
    this.game.register(this);
    let matchmakingStrategy = new MatchmakingStrategy(this.websocketService);
    this.strategies = new Map<string, IStrategy>();
    this.strategies.set("EndOfGame", new EndOfGameStrategy());
    this.strategies.set("Init", new InitIdGameStrategy());
    this.strategies.set("Create", matchmakingStrategy);
    this.strategies.set("Join", matchmakingStrategy);
    this.strategies.set("Skipped", new SkipStrategy());
    this.strategies.set("Start", new StartStrategy());
    this.strategies.set("Timeout", new TimeoutStrategy());
    this.strategies.set("Stone", new UpdateTurnStrategy());
    this.strategies.set("Cancelled", new CancelStrategy());
    this.strategies.set("Retry", matchmakingStrategy);
  }
  public update(object: Observable): void {
    this.game = object as Game;
  }

  

  /**
   * Interprete le message envoyé par le serveur websocket
   * @param message message envoyé par le serveur websocket
   * @param state définit l'état de la partie (en cours ou terminée)
   */
  public interpret(message: string, state: { end: boolean, won: string, player1score: string, player2score: string}): void {
    console.log("Received: " + message);
    let data = message.split('-');
    let action = data[1];
    if (message.length <= 3) {
      action = "Init";
    } 
    this.strategies.get(action)?.execute(data, state, this.idGame, this.game);
  }
}
