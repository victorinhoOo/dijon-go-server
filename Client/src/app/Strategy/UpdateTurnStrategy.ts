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
        game.changeTurn();
        game.setBoard(board);
        game.setCaptures(score);
        
    }
}
