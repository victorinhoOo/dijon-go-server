import { clear } from "console";
import { Observable } from "../Observer/Observable";

const ONE_HOUR_IN_MS = 3600000;
const TIMER_INTERVAL = 1000;

export class Game extends Observable{

  private currentTurn: string;
  private playerColor: string;

  private playerMs:number;

  private opponentMs:number;

  private opponentPseudo : string;

  private board: string;

  private captures : string;

  private timerInterval: NodeJS.Timeout | null;


  public constructor() {
    super();
    this.currentTurn = '';
    this.playerColor = '';
    this.opponentPseudo = '';
    this.board = '';
    this.captures = '';
    this.timerInterval = null;
    this.playerMs = ONE_HOUR_IN_MS;
    this.opponentMs = ONE_HOUR_IN_MS;
  }

  public getCaptures():string{
    return this.captures;
  }

  public setCaptures(captures:string){  
    this.captures = captures;
    this.notifyChange(this);
  }

  public getBoard():string{
    return this.board;
  }

  public setBoard(board:string){
    this.board = board;
    this.notifyChange(this);
  }

  public getOpponentPseudo():string{
    return this.opponentPseudo;
  }

  public setOpponentPseudo(pseudo:string){
    this.opponentPseudo = pseudo;
    this.notifyChange(this);
  }

  public getPlayerMs():number{
    return this.playerMs;
  }
  public getOpponentMs():number{
    return this.opponentMs;
  }

  /**
   * Changement de tour
   */
  public changeTurn(): void {
    this.currentTurn = this.currentTurn == 'black' ? 'white' : 'black';
  }

  /**
   * Récupère le tour actuel
   * @returns le tour actuel
   */
  public getCurrentTurn(): string {
    return this.currentTurn;
  }

  /**
   * Récupère la couleur du joueur
   * @returns la couleur du joueur
   */
  public getPlayerColor(): string {
    return this.playerColor;
  }

  /**
   * Initialisation du tour actuel
   */
  public initCurrentTurn() {
    this.currentTurn = 'black';
    this.notifyChange(this);
  }


  /**
   * Change la couleur du joueur
   * @param color la couleur à attribuer
   */
  public setPlayerColor(color: string) {
    this.playerColor = color;
  }



  /**
   * Savoir si c'est le tour du joueur
   * @returns true si c'est le tour du joueur, false sinon 
   */
  public isPlayerTurn(): boolean {
    return this.playerColor == this.currentTurn;
  }

  public endGame(){
    this.clearTimer();
    this.destroy();
  }
  
  public launchTimer(){
    this.timerInterval = setInterval(() => {
      if(this.playerColor == this.currentTurn){
        this.playerTimerTick();
      }else{
        this.opponentTimerTick();
      }
    }, TIMER_INTERVAL);
  }

  private clearTimer(){
    if(this.timerInterval != null){
      clearInterval(this.timerInterval);
    }
  }

  private playerTimerTick(){
    this.playerMs -= 1000;
    this.notifyChange(this);
  }

  private opponentTimerTick(){
    this.opponentMs -= 1000;
    this.notifyChange(this);
  }

  private destroy():void{
    this.currentTurn = '';
    this.playerColor = '';
    this.opponentPseudo = '';
    this.board = '';
    this.captures = '';
    this.playerMs = ONE_HOUR_IN_MS;
    this.opponentMs = ONE_HOUR_IN_MS;
    this.notifyChange(this);
  }




}
