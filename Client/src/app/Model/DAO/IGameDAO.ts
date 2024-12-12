import { Observable } from 'rxjs';
import { AvailableGameInfoDTO } from '../DTO/AvailableGameInfoDTO';
import { GameInfoDTO } from '../DTO/GameInfoDTO';

/**
 * Interface pour gérer les données liées aux parties de jeu
 */
export interface IGameDAO {
  /**
   * Récupère la liste des parties disponibles
   * @returns Un Observable qui émet la liste des parties disponibles
   */
  GetAvailableGames(): Observable<AvailableGameInfoDTO[]>;

  /**
   * Récupère la liste des parties jouées par un joueur
   * @param token Le token utilisateur du joueur souhaitant récupérer la liste de ses parties
   * @returns Un Observable qui émet la liste des parties jouées
   */
  GetGamesPlayed(token: string): Observable<GameInfoDTO[]>;
}
