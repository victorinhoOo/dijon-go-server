import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../Model/User'; 

@Injectable({
  providedIn: 'root',
})
/**
 * Gère le stockage des cookies utilisateurs et des tokens d'authenthification
 */
export class AuthService {
  private tokenKey = 'authToken';        // Clé pour le stockage du token
  private userKey = 'authUser';          // Clé pour le stockage de l'utilisateur

  constructor(private cookieService: CookieService) {}

  // Méthode pour définir le token
  public setToken(token: string): void {
    this.cookieService.set(this.tokenKey, token);
  }

  // Méthode pour obtenir le token
  public getToken(): string {
    return this.cookieService.get(this.tokenKey);
  }

  // Méthode pour supprimer le token
  public deleteToken(): void {
    this.cookieService.delete(this.tokenKey);
  }

  // Méthode pour définir l'utilisateur
  public setUser(user: User): void {
    const userData = JSON.stringify({ username: user.Username, email: user.Email });
    this.cookieService.set(this.userKey, userData);
  }

  // Méthode pour obtenir l'utilisateur
  public getUser(): User {
    const userData = this.cookieService.get(this.userKey);
    const { username, email } = JSON.parse(userData);
    return new User(username, email); // Création d'un nouvel objet User
  }

  // Méthode pour supprimer l'utilisateur
  public deleteUser(): void {
    this.cookieService.delete(this.userKey);
  }
}
