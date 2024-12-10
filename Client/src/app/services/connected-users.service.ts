import { Injectable } from '@angular/core';
import { Observable } from '../Observer/Observable';
import { IObserver } from '../Observer/IObserver';

@Injectable({
  providedIn: 'root'
})

/**
 * Service qui gère la liste des utilisateurs connectés.
 * Permet de définir et de récupérer les utilisateurs connectés,
 * ainsi que de notifier les observateurs des changements.
 */
export class ConnectedUsersService extends Observable {
  private connectedUsers: string[] = [];

  /**
   * Définit la liste des utilisateurs connectés.
   * Notifie les observateurs du changement.
   * @param users - Un tableau de chaînes représentant les utilisateurs connectés
   */
  public setConnectedUsers(users: string[]) {
    this.connectedUsers = users;
    this.notifyChange(this);
  }

  /**
   * Récupère la liste des utilisateurs connectés.
   * @returns Un tableau de chaînes représentant les utilisateurs connectés
   */
  public getConnectedUsers(): string[] {
    return this.connectedUsers;
  }
} 