import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

export class InitIdGameStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void
    {
        idGame.value = data[0];
    }
}
