import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IGameDAO } from '../../Model/DAO/IGameDAO';
import { AvailableGameInfoDTO } from '../../Model/DTO/AvailableGameInfoDTO';
import { GameInfoDTO } from '../../Model/DTO/GameInfoDTO';

/**
 * Implémentation simulée de l'interface IGameDAO pour les tests
 */
@Injectable({
  providedIn: 'root', // Enregistrer le service au niveau de l'application
})
export class FakeGameDAO implements IGameDAO {
  
  constructor() {}

  /**
   * Simule la récupération des parties disponibles.
   * @returns Un Observable contenant une liste simulée de parties disponibles.
   */
  public GetAvailableGames(): Observable<AvailableGameInfoDTO[]> {
    const mockAvailableGames = [
      new AvailableGameInfoDTO(1, 19, 'Japanese', 'Alice', 6.5, 'Partie 1', 0, 'Black'),
      new AvailableGameInfoDTO(2, 13, 'Chinese', 'Bob', 7.5, 'Partie 2', 2, 'White'),
    ];

    return of(mockAvailableGames); // Retourne un observable avec des données simulées
  }

  /**
   * Simule la récupération des parties jouées par un joueur en utilisant un token.
   * @param token Le token utilisateur du joueur.
   * @returns Un Observable contenant une liste simulée de parties jouées.
   */
  public GetGamesPlayed(token: string): Observable<GameInfoDTO[]> {
    const mockGamesPlayed = [
      new GameInfoDTO(1, 'Alice', 'Bob', 19, 'Japanese', 6.5, 5.0, true, new Date('2024-12-12T00:00:00Z')),
      new GameInfoDTO(2, 'Bob', 'Alice', 13, 'Chinese', 7.5, 7.0, false, new Date('2024-12-10T00:00:00Z')),
    ];

    return of(mockGamesPlayed); // Retourne un observable avec des données simulées
  }
}
