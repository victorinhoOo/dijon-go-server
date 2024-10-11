import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenKey = 'authToken'; // Clé pour le stockage du token
  private userPseudoKey = 'userName'; // Clé pour le stockage du pseudo utilisateur

  constructor(private cookieService: CookieService) {}

  // Méthode pour définir le token
  setToken(token: string): void {
    this.cookieService.set(this.tokenKey, token);
  }

  // Méthode pour obtenir le token
  getToken(): string {
    return this.cookieService.get(this.tokenKey);
  }

  // Méthode pour supprimer le token
  deleteToken(): void {
    this.cookieService.delete(this.tokenKey);
  }

  // Méthode pour définir le pseudo utilisateur
  setUserPseudo(pseudo: string): void {
    this.cookieService.set(this.userPseudoKey, pseudo);
  }

  // Méthode pour obtenir le pseudo utilisateur
  getUserPseudo(): string {
    return this.cookieService.get(this.userPseudoKey);
  }

  // Méthode pour supprimer le pseudo utilisateur
  deleteUserPseudo(): void {
    this.cookieService.delete(this.userPseudoKey);
  }
}
