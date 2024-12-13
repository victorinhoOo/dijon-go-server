import { FakeUserDAO } from '../FakeDAO/FakeUserDAO';
import { RegisterUserDTO } from '../../Model/DTO/RegisterUserDTO';

describe('Test Register', () => {
  let dao: FakeUserDAO;

  beforeEach(() => {
    dao = new FakeUserDAO();
  });
  /**
   * Test d inscription d'un user
   */

  it('doit réussir à creer un utilisateur ', async () => {
    const newUserDTO = new RegisterUserDTO('newUser', 'newuser@example.com', 'password123', null);
    try {
      const user = await dao.registerUser(newUserDTO).toPromise();
      expect(user.Username).toBe('newUser');
      expect(user.Email).toBe('newuser@example.com');
      expect(user.Elo).toBe(1000);
    } catch (error) {
      fail('Erreur lors de la creation d un user valide' + error);
    }
  });

  /**
   * Test d'erreur d'inscription  d un user deja existant
   */
  it('ne doit pas réussir à creer un utilisateur ', async () => {
    const duplicateUserDTO = new RegisterUserDTO('player1', 'player1new@example.com', 'password123', null);
    try {
      await dao.registerUser(duplicateUserDTO).toPromise();
      fail('Erreur attendue pour un nom d utilisateur en double, mais l enregistrement a réussi.');
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : JSON.stringify(err);
      expect(errorMessage).toBe('Username already exists.');
    }
  });

  
});
