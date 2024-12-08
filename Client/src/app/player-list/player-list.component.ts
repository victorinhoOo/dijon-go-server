import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WebsocketService } from '../websocket.service';
import { ConnectedUsersService } from '../services/connected-users.service';
import { ChatService } from '../services/chat.service';
import { IObserver } from '../Observer/IObserver';
import { Observable } from '../Observer/Observable';
import { UserCookieService } from '../Model/UserCookieService';
import { MatIconModule } from '@angular/material/icon';

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

  constructor(
    private connectedUsersService: ConnectedUsersService,
    private chatService: ChatService,
    private websocketService: WebsocketService,
    private userCookieService: UserCookieService
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
      this.updateMessages();
    }
  }

  sendMessage() {
    if (this.newMessage.trim() && this.selectedPlayer) {
      this.websocketService.sendMessage(this.newMessage, this.selectedPlayer);
      this.chatService.addMessage(this.currentUser, this.selectedPlayer, this.newMessage);
      this.newMessage = '';
    }
  }

  private updateMessages() {
    if (this.selectedPlayer) {
      this.messages = this.chatService.getMessages().filter(msg => 
        (msg.sender === this.currentUser && msg.receiver === this.selectedPlayer) ||
        (msg.sender === this.selectedPlayer && msg.receiver === this.currentUser)
      );
    }
  }

  update(observable: Observable): void {
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
      
      if (lastMessage && lastMessage.receiver === this.currentUser) {
        if (this.selectedPlayer !== lastMessage.sender) {
          const currentCount = this.unreadMessageCounts.get(lastMessage.sender) || 0;
          this.unreadMessageCounts.set(lastMessage.sender, currentCount + 1);
        }
      }
      
      this.updateMessages();
    }
  }

  closeChat() {
    this.selectedPlayer = null;
  }

  getUnreadMessageCount(player: string): number {
    return this.unreadMessageCounts.get(player) || 0;
  }
}
