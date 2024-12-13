import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AvailableGameInfoDTO } from '../DTO/AvailableGameInfoDTO';
import { environment } from '../../environment';
import { GameInfoDTO } from '../DTO/GameInfoDTO';
import { GameStateDTO } from '../DTO/GameStateDTO';
import { Injectable } from '@angular/core';
import { IGameDAO } from './IGameDAO';

/**
 * Gère les requêtes HTTP vers l'API concernant les parties de jeu
 */
@Injectable({
  providedIn: 'root'
})
export class GameDAO implements IGameDAO{
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
   * @param token Le token utilisateur du joueur souhaitant récupérer la liste de ses parties
   * @returns Un Observable qui émet la liste des parties jouées
   */
  public GetGamesPlayed(token: string): Observable<GameInfoDTO[]> {
    const params = new HttpParams().set('token', token);
    return this.http
      .post<{ games: GameInfoDTO[] }>(
        `${environment.apiUrl}/Games/Played-games`,
        null,
        { params }
      )
      .pipe(map((response) => response.games));
  }

  /**
   * Récupère les informations d'une partie en fonction de son identifiant
   * @param id identifiant de la partie
   * @returns les informations de la partie
   */
  public GetGameById(id: number): Observable<GameInfoDTO> {
    const params = new HttpParams().set('id', id);
    return this.http
      .post<{ game: GameInfoDTO }>(`${environment.apiUrl}/Games/Game`, null, {
        params,
      })
      .pipe(map((response) => response.game));
  }

  /**
   * Récupère les états d'une partie en fonction de son identifiant
   * @param id identifiant de la partie
   * @returns les états de la partie
   */
  public GetGameStatesById(id: number): Observable<GameStateDTO[]> {
    const params = new HttpParams().set('id', id);
    return this.http
      .post<{ states: GameStateDTO[] }>(
        `${environment.apiUrl}/Games/Game-states`,
        null,
        { params }
      )
      .pipe(map((response) => response.states));
  }


  /**
   * Récupère l'identifiant de la dernière partie jouée par un joueur
   * @param token token utilisateur du joueur souhaitant récupérer l'identifiant de sa dernière partie
   * @returns l'id de sa dernière partie jouée
   */
  public GetLastGameId(token: string): Observable<number> {
    const params = new HttpParams().set('token', token);
    return this.http
      .post<{ id: number }>(`${environment.apiUrl}/Games/Last-game-id`, null, {
        params,
      })
      .pipe(map((response) => response.id));
  }
}
