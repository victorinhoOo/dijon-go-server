import { Injectable } from '@angular/core';
import { resolve } from 'path';

@Injectable({
  providedIn: 'root',
})
export class WebsocketService {
  private websocket: WebSocket | null;
  private idGame: string;

  constructor() {
    this.websocket = null;
    this.idGame = '';
  }

  public getWs() {
    return this.websocket;
  }

  public connectWebsocket():Promise<void>{
    return new Promise((resolve, reject) => {
      this.websocket = new WebSocket('ws://127.0.0.1:7000/'); //10.211.55.3
      this.websocket.onopen = () => {
        console.log("connected");
        resolve();
      };

      this.websocket.onmessage = (message) => {
        if (message.data.length <= 3) {
          this.idGame = message.data;
        } else if (message.data.includes('x,y,color')) {
          this.updateBoard(message.data);
        }
        console.log(message.data);
      };

      this.websocket.onclose = () => {
        console.log('disconnected');
      };
    });
  }

  public disconnectWebsocket(): void {
    this.websocket?.close();
  }

  public createGame(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send('0/Create:');
    } else {
      console.log('not connected');
    }
  }

  public joinGame(id: number): void {
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send(`${id}/Join:`);
    } else {
      console.log('not connected');
    }
  }

  public placeStone(coordinates: string) {
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send(`${this.idGame}Stone:${coordinates}`);
      console.log(`${this.idGame}Stone:${coordinates}`);
    } else {
      console.log('not connected');
    }
  }

  public updateBoard(data: string) {
    let lines = data.split('\r\n');
    for (let i = 1; i < lines.length; i++) {
      let stoneData = lines[i].split(',');
      let x = stoneData[0];
      let y = stoneData[1];
      let color = stoneData[2];
      let stone = document.getElementById(`${x}-${y}`);
      switch (color) {
        case 'White':
          stone!.style.background = 'white';
          break;
        case 'Black':
          stone!.style.background = 'black';
          break;
        case 'Empty':
          stone!.style.background = 'transparent';
          break;
      }
    }
  }
}
