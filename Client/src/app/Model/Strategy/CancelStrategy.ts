import { IStrategy } from './IStrategy';
import { Game } from '../Game';
import { Router } from '@angular/router';

/**
 * Classe CancelStrategy qui implémente l'interface IStrategy.
 * Cette classe gère la logique d'annulation d'une partie.
 */
export class CancelStrategy implements IStrategy {

    private router: Router;

    /**
     * Constructeur de la classe CancelStrategy.
     * Initialise une instance de Router.
     */
    public constructor(){
        this.router = new Router();
    }

    /**
     * Exécute la stratégie d'annulation.
     * @param data - Un tableau de chaînes de caractères contenant des données supplémentaires.
     * @param state - Un objet représentant l'état du jeu, incluant si le jeu est terminé, le résultat, et les scores des joueurs.
     * @param idGame - Un objet contenant la valeur de l'identifiant du jeu.
     * @param game - Une instance de la classe Game représentant le jeu en cours.
     */
    public execute(data: string[], idGame: {value: string}, game: Game): void
    {
        if(idGame.value != ""){
            idGame.value = "";
            this.router.navigate(['/cancelled']);
        }
    }
}

