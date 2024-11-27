import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';


export class StartStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        game.initCurrentTurn();
        let pseudo = document.getElementById('pseudo-text');
        pseudo!.innerHTML = data[2]; // Récupère le pseudo de l'adversaire pour l'afficher sur la page
        let profilePic = document.getElementById('opponent-pic') as HTMLImageElement;
        profilePic!.src = `https://localhost:7065/profile-pics/${pseudo!.innerText}`; // Récupère l'avatar de l'adversaire pour l'afficher sur la page
        game.updateHover();
        setInterval(()=>{
            game.launchTimer();
        }, 1000);
    }
}
