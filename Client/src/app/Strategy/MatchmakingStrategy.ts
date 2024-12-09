import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { WebsocketService } from '../websocket.service';
import { User } from '../Model/User';
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
    let idLobby = data[3]
    let opponentUsername = data[4];
    let opponentElo = data[5];
    if (data.includes('Create')) {
      setTimeout(
        () =>
          this.confirmMatchmaking(opponentUsername, opponentElo).then((result) => {
            if(result.isConfirmed){
              this.websocketService.createMatchmakingGame();            }
            else{
              this.websocketService.cancelMatchmaking(idLobby);
            }
            
          }),
        50
      );
    } else if(data.includes('Join')){
      let idLobby = data[0];
      setTimeout(
        () =>
          this.confirmMatchmaking(opponentUsername, opponentElo).then((result) => {
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

  private confirmMatchmaking(opponentUsername: string, opponentElo: string) {
    return Swal.fire({
      title: `Une partie a été trouvée contre ${opponentUsername} (Elo: ${opponentElo})`,
      text: 'Voulez-vous la rejoindre ?',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'Oui',
      cancelButtonText: 'Non',
      customClass: {
        confirmButton: 'custom-yes-button',
        cancelButton: 'custom-no-button',
        timerProgressBar: "custom-timer-progress-bar"
        
      },
      timer: 10000,
      timerProgressBar: true,
      didOpen: () => {
        
    }});
  }
}
