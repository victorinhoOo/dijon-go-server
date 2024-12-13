import { IStrategy } from './IStrategy';
import { Game } from '../Game';
import { WebsocketService } from '../services/websocket.service';
import { User } from '../User';
import Swal from 'sweetalert2';
import { GamePopupDisplayer } from '../../IHM/GamePopupDisplayer';

// Constantes pour les index de tableau
const INDEX_LOBBY = 3;
const INDEX_OPPONENT_USERNAME = 4;
const INDEX_OPPONENT_ELO = 5;
const INDEX_JOIN_LOBBY = 0;

/**
 * Implémentation de la stratégie de matchmaking
 */
export class MatchmakingStrategy implements IStrategy {
  private matchmakingResolve: ((value: void) => void) | null = null;

  private matchmakingPopupDisplayer: GamePopupDisplayer;

  public constructor(private websocketService: WebsocketService) {
    this.matchmakingPopupDisplayer = new GamePopupDisplayer();
  }

  /**
   * Exécute la stratégie de matchmaking.
   * @param data - Les données reçues du serveur.
   * @param state - L'état actuel du jeu.
   * @param idGame - L'identifiant du jeu.
   * @param game - L'objet de jeu.
   */
  public async execute(
    data: string[],
    idGame: { value: string },
    game: Game
  ): Promise<void> {
    if (this.matchmakingResolve && !data.includes("Retry")) {
      this.matchmakingResolve();
      this.matchmakingResolve = null;
    }
    let idLobby = data[INDEX_LOBBY]
    let opponentUsername = data[INDEX_OPPONENT_USERNAME];
    let opponentElo = data[INDEX_OPPONENT_ELO];
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
      let idLobby = data[INDEX_JOIN_LOBBY];
      setTimeout(
        () =>
          this.confirmMatchmaking(opponentUsername, opponentElo).then((result) => {
            if (result.isConfirmed) {
              let stringId = data[INDEX_JOIN_LOBBY];
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
    let user = new User(opponentUsername,'',parseInt(opponentElo));
     return this.matchmakingPopupDisplayer.displayMatchmakingPopup(user);
    
  }
}
