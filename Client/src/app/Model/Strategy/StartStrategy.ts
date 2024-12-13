import { IStrategy } from './IStrategy';
import { Game } from '../Game';

const OPPONENT_PSEUDO_INDEX = 2;
const BOARD_INDEX = 3;

/**
 * Implémentation de la stratégie de démarrage de partie
 */
export class StartStrategy implements IStrategy {
    public execute(data: string[], idGame: {value: string}, game:Game):void {
        game.initCurrentTurn();
        game.setOpponentPseudo(data[OPPONENT_PSEUDO_INDEX]);
        game.launchTimer();
        let board = data[BOARD_INDEX];
        game.setBoard(board);
    }
}
