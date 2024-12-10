import { Injectable } from '@angular/core';
import { Observable } from '../Observer/Observable';
import { UserCookieService } from '../Model/UserCookieService';
import { MessageDTO } from '../Model/DTO/MessageDTO';

@Injectable({
  providedIn: 'root'
})
export class ChatService extends Observable {
  private messages: MessageDTO[] = [];
  private loadedConversations: Set<string> = new Set();
  private userCookieService: UserCookieService;

  constructor(userCookieService: UserCookieService) {
    super();
    this.userCookieService = userCookieService;
  }

  public addMessage(message: MessageDTO) {
    const isDuplicate = this.messages.some(msg => 
      msg.Sender() === message.Sender() &&
      msg.Receiver() === message.Receiver() &&
      msg.Content() === message.Content() &&
      msg.Timestamp().getTime() === message.Timestamp().getTime()
    );

    if (!isDuplicate) {
      this.messages.push(message);
      console.log("chat service", message);
      console.log("chat service", this.messages);
      this.notifyChange(this);
    }
  }

  public getMessages(): MessageDTO[] {
    return this.messages;
  }

  public isConversationLoaded(username: string): boolean {
    return this.loadedConversations.has(username);
  }

  public markConversationAsLoaded(username: string): void {
    this.loadedConversations.add(username);
  }
} 