import { log } from "console";
import { Game } from "./Model/Game";

/**
 * Classe qui interprete les messages envoyés par le serveur websocket
 */
export class Interpreter {
  private idGame: string;
  private color: string;
  private game: Game

  /**
   * Constructeur de la classe
   */
  constructor(game:Game) {
    this.idGame = '';
    this.game = game;
    this.color = '';
  }

  /**
   * Setter pour la couleur du joueur
   * @param color couleur du joueur
   */
  public setColor(color: string): void {
    this.color = color;
  }

  /**
   * Getter pour l'id du jeu
   * @returns l'id du jeu
   */
  public getIdGame(): string {
    return `${this.idGame}/`;
  }

  /**
   * Interprete le message envoyé par le serveur websocket
   * @param message message envoyé par le serveur websocket
   * @param state définit l'état de la partie (en cours ou terminée)
   */
  public interpret(message: string, state: { end: boolean, won: string, player1score: string, player2score: string}): void {
    if (message.length <= 3) {
      this.initIdGame(message);
    } else if (message.includes('x,y,color')) {
      this.updateTurn(message);
    } else if(message.includes("skipped")){
      this.skipTurn();
    }
    else if (message.includes('Start')) {
      this.startGame(message);
    } else if (message.includes('EndOfGame')) {
      let data = message.split(":")[1].split("|")
      let scores = data[0].split("-");
      state.player1score = scores[0];
      state.player2score = scores[1];
      state.won = data[1];
      console.log(state.won)
      state.end = true;
    }
    console.log(message);
  }

  private initIdGame(message: string): void {
    this.idGame = message.split('/')[0];
  }

  /**
   * Met à jour le plateau de jeu
   */
  private updateBoard(data: string) {
    let lines = data.split('\r\n');
    for (let i = 1; i < lines.length; i++) {
      let stoneData = lines[i].split(',');
      let x = stoneData[0];
      let y = stoneData[1];
      let color = stoneData[2];
      let stone = document.getElementById(`${x}-${y}`);
      this.discardKo(stone);
      switch (color) {
        case 'White':
          stone!.style.background = 'white';
          break;
        case 'Black':
          stone!.style.background = 'black';
          break;
        case 'Empty':
          stone!.style.background = 'transparent';
          break;
        case 'Ko':
          this.drawKo(stone);
      }
    }
  }

  private discardKo(stone: HTMLElement | null):void{
    if(stone != null){
      stone.style.border = "none";
      stone.style.borderRadius = "50%";
    }

  }

  private drawKo(stone: HTMLElement | null):void{
      stone!.style.borderRadius = "0";
      stone!.style.border = "5px solid #A7001E";
      stone!.style.boxSizing = "border-box";
      stone!.style.background = "transparent";

  }

  /**
   * Mets a jour le score des joueurs
   */
  private updateScore(score: string): void {
    let playerScore;
    let opponentScore;
    if (this.color == 'black') { 
      playerScore = score.split(';')[0];
      opponentScore = score.split(';')[1];
    } else {
      playerScore = score.split(';')[1];
      opponentScore = score.split(';')[0];
    }

    document.getElementById('opponent-score-value')!.innerHTML =
      'Prises : ' + opponentScore;
    document.getElementById('player-score-value')!.innerHTML =
      'Prises : ' + playerScore;
  }

  private updateTurn(message: string): void {
    let board = message.split('|')[0];
    let score = message.split('|')[1];
    this.updateBoard(board);
    this.updateScore(score);
    this.game.changeTurn();
    this.updateHover();
  }

  private skipTurn(){
    this.game.changeTurn();
    this.updateHover();
  }

  private startGame(message: string): void {
    this.game.initCurrentTurn();
    let pseudo = document.getElementById('pseudo-text');
    pseudo!.innerHTML = message.split(':')[1]; // Récupère le pseudo de l'adversaire pour l'afficher sur la page
    let profilePic = document.getElementById('opponent-pic') as HTMLImageElement;
    profilePic!.src = `https://localhost:7065/profile-pics/${pseudo!.innerText}`; // Récupère l'avatar de l'adversaire pour l'afficher sur la page
    this.updateHover();
  }

  public setGame(game:Game){
    this.game = game;
  } 

  public getPlayerColor():string{
    return this.game.getPlayerColor();
  }

  public getCurrentTurn():string{
    return this.game.getCurrentTurn();
  }

  public updateHover(){
    let stones = document.querySelectorAll(".stone, .bigger-stone");
    let stonesArray = Array.from(stones);
    if(this.game.isPlayerTurn()){
      document.getElementById("global-container")!.style.cursor = "pointer";
      stonesArray.forEach((stone)=>{
        stone.classList.add("active");
      })
    }else{
      document.getElementById("global-container")!.style.cursor = "not-allowed";
      stonesArray.forEach((stone)=>{
        stone.classList.remove("active");
      })
    }
  }
}
