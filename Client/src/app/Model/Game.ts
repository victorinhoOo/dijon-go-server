export class Game {
  private currentTurn: string;
  private playerColor: string;

  public constructor() {
    this.currentTurn = '';
    this.playerColor = '';
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

  private timerToMs(timer: string) {
    let minutes = Number(timer.split(':')[0]);
    let seconds = Number(timer.split(':')[1]);
    return (seconds + minutes * 60) * 1000;
  }


  /**
   * Trasforme un nombre de ms en un timer
   * @param ms nombre de ms
   * @returns un timer en string
   */
  public msToTimer(ms:string):string{
    let totalMs = Number(ms);
    let totalSeconds = Math.floor(totalMs/1000);
    let minutes = Math.floor(totalSeconds/60);
    let seconds = totalSeconds % 60;
    let stringMiniutes = minutes.toString().padStart(2,'0');
    let stringSeconds = seconds.toString().padStart(2,'0');
    let result = `${stringMiniutes}:${stringSeconds}`;
    return result;

  }


  /**
   * Lance le timer de la partie
   */
  public launchTimer() {
    if (this.playerColor == this.currentTurn) {
        let timer = document.getElementById("player-timer")!.innerText
        let ms = this.timerToMs(timer);
        ms -= 1000;
        timer = this.msToTimer(ms.toString());
        document.getElementById("player-timer")!.innerText = timer;
    }
    else{
        let timer = document.getElementById("opponent-timer")!.innerText
        let ms = this.timerToMs(timer);
        ms -= 1000;
        timer = this.msToTimer(ms.toString());
        document.getElementById("opponent-timer")!.innerText = timer;
    }
  }


  /**
   * Met à jour le hover des pierres
   */
  public updateHover(){
    let stones = document.querySelectorAll(".stone, .bigger-stone");
    let stonesArray = Array.from(stones);
    if(this.isPlayerTurn()){
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
