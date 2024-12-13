import { Observable } from 'rxjs';
import { RegisterUserDTO } from '../DTO/RegisterUserDTO';
import { LoginUserDTO } from '../DTO/LoginUserDTO';
import { UpdateUserDTO } from '../DTO/UpdateUserDTO';
import { User } from '../User';

/**
 * Interface pour la gestion des utilisateurs via des requêtes HTTP
 */
export interface IUserDAO {
  /**
   * Enregistre un nouvel utilisateur
   * @param registerUserDTO Les informations nécessaires à l'enregistrement d'un utilisateur
   * @returns Un Observable émettant la réponse du serveur ou une erreur
   */
  registerUser(registerUserDTO: RegisterUserDTO): Observable<any>;

  /**
   * Connecte un utilisateur
   * @param loginUserDTO Les informations nécessaires à la connexion d'un utilisateur
   * @returns Un Observable émettant le token utilisateur ou une erreur
   */
  LoginUser(loginUserDTO: LoginUserDTO): Observable<any>;

  /**
   * Récupère les informations d'un utilisateur via son token
   * @param token Le token de l'utilisateur
   * @returns Un Observable émettant un objet User ou une erreur
   */
  GetUser(token: string): Observable<User>;

  /**
   * Met à jour les informations d'un utilisateur
   * @param user Les nouvelles informations de l'utilisateur
   * @returns Un Observable émettant un message de succès ou une erreur
   */
  UpdateUser(user: UpdateUserDTO): Observable<any>;

  /**
   * Récupère le classement des meilleurs joueurs
   * @returns Un Observable contenant un dictionnaire des joueurs et leurs Elos
   */
  GetLeaderboard(): Observable<{ [username: string]: number }>;
}
