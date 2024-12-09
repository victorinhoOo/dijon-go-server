import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WebsocketService } from '../websocket.service';
import { ConnectedUsersService } from '../services/connected-users.service';
import { ChatService } from '../services/chat.service';
import { IObserver } from '../Observer/IObserver';
import { Observable } from '../Observer/Observable';
import { UserCookieService } from '../Model/UserCookieService';
import { MatIconModule } from '@angular/material/icon';
import { MessageDAO } from '../Model/DAO/MessageDAO';
import { MessageDTO } from '../Model/DTO/MessageDTO';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent implements OnInit, IObserver {
  connectedPlayers: string[] = [];
  selectedPlayer: string | null = null;
  messages: any[] = [];
  newMessage: string = '';
  currentUser: string;
  unreadMessageCounts: Map<string, number> = new Map();
  @ViewChild('messageContainer') private messageContainer!: ElementRef;
  isChatOpen: boolean = false;

  constructor(
    private connectedUsersService: ConnectedUsersService,
    private chatService: ChatService,
    private websocketService: WebsocketService,
    private userCookieService: UserCookieService,
    private messageDAO: MessageDAO
  ) {
    this.currentUser = this.userCookieService.getUser()!.Username;
  }

  ngOnInit() {
    this.connectedUsersService.register(this);
    this.chatService.register(this);
    this.currentUser = this.userCookieService.getUser()!.Username;
    this.connectedPlayers = this.connectedUsersService.getConnectedUsers()
      .filter(player => player !== this.currentUser);
    
    this.connectedPlayers.forEach(player => {
      this.unreadMessageCounts.set(player, 0);
    });
  }

  selectPlayer(player: string) {
    if (player !== this.currentUser) {
      this.selectedPlayer = player;
      this.unreadMessageCounts.set(player, 0);
      
      if (!this.chatService.isConversationLoaded(player)) {
        const token = this.userCookieService.getToken();
        if (token) {
          this.messageDAO.GetConversation(token, player).subscribe({
            next: (response) => {
              if (response && response.Messages) {
                response.Messages.forEach(msg => {
                  this.chatService.addMessage(msg);
                });
                this.chatService.markConversationAsLoaded(player);
                this.updateMessages();
              }
            },
            error: (error) => {
              console.error('Erreur lors du chargement des messages:', error);
            }
          });
        }
      } else {
        this.updateMessages();
      }
    }
  }

  public sendMessage() {
    if (this.newMessage.trim() && this.selectedPlayer) {
      this.websocketService.sendMessage(this.newMessage, this.selectedPlayer);
      const message = new MessageDTO(this.currentUser, this.selectedPlayer, this.newMessage);
      this.chatService.addMessage(message);
      this.newMessage = '';
      setTimeout(() => this.scrollToBottom(), 100);
    }
  }

  private updateMessages() {
    if (this.selectedPlayer) {
      this.messages = this.chatService.getMessages().filter(msg => 
        (msg.Sender() === this.currentUser && msg.Receiver() === this.selectedPlayer) ||
        (msg.Sender() === this.selectedPlayer && msg.Receiver() === this.currentUser)
      );
      console.log("player list", this.messages);
      
      setTimeout(() => this.scrollToBottom(), 100);
    }
  }

  private scrollToBottom(): void {
    try {
      const container = this.messageContainer.nativeElement;
      if (container) {
        container.scrollTop = container.scrollHeight;
        console.log('Scrolling to:', container.scrollHeight);
      }
    } catch(err) {
      console.error('Error scrolling:', err);
    }
  }

  public update(observable: Observable): void {
    if (observable instanceof ConnectedUsersService) {
      this.connectedPlayers = observable.getConnectedUsers()
        .filter(player => player !== this.currentUser);
      
      this.connectedPlayers.forEach(player => {
        if (!this.unreadMessageCounts.has(player)) {
          this.unreadMessageCounts.set(player, 0);
        }
      });
    } else if (observable instanceof ChatService) {
      const messages = this.chatService.getMessages();
      const lastMessage = messages[messages.length - 1];
      
      if (lastMessage && lastMessage.Receiver() === this.currentUser) {
        if (!this.isChatOpen || this.selectedPlayer !== lastMessage.Sender()) {
          const currentCount = this.unreadMessageCounts.get(lastMessage.Sender()) || 0;
          this.unreadMessageCounts.set(lastMessage.Sender(), currentCount + 1);
        }
      }
      
      this.updateMessages();
    }
  }

  public closeChat() {
    this.selectedPlayer = null;
  }

  public toggleChat() {
    this.isChatOpen = !this.isChatOpen;
  }

  public getUnreadMessageCount(player: string): number {
    return this.unreadMessageCounts.get(player) || 0;
  }

  public getTotalUnreadMessages(): number {
    let total = 0;
    this.unreadMessageCounts.forEach(count => {
      total += count;
    });
    return total;
  }
}
