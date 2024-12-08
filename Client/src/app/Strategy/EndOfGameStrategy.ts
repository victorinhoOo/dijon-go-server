import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

const PLAYER1_SCORE_INDEX = 2;
const PLAYER2_SCORE_INDEX = 3;
const WON_INDEX = 4;

/**
 * Implémenter la stratégie de fin de partie
 */
export class EndOfGameStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void{
        state.player1score = data[PLAYER1_SCORE_INDEX];
        state.player2score = data[PLAYER2_SCORE_INDEX];
        state.won = data[WON_INDEX];
        state.end = true;
        idGame.value = "";
        game.endGame();
    }
}
