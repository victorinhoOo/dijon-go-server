import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { environment } from '../environment';

const PROFILE_PIC_URL = environment.apiUrl + '/profile-pics/';

/**
 * Implémentation de la stratégie de démarrage de partie
 */
export class StartStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        game.initCurrentTurn();
        game.setOpponentPseudo(data[2]);
        game.launchTimer();
        let board = data[3];
        game.setBoard(board);
    }
}
