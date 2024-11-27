import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { WebsocketService } from '../websocket.service';


export class MatchmakingStrategy implements IStrategy {
    private matchmakingResolve: ((value: void) => void) | null = null;

    public constructor(private websocketService: WebsocketService) {}

    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void
    {
        if (this.matchmakingResolve) {
            this.matchmakingResolve();
            this.matchmakingResolve = null;
          }
          if(data.includes("Create")){
            this.websocketService.createGame(19, "j", "matchmaking");
          }
          else {
            let stringId = data[0]
            let idGame = Number(stringId);
            this.websocketService.joinGame(idGame, "matchmaking", "j", 19);
          }
    }
}
