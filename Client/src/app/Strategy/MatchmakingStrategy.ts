import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { WebsocketService } from '../websocket.service';
import Swal from 'sweetalert2';

/**
 * Implémentation de la stratégie de matchmaking
 */
export class MatchmakingStrategy implements IStrategy {
  private matchmakingResolve: ((value: void) => void) | null = null;

  public constructor(private websocketService: WebsocketService) {}

  public async execute(
    data: string[],
    state: {
      end: boolean;
      won: string;
      player1score: string;
      player2score: string;
    },
    idGame: { value: string },
    game: Game
  ): Promise<void> {
    if (this.matchmakingResolve && !data.includes("Retry")) {
      this.matchmakingResolve();
      this.matchmakingResolve = null;
    }
    if (data.includes('Create')) {
      let idLobby = data[3]
      setTimeout(
        () =>
          this.confirmMatchmaking().then((result) => {
            if(result.isConfirmed){
              this.websocketService.createGame(19, 'j', 'matchmaking');
            }
            else{
              this.websocketService.cancelMatchmaking(idLobby);
            }
            
          }),
        50
      );
    } else if(data.includes('Join')){
      let idLobby = data[0];
      console.log(idLobby);
      setTimeout(
        () =>
          this.confirmMatchmaking().then((result) => {
            if (result.isConfirmed) {
              let stringId = data[0];
              let idGame = Number(stringId);
              this.websocketService.joinGame(idGame, 'matchmaking', 'j', 19);
            }
            else{
              this.websocketService.cancelMatchmaking(idLobby);
            }
          }),
        50
      );
    }else if(data.includes("Retry")){
      await this.websocketService.joinMatchmaking();
    }
  }

  private confirmMatchmaking() {
    return Swal.fire({
      title: 'Une partie a été trouvée !',
      text: 'Voulez-vous la rejoindre ?',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'Oui',
      cancelButtonText: 'Non',
    });
  }
}
