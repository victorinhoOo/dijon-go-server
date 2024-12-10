import { IStrategy } from "./IStrategy";
import { Game } from "../Model/Game";
import { ChatService } from "../services/chat.service";
import { UserCookieService } from "../Model/UserCookieService";
import { MessageDTO } from "../Model/DTO/MessageDTO";

const DATA_SENDER_INDEX = 2; // Index pour l'expéditeur
const DATA_MESSAGE_INDEX = 3; // Index pour le message

/**
 * Gère l'exécution des stratégies de chat dans le jeu.
 */
export class ChatStrategy implements IStrategy {
    constructor(private chatService: ChatService, private userCookieService: UserCookieService) {}

    /**
     * Exécute la stratégie de chat en traitant les données reçues.
     * @param data - Un tableau de chaînes contenant les informations du chat.
     * @param state - L'état actuel du jeu, incluant si le jeu est terminé et les scores des joueurs.
     * @param idGame - L'identifiant du jeu en cours.
     * @param game - L'objet représentant le jeu en cours.
     */
    public execute(
        data: string[],
        state: { end: boolean; won: string; player1score: string; player2score: string; },
        idGame: { value: string; },
        game: Game
    ): void {
        // Format reçu: 0-Chat-sender-message
        if (data.length >= 4) {
            const sender = data[DATA_SENDER_INDEX]; // Utilisation de la constante pour l'expéditeur
            const message = data[DATA_MESSAGE_INDEX]; // Utilisation de la constante pour le message
            const receiver = this.userCookieService.getUser()!.Username;
            const messageDTO = new MessageDTO(sender, receiver, message, new Date());
            this.chatService.addMessage(messageDTO);
        }
    }
} 