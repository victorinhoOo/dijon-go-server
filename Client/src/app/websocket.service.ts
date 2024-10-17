import { Injectable } from '@angular/core';
import { UserCookieService } from './Model/UserCookieService';
import { resolve } from 'path';
import { User } from './Model/User';

@Injectable({
  providedIn: 'root',
})
export class WebsocketService {
  private websocket: WebSocket | null;
  private idGame: string;
  private color: string;

  constructor(private userCookieService: UserCookieService) {
    this.websocket = null;
    this.idGame = '';
    this.color = "";
  }

  public getWs() {
    return this.websocket;
  }

  public connectWebsocket():Promise<void>{
    return new Promise((resolve, reject) => {
      this.websocket = new WebSocket('ws:///10.211.55.3:7000/'); //10.211.55.3
      this.websocket.onopen = () => {
        console.log("connected");
        resolve();
      };

      this.websocket.onmessage = (message) => {
        if (message.data.length <= 3) {
          this.idGame = message.data;
        } else if (message.data.includes('x,y,color')) {
          this.updateBoard(message.data.split("|")[0]);
          this.updateScore(message.data.split("|")[1]);
        }
        else if(message.data.includes("Start")){
          let pseudo = document.getElementById("pseudo-text");
          pseudo!.innerHTML = message.data.split(':')[1];
          let profilePic = document.getElementById("opponent-pic") as HTMLImageElement;
          profilePic!.src = `https://localhost:7065/profile-pics/${pseudo!.innerText}`;
        }
        console.log(message.data);
      };

      this.websocket.onclose = () => {
        console.log('disconnected');
      };
    });
  }

  public disconnectWebsocket(){
    this.websocket?.close(1000);
  }

  public createGame(): void {
    if (this.websocket != null && this.websocket.OPEN) {
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`0/Create:${userToken}`);
      this.color = "black";
    } else {
      console.log('not connected');
    }
  }

  public joinGame(id: number): void {
    if (this.websocket != null && this.websocket.OPEN) {
      let userToken = this.userCookieService.getToken();
      this.websocket.send(`${id}/Join:${userToken}`);
      this.color = "white";
    } else {
      console.log('not connected');
    }
  }

  public skipTurn():void{
    if (this.websocket != null && this.websocket.OPEN) {
      this.websocket.send(`${this.idGame}Skip:`);
    }else{
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


  public updateScore(score:string):void{
    let playerScore;
    let opponentScore;
    if(this.color == "black"){
      playerScore = score.split(";")[0];
      opponentScore = score.split(";")[1];
    }
    else{
      playerScore = score.split(";")[1];
      opponentScore = score.split(";")[0];
    }

    document.getElementById("opponent-score-value")!.innerHTML = opponentScore;
    document.getElementById("player-score-value")!.innerHTML = playerScore;
  }
}
