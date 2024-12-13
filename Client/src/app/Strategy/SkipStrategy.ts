import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';

/**
 * Implémentation de la stratégie de passage de tour
 */
export class SkipStrategy implements IStrategy {
    public execute(data: string[], idGame: {value: string}, game:Game):void {
        game.changeTurn();
    }
}
