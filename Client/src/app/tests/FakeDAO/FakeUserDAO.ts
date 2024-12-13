import { Observable, of, throwError } from 'rxjs';
import { User } from '../../Model/User';
import { RegisterUserDTO } from '../../Model/DTO/RegisterUserDTO';
import { LoginUserDTO } from '../../Model/DTO/LoginUserDTO';
import { UpdateUserDTO } from '../../Model/DTO/UpdateUserDTO';
import { IUserDAO } from '../../Model/DAO/IUserDAO';

export class FakeUserDAO implements IUserDAO {
  private users: User[] = []; // Simule une base de données en mémoire

  constructor() {
    // Ajout d'utilisateurs fictifs pour tester
    this.users.push(new User('player1', 'player1@example.com', 1200));
    this.users.push(new User('player2', 'player2@example.com', 1500));
    this.users.push(new User('player3', 'player3@example.com', 1800));
    this.users.push(new User('bob', 'bob@example.com', 200));
    this.users.push(new User('alice', 'alice@example.com', 150));
    
  }

  /**
   * Enregistre un nouvel utilisateur.
   * @param registerUserDTO Les données de l'utilisateur à enregistrer.
   * @returns Un Observable qui émet l'utilisateur créé ou une erreur.
   */
  registerUser(registerUserDTO: RegisterUserDTO): Observable<any> {
    const existingUser = this.users.find((u) => u.Username === registerUserDTO.Username);
    if (existingUser) {
      return throwError(() => new Error('Username already exists.'));
    }

    const newUser = new User(registerUserDTO.Username, registerUserDTO.Email, 1000); // Elo par défaut = 1000
    this.users.push(newUser);
    return of(newUser);
  }

  /**
   * Connecte un utilisateur.
   * @param loginUserDTO Les données de connexion.
   * @returns Un Observable qui émet l'utilisateur correspondant ou une erreur.
   */
  LoginUser(loginUserDTO: LoginUserDTO): Observable<any> {
    const user = this.users.find((u) => u.Username === loginUserDTO.Username);
    if (!user) {
      return throwError(() => new Error('User not found.'));
    }
    return of({ token: `fake-token-for-${user.Username}` }); // Simule un token
  }

  /**
   * Récupère les informations d'un utilisateur.
   * @param token Le token de l'utilisateur.
   * @returns Un Observable contenant un objet utilisateur.
   */
  GetUser(token: string): Observable<User> {
    const username = token.replace('fake-token-for-', ''); // Extrait le nom d'utilisateur depuis le token
    const user = this.users.find((u) => u.Username === username);
    if (!user) {
      return throwError(() => new Error('Invalid token.'));
    }
    return of(user);
  }

  /**
   * Met à jour les informations d'un utilisateur.
   * @param updateUserDTO Les nouvelles informations de l'utilisateur.
   * @returns Un Observable contenant un message de succès ou une erreur.
   */
  UpdateUser(updateUserDTO: UpdateUserDTO): Observable<any> {
    const user = this.users.find((u) => u.Username === updateUserDTO.Username);
    if (!user) {
      return throwError(() => new Error('User not found.'));
    }
  
    // Vérifie si le token correspond (simulation)
    const tokenMatches = `fake-token-for-${user.Username}` === updateUserDTO.TokenUser;
    if (!tokenMatches) {
      return throwError(() => new Error('Invalid token.'));
    }
  
    // Mise à jour des informations autorisées
    if (updateUserDTO.Email) {
      user.Email = updateUserDTO.Email;
    }
  
    // Vérifie si un changement de mot de passe est demandé
    if (updateUserDTO.OldPassword && updateUserDTO.Password) {
      // Simule une validation de l'ancien mot de passe (exemple simplifié)
      if (updateUserDTO.OldPassword !== 'fake-old-password') {
        return throwError(() => new Error('Old password is incorrect.'));
      }
    }
  
    return of({ message: 'User updated successfully.' });
  }
  

  /**
   * Récupère le classement des 5 meilleurs joueurs.
   * @returns Un Observable contenant un dictionnaire des utilisateurs et leurs scores.
   */
  GetLeaderboard(): Observable<{ [username: string]: number }> {
    const leaderboard = this.users
      .sort((a, b) => b.Elo - a.Elo) // Tri décroissant par Elo
      .slice(0, 5) // Top 5
      .reduce((acc, user) => {
        acc[user.Username] = user.Elo;
        return acc;
      }, {} as { [username: string]: number });

    return of(leaderboard);
  }
    // Getter pour obtenir tous les utilisateurs
    public getUsers(): User[] {
      return this.users;
    }
}
