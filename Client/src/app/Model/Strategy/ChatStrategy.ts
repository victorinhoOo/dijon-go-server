import { IStrategy } from "./IStrategy";
import { Game } from "../Game";
import { ChatService } from "../services/chat.service";
import { UserCookieService } from "../services/UserCookieService";
import { MessageDTO } from "../DTO/MessageDTO";

// Constantes pour les indices du tableau data
const DATA_ACTION_INDEX = 1;  // Index pour l'action (Chat)
const DATA_SENDER_INDEX = 2;  // Index pour l'expéditeur
const DATA_MESSAGE_INDEX = 3; // Index pour le message
const DATA_TIMESTAMP_INDEX = 4; // Index pour le timestamp
const MINIMUM_DATA_LENGTH = 4; // Longueur minimale attendue du tableau data

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
        idGame: { value: string; },
        game: Game
    ): void {
        // Format reçu: 0-Chat-sender-message
        if (data.length >= MINIMUM_DATA_LENGTH) {
            const sender = data[DATA_SENDER_INDEX];
            const message = data[DATA_MESSAGE_INDEX];
            const receiver = this.userCookieService.getUser()!.Username;
            
            // Reformate la date pour qu'elle soit valide en format ISO 8601
            const timestampString = data[DATA_TIMESTAMP_INDEX];
            const [datePart, timePart] = timestampString.split(' ');
            const [day, month, year] = datePart.split('/');
            const formattedDate = `${year}-${month}-${day}T${timePart}`; // Format ISO 8601          
            const timestamp = new Date(formattedDate);
            const messageDTO = new MessageDTO(sender, receiver, message, timestamp);
            this.chatService.addMessage(messageDTO);
        }
    }
} 