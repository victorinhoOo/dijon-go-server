import { Component } from '@angular/core';
import { GridComponent } from '../grid/grid.component';

@Component({
  selector: 'app-game-screen',
  standalone: true,
  imports: [GridComponent],
  templateUrl: './game-screen.component.html',
  styleUrl: './game-screen.component.css'
})
export class GameScreenComponent {

  public constructor(){
    alert("ok")
    const ws = new WebSocket("wss://127.0.0.1:7000");

    ws.onopen = ()=>{
      console.log("connected");
    }

    ws.onerror = (event) => {
      console.error("WebSocket error observed:", event);
  };
  }
}
