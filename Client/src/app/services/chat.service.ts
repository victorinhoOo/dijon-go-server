import { Injectable } from '@angular/core';
import { Observable } from '../Observer/Observable';
import { UserCookieService } from '../Model/UserCookieService';

interface ChatMessage {
  sender: string;
  receiver: string;
  content: string;
  timestamp: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService extends Observable {
  private messages: ChatMessage[] = [];
  private userCookieService: UserCookieService;

  constructor(userCookieService: UserCookieService) {
    super();
    this.userCookieService = userCookieService;
  }

  public addMessage(sender: string, receiver: string, content: string) {
    this.messages.push({
      sender,
      receiver,
      content,
      timestamp: new Date()
    });
    this.notifyChange(this);
  }

  public getMessages(): ChatMessage[] {
    return this.messages;
  }
} 