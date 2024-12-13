import { Game } from '../Game';

/**
 * Interface pour les stratégies
 */
export interface IStrategy {
    /**
     * Exécute le code en fonction de la stratégie
     * @param data message envoyé par le serveur
     * @param state état de la partie
     * @param idGame identifiant de la partie
     * @param game partie en cours
     */
    execute(data: string[], idGame: {value: string}, game:Game,):void;
}