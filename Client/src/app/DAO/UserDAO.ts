import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';
import { UpdateUserDTO }from '../Model/DTO/UpdateUserDTO';
import { User } from '../Model/User';
import { HttpParams } from '@angular/common/http';

/**
 * Gère les requêtes HTTP vers l'API utilisateur
 */
export class UserDAO {
  
  // URL de base pour les requêtes vers l'API utilisateur
  private readonly url = 'https://localhost:7065/User/'; 

  /**
   * Constructeur de la classe UserDAO.
   * Initialise HttpClient pour effectuer des requêtes HTTP
   * @param http HttpClient utilisé pour envoyer les requêtes HTTP
   */
  constructor(private http: HttpClient) {}

  /**
   * Envoie une requête POST au serveur pour enregistrer un nouvel utilisateur
   * @param user Objet contenant les données de l'utilisateur à enregistrer (RegisterUserDTO)
   * @returns Un Observable qui émet la réponse du serveur ou une erreur
   */
  public registerUser(registerUserDTO: RegisterUserDTO): Observable<any> {
    return this.http.post<{ message: string }>(this.url + 'Register', registerUserDTO).pipe(
      catchError(error => {
        // Gestion des erreurs HTTP et remontée d'un message d'erreur
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Envoie une requête POST pour se connecter au serveur
   * @param loginUserDTO Objet contenant les données de l'utilisateur (LoginUserDTO)
   * @returns Un Observable qui émet la réponse du serveur ou une erreur
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
   * Envoie une requête GET pour récupérer les informations de l'utilisateur
   * @param token token utilisateur
   * @returns Les informations de l'utilisateur sous forme d'objet
  **/
  public GetUser(token: string): Observable<User> {
    const params = new HttpParams().set('tokenUser', token);
    return this.http.get<{ user: { username: string, email: string } }>(this.url + 'Get', { params }).pipe(
      map((response: { user: { username: string, email: string } }) => {
        // Créé un nouvel objet User à partir de l'objet  renvoyé par le serveur
        return new User(response.user.username, response.user.email);
      }),
      catchError(error => {
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

  /**
   * Envoie une requête POST pour mettre à jour les informations de l'utilisateur
   * @param user L'utilisateur à modifier (UpdateUserDTO)
   * @returns un message d'erreur ou de succès
   */
  public UpdateUser(user: UpdateUserDTO): Observable<any>
  {
    return this.http.post<{ message: string }>(this.url + 'Update', user).pipe(
      catchError(error => {
        // Gestion des erreurs HTTP et remontée d'un message d'erreur
        return throwError(() => new Error(error.error?.message || 'Erreur de connexion au serveur'));
      })
    );
  }

}


