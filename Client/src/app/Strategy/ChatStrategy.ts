import { IStrategy } from "./IStrategy";
import { Game } from "../Model/Game";
import { ChatService } from "../services/chat.service";
import { UserCookieService } from "../Model/UserCookieService";

export class ChatStrategy implements IStrategy {
    constructor(private chatService: ChatService, private userCookieService: UserCookieService) {}

    public execute(
        data: string[],
        state: { end: boolean; won: string; player1score: string; player2score: string; },
        idGame: { value: string; },
        game: Game
    ): void {
        // Format reçu: 0-Chat-sender-message
        if (data.length >= 4) {
            const sender = data[2];
            const message = data[3];
            const receiver = this.userCookieService.getUser()!.Username;
            this.chatService.addMessage(sender, receiver, message);
        }
    }
} 