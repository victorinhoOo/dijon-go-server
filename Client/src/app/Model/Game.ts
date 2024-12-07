import { Observable } from "../Observer/Observable";

const ONE_HOUR_IN_MS = 3600000;

export class Game extends Observable{

  private currentTurn: string;
  private playerColor: string;

  private playerMs:number;

  private opponentMs:number;

  private opponentPseudo : string ;


  public constructor() {
    super();
    this.currentTurn = '';
    this.playerColor = '';
    this.opponentPseudo = '';
    this.playerMs = ONE_HOUR_IN_MS;
    this.opponentMs = ONE_HOUR_IN_MS;
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
  
  public launchTimer(){
    if(this.playerColor == this.currentTurn){
      this.playerTimerTick();
    }else{
      this.opponentTimerTick();
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
}
