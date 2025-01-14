import { User } from "../../Model/User";
import { FakeUserDAO } from "../FakeDAO/FakeUserDAO";

describe('Test Rank', () => {
    let fakeUserDAO: FakeUserDAO;
    let users: User[];

    beforeEach(() => {
        fakeUserDAO = new FakeUserDAO();
        users = fakeUserDAO.getUsers(); // Récupère tous les utilisateurs du DAO
    });

    it('devrait retourner les bons rangs en fonction des Elo des utilisateurs', () => {
        // Test pour player1 avec Elo = 1200
        const player1 = users.find(user => user.Username === 'player1');
        expect(player1?.getRank()).toBe('8 Kyu');
        
        // Test pour player2 avec Elo = 1500
        const player2 = users.find(user => user.Username === 'player2');
        expect(player2?.getRank()).toBe('5 Kyu');

        // Test pour player3 avec Elo = 1800
        const player3 = users.find(user => user.Username === 'player3');
        expect(player3?.getRank()).toBe('2 Kyu');

        // Test pour bob avec Elo = 200
        const bob = users.find(user => user.Username === 'bob');
        expect(bob?.getRank()).toBe('18 Kyu');

        // Test pour alice avec Elo = 150
        const alice = users.find(user => user.Username === 'alice');
        expect(alice?.getRank()).toBe('19 Kyu');

        // Test pour un utilisateur avec un Elo supérieur à 3600 (9 Dan)
        const highEloUser = new User('highEloPlayer', 'highelo@example.com', 4000);
        expect(highEloUser.getRank()).toBe('9 Dan');

    });
});
