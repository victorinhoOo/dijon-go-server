import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

const PLAYER1_SCORE_INDEX = 2;
const PLAYER2_SCORE_INDEX = 3;
const WON_INDEX = 4;

/**
 * Implémente la stratégie de fin de partie
 */
export class EndOfGameStrategy implements IStrategy {
    public execute(data: string[], idGame: {value: string}, game:Game):void{
        let won = data[WON_INDEX] == "True";
        idGame.value = "";
        game.endGame(data[PLAYER1_SCORE_INDEX], data[PLAYER2_SCORE_INDEX], won);
    }
}
