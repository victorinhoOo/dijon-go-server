import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { MessageDTO } from '../DTO/MessageDTO';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})

/**
 * Gère les requêtes liées aux messages entre deux joueurs
 */
export class MessageDAO implements MessageDAO {
  private baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.apiUrl + '/Messages';
  }

   /**
   * Récupère la conversation entre l'utilisateur courant et un autre utilisateur.
   * @param token Le token de l'utilisateur courant
   * @param usernameRecipient Le nom d'utilisateur du destinataire
   * @returns Un Observable contenant la liste des messages dans une propriété Messages.
   */
  public GetConversation(token: string, usernameRecipient: string): Observable<{ Messages: MessageDTO[] }> {
    const params = {
      token: token,
      usernameRecipient: usernameRecipient
    };

    return this.http.post<any>(
      `${this.baseUrl}/Conversation`,
      null,
      { params }
    ).pipe(
      map(response => ({
        Messages: response.messages.map((msg: any) => new MessageDTO(
          msg.senderUsername,
          msg.receiverUsername,
          msg.content,
          new Date(msg.timestamp)
        ))
      }))
    );
  }
}
