export class Game {
  private currentTurn: string;
  private playerColor: string;

  public constructor() {
    this.currentTurn = '';
    this.playerColor = '';
  }

  public changeTurn(): void {
    this.currentTurn = this.currentTurn == 'black' ? 'white' : 'black';
  }

  public getCurrentTurn(): string {
    return this.currentTurn;
  }

  public getPlayerColor(): string {
    return this.playerColor;
  }

  public initCurrentTurn() {
    this.currentTurn = 'black';
  }

  public setPlayerColor(color: string) {
    this.playerColor = color;
  }

  public isPlayerTurn(): boolean {
    return this.playerColor == this.currentTurn;
  }

  private timerToMs(timer: string) {
    let minutes = Number(timer.split(':')[0]);
    let seconds = Number(timer.split(':')[1]);
    return (seconds + minutes * 60) * 1000;
  }

  public msToTimer(ms:string):string{
    let totalMs = Number(ms);
    let totalSeconds = Math.floor(totalMs/1000);
    let minutes = Math.floor(totalSeconds/60);
    let seconds = totalSeconds % 60;
    let result = `${minutes.toString().padStart(2,'0')}:${seconds.toString().padStart(2,'0')}`;
    return result;

  }

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
}
