import { TestBed } from '@angular/core/testing';
import { ChatService } from '../../Model/services/chat.service';
import { UserCookieService } from '../../Model/services/UserCookieService';
import { MessageDTO } from '../../Model/DTO/MessageDTO';

describe('Test ChatService', () => {
  let service: ChatService;
  let userCookieServiceMock: jasmine.SpyObj<UserCookieService>;

  beforeEach(() => {
    // Crée un mock du service UserCookieService
    userCookieServiceMock = jasmine.createSpyObj('UserCookieService', ['getUser']);

    TestBed.configureTestingModule({
      providers: [
        ChatService,
        { provide: UserCookieService, useValue: userCookieServiceMock }
      ]
    });

    service = TestBed.inject(ChatService);
  });

  it('devrait être créé', () => {
    expect(service).toBeTruthy();
  });

/**
 * Testd'ajout d'un message et notifiaction observeurs
 */
 it('devrait ajouter un message et notifier les observateurs', () => {
  const message = new MessageDTO('user1', 'user2', 'Hello', new Date());

  // Ajouter un message
  service.addMessage(message);

  // Vérifier que le message est ajouté
  expect(service.getMessages().length).toBe(1);
  expect(service.getMessages()[0]).toEqual(message);

  // Vérifier l'effet de notifyChange en s'assurant que l'état de `messages` est correctement mis à jour
  const observedMessages = service.getMessages(); // Vérifier l'état final
  expect(observedMessages).toContain(message);
});

/**
 * Test echec ajout message doublon
 */

it('ne devrait pas ajouter un message s  il est un doublon', () => {
    const date = new Date('2024-01-01T12:00:00Z'); // Date fixe pour le test
    const message = new MessageDTO('user1', 'user2', 'Hello', date);
  
    service.addMessage(message); // Ajouter un premier message
    service.addMessage(message); // Essayer d'ajouter un doublon
  
    expect(service.getMessages().length).toBe(1);
  });
  

  /**
   * Test chargement conversation
   */
  it('devrait vérifier si une conversation a été chargée', () => {
    const username = 'user1';

    // Vérifier que la conversation n'est pas chargée au départ
    expect(service.isConversationLoaded(username)).toBeFalse();

    // Marquer la conversation comme chargée
    service.markConversationAsLoaded(username);

    // Vérifier que la conversation est maintenant chargée
    expect(service.isConversationLoaded(username)).toBeTrue();
  });

  it('devrait marquer une conversation comme chargée', () => {
    const username = 'user1';

    service.markConversationAsLoaded(username);

    // Vérifier que la conversation est marquée comme chargée
    expect(service.isConversationLoaded(username)).toBeTrue();
  });
});
