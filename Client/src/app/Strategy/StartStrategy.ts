import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { environment } from '../environment';

const PROFILE_PIC_URL = environment.apiUrl + '/profile-pics/';
const TIMER_INTERVAL = 1000;

/**
 * Implémentation de la stratégie de démarrage de partie
 */
export class StartStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        game.initCurrentTurn();
        game.setOpponentPseudo(data[2]);
        setInterval(()=>{
            game.launchTimer();
        }, TIMER_INTERVAL);
        let board = data[3];
        this.updateBoard(board);
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
}
