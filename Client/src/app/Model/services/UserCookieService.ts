import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../User'; 
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
/**
 * Gère le stockage des cookies utilisateurs et des tokens d'authentification
 */
export class UserCookieService {
  private tokenKey = 'authToken';        // Clé pour le stockage du token
  private userKey = 'authUser';          // Clé pour le stockage de l'utilisateur

  private tokenSubject: BehaviorSubject<string>;
  private userSubject: BehaviorSubject<User | null>;

  constructor(private cookieService: CookieService) {
    this.tokenSubject = new BehaviorSubject<string>(this.getToken());
    this.userSubject = new BehaviorSubject<User | null>(this.getUser());
  }

  // Méthode pour définir le token
  public setToken(token: string): void {
    this.cookieService.set(this.tokenKey, token);
    this.tokenSubject.next(token);
  }

  /**
   * Méthode pour obtenir le token sous forme d'observable
   */
  public getTokenObservable() {
    return this.tokenSubject.asObservable();
  }

  // Méthode pour obtenir le token
  public getToken(): string {
    return this.cookieService.get(this.tokenKey);
  }

  // Méthode pour supprimer le token
  public deleteToken(): void {
    this.cookieService.delete(this.tokenKey);
    this.tokenSubject.next('');
  }

  // Méthode pour définir l'utilisateur
  public setUser(user: User): void {
    const userData = JSON.stringify({ username: user.Username, email: user.Email, elo: user.Elo });
    this.cookieService.set(this.userKey, userData);
    this.userSubject.next(user); // Mise à jour du BehaviorSubject pour notifier les abonnés
  }

  /**
   * Méthode pour obtenir l'utilisateur sous forme d'observable
   */
  public getUserObservable() {
    return this.userSubject.asObservable();
  }

  // Méthode pour obtenir l'utilisateur
  public getUser(): User | null {
    const userData = this.cookieService.get(this.userKey);
    let user: User | null = null; 
    if (userData) {
      const { username, email, elo } = JSON.parse(userData);
      user = new User(username, email, elo); 
    } 
    return user; 
  }

  // Méthode pour supprimer l'utilisateur
  public deleteUser(): void {
    this.cookieService.delete(this.userKey);
    this.userSubject.next(null); // Mettre à jour le BehaviorSubject pour indiquer qu'il n'y a plus d'utilisateur
  }

}
