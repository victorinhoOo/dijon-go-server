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

  /**
   * Récupère les captures
   * @returns les captures
   */
  public getCaptures():string{
    return this.captures;
  }

  /**
   * Chang la valeur de l'attribut puis notify ses observateurs
   * @param captures valeur à attribuer 
   */
  public setCaptures(captures:string){  
    this.captures = captures;
    this.notifyChange(this);
  }

  /**
   * récupère le plateau de jeu
   * @returns le plateau de jeu
   */
  public getBoard():string{
    return this.board;
  }


  /**
   * Modifie le plateau de jeu puis notifie ses observateurs
   * @param board nouveau plateau de jeu
   */
  public setBoard(board:string){
    this.board = board;
    this.notifyChange(this);
  }

  /**
   * Récupère le pseudo de l'adversaire
   * @returns le pseudo de l'adversaire
   */
  public getOpponentPseudo():string{
    return this.opponentPseudo;
  }

  /**
   * Modifier le pseudo de l'adversaire puis notifie ses observateurs
   * @param pseudo le nouveau pseudo de l'adversaire
   */
  public setOpponentPseudo(pseudo:string){
    this.opponentPseudo = pseudo;
    this.notifyChange(this);
  }

  /**
   * Récupère le temps restant du joueur
   * @returns le nombre de millisecondes restantes
   */
  public getPlayerMs():number{
    return this.playerMs;
  }


  /**
   * Récupère le temps restant de l'adversaire
   * @returns le nombre de millisecondes restantes
   */
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
  public initCurrentTurn():void {
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

  /**
   * Exécuté à la fin de la partie, réinitialise les timers et les attributs
   */
  public endGame(){
    this.clearTimer();
    this.destroy();
  }
  
  /**
   * Lance le timer du joueur qui doit jouer
   */
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
