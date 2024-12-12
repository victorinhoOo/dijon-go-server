import { LoginUserDTO } from "../../Model/DTO/LoginUserDTO";
import { FakeUserDAO } from "../FakeDAO/FakeUserDAO";


describe('Test GetUser', () => {
    let dao: FakeUserDAO;
  
    beforeEach(() => {
      dao = new FakeUserDAO();
    });

    /**
     * test recuperation des info du user
     */
    it('doit récupérer les informations de l utilisateur avec token valide', async () => {
        const token = 'fake-token-for-player1';
        try {
          const user = await dao.GetUser(token).toPromise();
          expect(user?.Username).toBe('player1');
          expect(user?.Email).toBe('player1@example.com');
          expect(user?.Elo).toBe(1200);
        } catch (error) {
          fail('N a pas reussi à recuperer les info de l utilisateur avec son token' + error);
        }
      });
    /** 
     *ne doit pas trouver le user
    */
      it('Ne doit pas récupérer les informations de l utilisateur avec token invalide', async () => {
        const token = 'fake-token-for-nonexistent';
        try {
          await dao.GetUser(token).toPromise();
          fail('collecte des informations d un user avec un token invalide');
        } catch (err) {
          const errorMessage = err instanceof Error ? err.message : JSON.stringify(err);
          expect(errorMessage).toBe('Invalid token.');
        }
      });
    
});