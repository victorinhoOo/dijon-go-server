import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  private websocket: WebSocket;
  private idGame: string;

  constructor() 
  {
    this.websocket = new WebSocket("ws://10.211.55.3:7000/");
    this.idGame=""
  }


  public connectWebsocket(){
    this.websocket.onopen = ()=>{
      this.websocket.send("0/Create:")
    }

    this.websocket.onmessage = (message)=>{
      if(message.data.length <= 3){
        this.idGame = message.data;
      }
      else if(message.data.includes("x,y,color")){
        this.updateBoard(message.data);
      }
      console.log(message.data);
    }

    this.websocket.onclose = ()=>{
      console.log("disconnected");
    }

  }

  public placeStone(coordinates:string){
    if(this.websocket.OPEN){
      this.websocket.send(`${this.idGame}Stone:${coordinates}`)
      console.log(`${this.idGame}Stone:${coordinates}`);
    }
  }

  public updateBoard(data:string){
    let lines = data.split("\r\n");
    for(let i=1; i<lines.length; i++){
      let stoneData = lines[i].split(",");
      let x = stoneData[0];
      let y = stoneData[1];
      let color = stoneData[2];
      let stone = document.getElementById(`${x}-${y}`);
      switch(color){
        case "White": stone!.style.background = "white";break;
        case "Black": stone!.style.background = "black";break;
        case "Empty": stone!.style.background = "transparent";break;
      }
    }
  }
}
