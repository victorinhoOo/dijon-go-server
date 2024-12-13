import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { RegisterUserDTO } from '../DTO/RegisterUserDTO';
import { LoginUserDTO } from '../DTO/LoginUserDTO';
import { UpdateUserDTO }from '../DTO/UpdateUserDTO';
import { User } from '../User';
import { HttpParams } from '@angular/common/http';
import { environment } from '../../environment';
import { IUserDAO } from './IUserDAO';

/**
 * Gère les requêtes HTTP vers l'API pour la gestion des comptes utilisateurs
 */
export class UserDAO implements IUserDAO{
  
  // URL de base pour les requêtes vers l'API utilisateur
  private readonly url = `${environment.apiUrl}/User/`;

  /**
   * Constructeur de la classe UserDAO.
   * Initialise HttpClient pour effectuer des requêtes HTTP
   * @param http HttpClient utilisé pour envoyer les requêtes HTTP
   */
  constructor(private http: HttpClient) {}

  /**
   * Enregistre un nouvel utilisateur
   * @param registerUserDTO Les informations nécessaires à l'enregistrement d'un utilisateur
   * @returns Un Observable émettant la réponse du serveur ou une erreur
   */
  public registerUser(registerUserDTO: RegisterUserDTO): Observable<any> {
    // On passe par un formdata pour envoyer les données car on envoie un fichier (l'image de profil)
    const formData: FormData = new FormData();  
    formData.append('username', registerUserDTO.Username || '');
    formData.append('email', registerUserDTO.Email || '');
    formData.append('password', registerUserDTO.Password || '');   
    // Ajoute le fichier si disponible
    if (registerUserDTO.ProfilePic) {
      formData.append('profilePic', registerUserDTO.ProfilePic);
    }
    return this.http.post<{ message: string }>(this.url + 'Register', formData).pipe(
      catchError(error => {
        // Gestion des erreurs HTTP et remontée d'un message d'erreur
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Connecte un utilisateur
   * @param loginUserDTO Les informations nécessaires à la connexion d'un utilisateur
   * @returns Un Observable émettant le token utilisateur ou une erreur
   */
  public LoginUser(loginUserDTO: LoginUserDTO): Observable<any> {
    return this.http.post<{ token: string, message?: string }>(this.url + 'Login', loginUserDTO).pipe(
      catchError(error => {
        // Gestion des erreurs HTTP et remontée d'un message d'erreur
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Envoie une requête POST pour se connecter avec Google
   * @param idToken le token Google de l'utilisateur
   */
  public GoogleLogin(idToken: string) {
    const params = new HttpParams().set('idToken', idToken);
    return this.http.post<{ token: string }>(`${this.url}GoogleLogin`, null, { params });
  }

  /**
   * Récupère les informations d'un utilisateur via son token
   * @param token Le token de l'utilisateur
   * @returns Un Observable émettant un objet User ou une erreur
   */
  public GetUser(token: string): Observable<User> {
    const params = new HttpParams().set('tokenUser', token);
    return this.http.get<{ user: { username: string, email: string, elo: number } }>(this.url + 'Get', { params }).pipe(
      map((response: { user: { username: string, email: string, elo: number } }) => {
        // Créé un nouvel objet User à partir de l'objet  renvoyé par le serveur
        return new User(response.user.username, response.user.email,response.user.elo);
      }),
      catchError(error => {
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Met à jour les informations d'un utilisateur
   * @param user Les nouvelles informations de l'utilisateur
   * @returns Un Observable émettant un message de succès ou une erreur
   */
  public UpdateUser(user: UpdateUserDTO): Observable<any> {
    // On passe par un formdata pour envoyer les données car on envoie un fichier (l'image de profil)
    const formData: FormData = new FormData();  
    formData.append('tokenuser', user.TokenUser);
    formData.append('username', user.Username || '');
    formData.append('email', user.Email || '');
    formData.append('oldpassword', user.OldPassword || '');
    formData.append('password', user.Password || '');   
    // Ajoute le fichier si disponible
    if (user.ProfilePic) {
      formData.append('profilePic', user.ProfilePic);
    }
    return this.http.post<{ message: string }>(this.url + 'Update', formData).pipe(
      catchError(error => {
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Récupère le classement des meilleurs joueurs
   * @returns Un Observable contenant un dictionnaire des joueurs et leurs Elos
   */

  public GetLeaderboard(): Observable<{ [username: string]: number }> {
    return this.http.get<{ [username: string]: number }>((this.url) + 'Leaderboard').pipe(
      map((response) => response),
      catchError((error) =>
        throwError(() => new Error(error.error?.message || 'Erreur lors de la récupération du leaderboard'))
      )
    );
  }


}


