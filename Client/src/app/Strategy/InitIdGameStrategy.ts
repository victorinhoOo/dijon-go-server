import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

const DEFAULT_ID_INDEX = 0;

/**
 * Implémenter la stratégie d'initialisation de l'id de la partie
 */
export class InitIdGameStrategy implements IStrategy {
    
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void
    {
        idGame.value = data[DEFAULT_ID_INDEX];
    }
}
