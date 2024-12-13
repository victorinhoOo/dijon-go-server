import { LoginUserDTO } from "../../Model/DTO/LoginUserDTO";
import { FakeUserDAO } from "../FakeDAO/FakeUserDAO";


describe('Test Connexion', () => {
    let dao: FakeUserDAO;
  
    beforeEach(() => {
      dao = new FakeUserDAO();
    });

    /**
     * test la connection d'un user avec token valide
     */
    it('doit connecter un utilisateur existant et renvoyer un token', async () => {
        const loginUserDTO = new LoginUserDTO('player1', 'password123');
        try {
          const response = await dao.LoginUser(loginUserDTO).toPromise();
          expect(response.token).toBe('fake-token-for-player1');
        } catch (error) {
          fail('Erreur lors de la connexion' + error);
        }
      });
    
    /**
     * test la connection d'un user avec token non valide
     */
      it('ne doit pas connecter un utilisateur ', async () => {
        const loginUserDTO = new LoginUserDTO('nonexistent', 'password123');
        try {
          await dao.LoginUser(loginUserDTO).toPromise();
          fail('connecte un utilisateur invalide');
        } catch (err) {
          const errorMessage = err instanceof Error ? err.message : JSON.stringify(err);
          expect(errorMessage).toBe('User not found.');
        }
      });
});