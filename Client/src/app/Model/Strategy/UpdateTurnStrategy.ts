import { IStrategy } from './IStrategy';
import { Game } from '../Game';

const BOARD_INDEX = 2;
const SCORE_INDEX_PLAYER = 3;
const SCORE_INDEX_OPPONENT = 4;
/**
 * Implémentation de la stratégie de changement de tour
 */
export class UpdateTurnStrategy implements IStrategy {

    public execute(data: string[], idGame: {value: string}, game:Game):void {
        let board = data[BOARD_INDEX];
        let score = `${data[SCORE_INDEX_PLAYER]};${data[SCORE_INDEX_OPPONENT]}`;
        game.changeTurn();
        game.setBoard(board);
        game.setCaptures(score);     
    }
}
