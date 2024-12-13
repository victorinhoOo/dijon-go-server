import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WebsocketService } from '../../../Model/services/websocket.service';
import { ConnectedUsersService } from '../../../Model/services/connected-users.service';
import { ChatService } from '../../../Model/services/chat.service';
import { IObserver } from '../../../Model/Observer/IObserver';
import { Observable } from '../../../Model/Observer/Observable';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { MatIconModule } from '@angular/material/icon';
import { MessageDAO } from '../../../Model/DAO/MessageDAO';
import { MessageDTO } from '../../../Model/DTO/MessageDTO';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})

/**
 * Composant qui permet de voir la liste des joueurs connectés et de commencer une conversation avec un joueur
 */
export class PlayerListComponent implements OnInit, IObserver {
  private connectedPlayers: string[] = [];
  
  private selectedPlayer: string | null = null;
  
  private currentUser: string;

  private messages: MessageDTO[] = [];
  
  private newMessage: string = '';
  
  private unreadMessageCounts: Map<string, number> = new Map();

  @ViewChild('messageContainer') private messageContainer!: ElementRef;
  

  private isChatOpen: boolean = false;

 /**
  * Renvoi les joueurs connectés
  */
  public ConnectedPlayers = () => this.connectedPlayers;

  /**
   * Renvoi le joueur sélectionné pour la conversation
   */
  public SelectedPlayer = () => this.selectedPlayer;

  /**
   * Renvoi les messages de la conversation
   */
  public Messages = () => this.messages;

  /**
   * Renvoi le message en cours de rédaction
   */
  public NewMessage = () => this.newMessage;

  /**
   * Renvoi le nom d'utilisateur du joueur actuel
   */
  public CurrentUser = () => this.currentUser;

  /**
   * Renvoi l'état d'ouverture du panneau de messagerie
   */
  public IsChatOpen = () => this.isChatOpen;

  /**
   * Modifie le message en cours de rédaction
   * @param value la nouvelle valeur du message
   */
  public SetNewMessage = (value: string) => this.newMessage = value;

  constructor(
    private connectedUsersService: ConnectedUsersService,
    private chatService: ChatService,
    private websocketService: WebsocketService,
    private userCookieService: UserCookieService,
    private messageDAO: MessageDAO
  ) {
    this.currentUser = this.userCookieService.getUser()!.Username;
  }

  // Initialisation
  public ngOnInit() {
    this.initializeServices();
    this.initializeConnectedPlayers();
  }

  /**
   * Initialise les services nécessaires et l'utilisateur courant.
   */
  private initializeServices() {
    this.connectedUsersService.register(this);
    this.chatService.register(this);
    this.currentUser = this.userCookieService.getUser()!.Username;
  }

  /**
   * Initialise la liste des joueurs connectés en excluant l'utilisateur courant.
   */
  private initializeConnectedPlayers() {
    this.connectedPlayers = this.connectedUsersService.getConnectedUsers()
      .filter(player => player !== this.currentUser);
    this.initializeUnreadCounts();
  }

  /**
   * Initialise le compteur de messages non lus pour chaque joueur connecté.
   */
  private initializeUnreadCounts() {
    this.connectedPlayers.forEach(player => {
      this.unreadMessageCounts.set(player, 0);
    });
  }

  // Gestion des messages
  public selectPlayer(player: string) {
    if (player !== this.currentUser){
      this.selectedPlayer = player;
      this.resetUnreadCount(player);
      this.loadConversation(player);
    }
  }

  /**
   * Charge la conversation avec le joueur sélectionné.
   * @param player Le joueur dont la conversation doit être chargée.
   */
  private loadConversation(player: string) {
    if (this.chatService.isConversationLoaded(player)) {
      this.updateMessages();
    }
    else{
      const token = this.userCookieService.getToken();
      if(token){
        this.loadHistoricalMessages(token, player);
      }
    }
  }

  /**
   * Charge les messages historiques pour une conversation donnée.
   * @param token Le token d'authentification de l'utilisateur.
   * @param player Le joueur dont les messages doivent être chargés.
   */
  private loadHistoricalMessages(token: string, player: string) {
    const currentMessages = this.chatService.getMessages();
    
    this.messageDAO.GetConversation(token, player).subscribe({
      next: (response) => this.handleHistoricalMessages(response, currentMessages, player),
      error: (error) => this.showErrorPopup('Erreur lors du chargement des messages: ' + error)
    });
  }

  /**
   * Gère les messages historiques reçus et les ajoute à la conversation.
   * @param response Les messages historiques reçus.
   * @param currentMessages Les messages actuellement chargés.
   * @param player Le joueur dont la conversation est mise à jour.
   */
  private handleHistoricalMessages(response: { Messages: MessageDTO[] }, currentMessages: MessageDTO[], player: string) {
    if (response?.Messages){
      response.Messages.forEach((msg: MessageDTO) => {
          if (!this.messageExists(msg, currentMessages)) {
              this.chatService.addMessage(msg);
          }
      });
  
      this.chatService.markConversationAsLoaded(player);
      this.updateMessages();
    }
  }

  /**
   * Vérifie si un message existe déjà dans la liste des messages actuels.
   * @param msg Le message à vérifier.
   * @param currentMessages La liste des messages actuels.
   * @returns Vrai si le message existe, sinon faux.
   */
  private messageExists(msg: MessageDTO, currentMessages: MessageDTO[]): boolean {
    return currentMessages.some(currentMsg => currentMsg.Id() === msg.Id());
  }

  // Actions utilisateur
  public sendMessage() {
    if (this.newMessage.trim() && this.selectedPlayer){
      const message = new MessageDTO(this.currentUser, this.selectedPlayer, this.newMessage);
      this.websocketService.sendMessage(this.newMessage, this.selectedPlayer);
      this.chatService.addMessage(message);
      this.newMessage = '';
      this.scrollToBottomWithDelay();
    }
  }

  /**
   * Ferme la conversation actuelle.
   */
  public closeChat() {
    this.selectedPlayer = null;
  }

  /**
   * Bascule l'état d'ouverture du panneau de messagerie.
   */
  public toggleChat() {
    this.isChatOpen = !this.isChatOpen;
  }

  // Gestion des messages non lus
  public getUnreadMessageCount(player: string): number {
    return this.unreadMessageCounts.get(player) || 0;
  }

  /**
   * Récupère le nombre total de messages non lus.
   * @returns le nombre total de messages non lus.
   */
  public getTotalUnreadMessages(): number {
    return Array.from(this.unreadMessageCounts.values()).reduce((a, b) => a + b, 0);
  }

  private resetUnreadCount(player: string) {
    this.unreadMessageCounts.set(player, 0);
  }

  // Mise à jour de l'affichage
  private updateMessages() {
    if (!this.selectedPlayer) return;

    this.messages = this.filterAndSortMessages();
    this.scrollToBottomWithDelay();
  }

  /**
   * Filtre et trie les messages pour la conversation actuelle.
   * @returns La liste des messages filtrés et triés.
   */
  private filterAndSortMessages(): MessageDTO[] {
    const filteredMessages = this.chatService.getMessages().filter(msg => 
      (msg.Sender() === this.currentUser && msg.Receiver() === this.selectedPlayer) ||
      (msg.Sender() === this.selectedPlayer && msg.Receiver() === this.currentUser)
    );

    return filteredMessages.sort((a, b) => a.Timestamp().getTime() - b.Timestamp().getTime());
  }

  /**
   * Fait défiler le conteneur de messages vers le bas après un délai.
   */
  private scrollToBottomWithDelay() {
    setTimeout(() => this.scrollToBottom(), 10);
  }

  /**
   * Fait défiler le conteneur de messages vers le bas.
   */
  private scrollToBottom() {
    const container = this.messageContainer?.nativeElement;
    if (container) {
      container.scrollTop = container.scrollHeight;
    }
  }

  // Gestion des mises à jour
  public update(observable: Observable): void {
    if (observable instanceof ConnectedUsersService) {
      this.handleConnectedUsersUpdate();
    } else if (observable instanceof ChatService) {
      this.handleChatUpdate();
    }
  }

  private handleConnectedUsersUpdate() {
    this.initializeConnectedPlayers();
  }

  private handleChatUpdate() {
    const lastMessage = this.getLastMessage();
    if (lastMessage && this.isNewMessageForCurrentUser(lastMessage)) {
        this.updateUnreadCount(lastMessage);
    }
    this.updateMessages();
  }

  private getLastMessage(): MessageDTO | undefined {
    const messages = this.chatService.getMessages();
    return messages[messages.length - 1];
  }

  private isNewMessageForCurrentUser(message?: MessageDTO): boolean {
    return message?.Receiver() === this.currentUser;
  }

  private updateUnreadCount(message: MessageDTO) {
    if (!this.isChatOpen || this.selectedPlayer !== message.Sender()) {
      const currentCount = this.unreadMessageCounts.get(message.Sender()) || 0;
      this.unreadMessageCounts.set(message.Sender(), currentCount + 1);
    }
  }

  /**
   * Affiche un popup d'erreur avec un message donné.
   * @param message message d'ereur à afficher
   */
  private showErrorPopup(message: string) {
    Swal.fire({
      icon: 'error',
      title: 'Erreur',
      text: message,
    });
  }
}
