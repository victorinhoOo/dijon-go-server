import { IStrategy } from "./IStrategy";
import { Game } from "../Game";
import { ConnectedUsersService } from "../services/connected-users.service";

const USER_LIST_INDEX = 2; // Index pour la liste des utilisateurs

/**
 * Gère l'exécution de la stratégie pour la liste des utilisateurs.
 */
export class UserListStrategy implements IStrategy {
    constructor(private connectedUsersService: ConnectedUsersService) {}

    /**
     * Exécute la stratégie en traitant les données des utilisateurs connectés.
     * @param data - Tableau de chaînes de caractères contenant les données.
     * @param state - État du jeu contenant des informations sur la fin et le score.
     * @param idGame - Identifiant du jeu.
     * @param game - Instance du jeu.
     */
    public execute(
        data: string[],
        idGame: { value: string; },
        game: Game
    ): void {
        // Format attendu: UserList-user1,user2,user3
        if (data.length >= 1) {
            const users = data[USER_LIST_INDEX].split(','); 
            this.connectedUsersService.setConnectedUsers(users);
        }
    }
} 