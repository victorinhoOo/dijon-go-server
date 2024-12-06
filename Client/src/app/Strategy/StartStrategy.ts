import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { environment } from '../environment';

const PROFILE_PIC_URL = environment.apiUrl + '/profile-pics/';
const TIMER_INTERVAL = 1000;

/**
 * Implémentation de la stratégie de démarrage de partie
 */
export class StartStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        game.initCurrentTurn();
        let pseudo = document.getElementById('pseudo-text');
        pseudo!.innerHTML = data[2]; // Récupère le pseudo de l'adversaire pour l'afficher sur la page
        let profilePic = document.getElementById('opponent-pic') as HTMLImageElement;
        profilePic!.src = `${PROFILE_PIC_URL}${pseudo!.innerText}`; // Récupère l'avatar de l'adversaire pour l'afficher sur la page
        game.updateHover();
        setInterval(()=>{
            game.launchTimer();
        }, TIMER_INTERVAL);
    }
}
