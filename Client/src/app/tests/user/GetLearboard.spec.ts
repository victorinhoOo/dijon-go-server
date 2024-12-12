import { FakeUserDAO } from "../FakeDAO/FakeUserDAO";


describe('Test Leaderboard', () => {
    let dao: FakeUserDAO;
  
    beforeEach(() => {
      dao = new FakeUserDAO();
    });

 /**
  * Test la recuperation des meilleurs joueurs du jeu par elo
  */
  it('devrait récupérer le classement avec les 5 meilleurs utilisateurs triés par Elo', async () => {
    try {
      const leaderboard = await dao.GetLeaderboard().toPromise();
  
      if (!leaderboard) {
        fail('Leaderboard is undefined');
        return;
      }
  
      const sortedLeaderboard = Object.entries(leaderboard)
        .sort(([, eloA], [, eloB]) => eloB - eloA)
        .slice(0, 5);
  
      // Get the usernames of the top 5 users
      const topUsers = sortedLeaderboard.map(([username]) => username);
  
      expect(topUsers.length).toBeLessThanOrEqual(5); //verifie le nb des 5 meilleurs joueurs
      expect(topUsers[0]).toBe('player3'); //verifie que le joueur 3 est bien le meilleur
      expect(leaderboard['player3']).toBe(1800); //check l'elo du joueur
    } catch (error) {
      fail('erreur lors de la recuperation du leaderboard' + error);
    }
  });
});