import { Observable } from 'rxjs';
import { MessageDTO } from '../DTO/MessageDTO';

/**
 * Interface pour le service MessageDAO, décrivant les méthodes disponibles.
 */
export interface IMessageDAO {
  /**
   * Récupère la conversation entre l'utilisateur courant et un autre utilisateur.
   * @param token Le token de l'utilisateur courant
   * @param usernameRecipient Le nom d'utilisateur du destinataire
   * @returns Un Observable contenant la liste des messages dans une propriété Messages.
   */
  GetConversation(token: string, usernameRecipient: string): Observable<{ Messages: MessageDTO[] }>;
}
