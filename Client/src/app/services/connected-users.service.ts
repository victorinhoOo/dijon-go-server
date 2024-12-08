import { Injectable } from '@angular/core';
import { Observable } from '../Observer/Observable';
import { IObserver } from '../Observer/IObserver';

@Injectable({
  providedIn: 'root'
})
export class ConnectedUsersService extends Observable {
  private connectedUsers: string[] = [];

  public setConnectedUsers(users: string[]) {
    this.connectedUsers = users;
    this.notifyChange(this);
  }

  public getConnectedUsers(): string[] {
    return this.connectedUsers;
  }
} 