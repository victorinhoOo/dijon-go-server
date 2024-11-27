import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

/**
 * Implémenter la stratégie de fin de partie
 */
export class EndOfGameStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void{
        state.player1score = data[2];
        state.player2score = data[3];
        state.won = data[4];
        console.log(state.won)
        state.end = true;
    }
}
