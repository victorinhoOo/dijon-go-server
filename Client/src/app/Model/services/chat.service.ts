import { Injectable } from '@angular/core';
import { Observable } from '../Observer/Observable';
import { MessageDTO } from '../DTO/MessageDTO';

/**
 * Service de chat qui gère les messages et les conversations.
 */
@Injectable({
  providedIn: 'root'
})
export class ChatService extends Observable {
  private messages: MessageDTO[] = [];
  private loadedConversations: Set<string> = new Set();

  constructor() {
    super();
  }

  /**
   * Ajoute un message au service de chat.
   * Vérifie si le message est un doublon avant de l'ajouter.
   * @param message Le message à ajouter.
   */
  public addMessage(message: MessageDTO) {
    const isDuplicate = this.messages.some(msg => msg.Id() === message.Id());
    if (!isDuplicate) {
      this.messages.push(message);
      this.notifyChange(this);
    }
  }

  /**
   * Récupère tous les messages du service de chat.
   * @returns Un tableau de MessageDTO contenant tous les messages.
   */
  public getMessages(): MessageDTO[] {
    return this.messages;
  }

  /**
   * Vérifie si une conversation a été chargée pour un utilisateur donné.
   * @param username Le nom d'utilisateur à vérifier.
   * @returns Vrai si la conversation est chargée, sinon faux.
   */
  public isConversationLoaded(username: string): boolean {
    return this.loadedConversations.has(username);
  }

  /**
   * Marque une conversation comme chargée pour un utilisateur donné.
   * @param username Le nom d'utilisateur dont la conversation est marquée comme chargée.
   */
  public markConversationAsLoaded(username: string): void {
    this.loadedConversations.add(username);
  }
} 