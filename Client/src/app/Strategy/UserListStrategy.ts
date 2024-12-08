import { IStrategy } from "./IStrategy";
import { Game } from "../Model/Game";
import { ConnectedUsersService } from "../services/connected-users.service";

export class UserListStrategy implements IStrategy {
    constructor(private connectedUsersService: ConnectedUsersService) {}

    public execute(
        data: string[],
        state: { end: boolean; won: string; player1score: string; player2score: string; },
        idGame: { value: string; },
        game: Game
    ): void {
        // Format attendu: UserList-user1,user2,user3
        if (data.length >= 1) {
            const users = data[2].split(',');
            this.connectedUsersService.setConnectedUsers(users);
        }
    }
} 