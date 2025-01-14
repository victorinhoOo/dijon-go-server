import { TestBed } from '@angular/core/testing';
import { IGameDAO } from '../../Model/DAO/IGameDAO';
import { FakeGameDAO } from '../FakeDAO/FakeGameDAO';

describe('Test Game', () => {
  let gameDAO: IGameDAO;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FakeGameDAO], // Fournir FakeGameDAO comme dépendance
    });

    gameDAO = TestBed.inject(FakeGameDAO); // Injecter FakeGameDAO
  });

  /**
   * Test recuperation des games
   */
  it('devrait récupérer la liste des parties disponibles', async () => {
    const games = await gameDAO.GetAvailableGames().toPromise();
  
    expect(games).toBeDefined(); // Vérifier que games n'est pas undefined
    expect(games?.length).toBe(2); // Vérifier qu'il y a bien deux parties
  
    // Vérifier si games[0] est défini avant d'accéder à ses méthodes
    if (games && games[0]) {
      expect(games[0].Id()).toBe(1);
      expect(games[0].Name()).toBe('Partie 1');
      expect(games[0].CreatorName()).toBe('Alice');
      expect(games[0].Handicap()).toBe(0);
      expect(games[0].HandicapColor()).toBe('Black');
    }
  
    // Vérifier si games[1] est défini avant d'accéder à ses méthodes
    if (games && games[1]) {
      expect(games[1].Id()).toBe(2);
      expect(games[1].Name()).toBe('Partie 2');
      expect(games[1].CreatorName()).toBe('Bob');
      expect(games[1].Handicap()).toBe(2);
      expect(games[1].HandicapColor()).toBe('White');
    }
  });
  
  

  /**
   * Test recuperation des games jouées
   */
  it('devrait récupérer la liste des parties jouées', async () => {
    const token = 'fake-token'; // Un token fictif pour tester
    const games = await gameDAO.GetGamesPlayed(token).toPromise(); // Utilisation de toPromise() pour attendre la réponse

    expect(games).toBeDefined(); // Vérifier que games n'est pas undefined ou null
    expect(games?.length).toBe(2); // Vérifier que deux parties sont retournées

    // Vérifier la première partie jouée
    if (games && games[0]) {  // Vérification explicite de la disponibilité de games et games[0]
      expect(games[0].Id()).toBe(1); // Utilisation de la méthode Id()
      expect(games[0].UsernamePlayer1()).toBe('Alice');
      expect(games[0].UsernamePlayer2()).toBe('Bob');
      expect(games[0].Size()).toBe(19);
      expect(games[0].Rule()).toBe('Japanese');
      expect(games[0].ScorePlayer1()).toBe(6.5);
      expect(games[0].ScorePlayer2()).toBe(5.0);
      expect(games[0].Won()).toBe(true);
      expect(games[0].Date()).toEqual(new Date('2024-12-12T00:00:00Z'));
    }

    // Vérifier la deuxième partie jouée
    if (games && games[1]) {  // Vérification explicite de la disponibilité de games et games[1]
      expect(games[1].Id()).toBe(2);
      expect(games[1].UsernamePlayer1()).toBe('Bob');
      expect(games[1].UsernamePlayer2()).toBe('Alice');
      expect(games[1].Size()).toBe(13);
      expect(games[1].Rule()).toBe('Chinese');
      expect(games[1].ScorePlayer1()).toBe(7.5);
      expect(games[1].ScorePlayer2()).toBe(7.0);
      expect(games[1].Won()).toBe(false);
      expect(games[1].Date()).toEqual(new Date('2024-12-10T00:00:00Z'));
    }
  });
});
