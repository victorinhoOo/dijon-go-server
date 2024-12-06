import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

const BOARD_INDEX = 2;
const SCORE_INDEX_PLAYER = 3;
const SCORE_INDEX_OPPONENT = 4;
const TIMER_INDEX = 5;
/**
 * Implémentation de la stratégie de changement de tour
 */
export class UpdateTurnStrategy implements IStrategy {

    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        let board = data[BOARD_INDEX];
        let score = `${data[SCORE_INDEX_PLAYER]};${data[SCORE_INDEX_OPPONENT]}`;
        let timer = data[TIMER_INDEX];
        this.updateBoard(board);
        this.updateScore(score, game);
        this.updateTimer(timer, game);
        game.changeTurn();
        this.updateHover(game);
    }


    private updateScore(score: string, game:Game): void {
        let playerScore;
        let opponentScore;
        if (game.getPlayerColor() == 'black') { 
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

    private updateBoard(data: string) {
        let lines = data.split('\r\n');
        const colorMap: { [key: string]: string } = {
            'White': 'white',
            'Black': 'black',
            'Empty': 'transparent'
        };

        for (let i = 1; i < lines.length; i++) {
            let stoneData = lines[i].split(',');
            let x = stoneData[0];
            let y = stoneData[1];
            let color = stoneData[2];
            let stone = document.getElementById(`${x}-${y}`);
            this.discardKo(stone);

            if (colorMap[color]) {
                stone!.style.background = colorMap[color];
            } else if (color === 'Ko') {
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

    private updateTimer(ms:string, game:Game):void{
        let timer = game.msToTimer(ms)
        if(game.getPlayerColor() == game.getCurrentTurn()){
          document.getElementById("player-timer")!.innerText = timer
        }
        else{
          document.getElementById("opponent-timer")!.innerText = timer;
        }
    }

    public updateHover(game:Game):void{
        let stones = document.querySelectorAll(".stone, .bigger-stone");
        let stonesArray = Array.from(stones);
        if(game.isPlayerTurn()){
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
