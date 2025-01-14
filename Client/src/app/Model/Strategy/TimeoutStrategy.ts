import { IStrategy } from './IStrategy';
import { Game } from '../Game';
import { GamePopupDisplayer } from '../../IHM/GamePopupDisplayer';

/**
 * Implémentation de la stratégie de timeout
 */
export class TimeoutStrategy implements IStrategy {

    private popUpDisdaplayer: GamePopupDisplayer;

    public constructor() {
        this.popUpDisdaplayer = new GamePopupDisplayer();
    }
    public execute(data: string[], idGame: {value: string}, game:Game):void {
       this.popUpDisdaplayer.displayTimeOutPopup();
    }
}
