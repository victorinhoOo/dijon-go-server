import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AvailableGameInfoDTO } from '../DTO/AvailableGameInfoDTO';
import { environment } from '../../environment';
import { GameInfoDTO } from '../DTO/GameInfoDTO';

/**
 * Gère les requêtes HTTP vers l'API concernant les parties de jeu
 */
export class GameDAO {
  /**
   * Constructeur de la classe GameDAO.
   * Initialise HttpClient pour effectuer des requêtes HTTP
   * @param http HttpClient utilisé pour envoyer les requêtes HTTP
   */
  constructor(private http: HttpClient) {}

  /**
   * Récupère la liste des parties disponibles
   * @returns Un Observable qui émet la liste des parties disponibles
   */
  public GetAvailableGames(): Observable<AvailableGameInfoDTO[]> {
    return this.http
      .get<{ games: AvailableGameInfoDTO[] }>(
        `${environment.apiUrl}/Games/Available-games`
      )
      .pipe(map((response) => response.games));
  }

  /**
   * Récupère la liste des parties jouées par un joueur
   * @param token token utilisateur du joueur souhaitant récupérer la liste de ses parties
   * @returns Un Observable qui émet la liste des parties jouées
   */
  public GetGamesPlayed(token: string): Observable<GameInfoDTO[]> {
    const params = new HttpParams().set('token', token); 
    return this.http
      .post<{ games: GameInfoDTO[] }>(
        `${environment.apiUrl}/Games/Played-games`,null, { params }
      )
      .pipe(map((response) => response.games));
  }


}
