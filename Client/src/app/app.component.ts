import { AfterViewInit, Component, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridComponent } from './grid/grid.component';
import { NavbarComponent } from './navbar/navbar.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements AfterViewInit{
  title = 'Client';
  private state: string;

  public constructor() {
    this.state = 'light';
  }


  public ngAfterViewInit(){
    document.getElementById('state-button')!.addEventListener('click', () => {
      this.changeLightState();
    });
    this.connectWebSocket();
  }


  public changeLightState(): void {
    if (this.state == 'light') {
      document.body.style.background = 'black';
      document.body.style.color = 'white';
      document
        .getElementById('navbar-container')!
        .style.setProperty('background', 'grey', 'important');
      document
        .getElementById('navbar-container')!
        .style.setProperty('color', 'white', 'important');
      Array.from(document.getElementsByClassName('timer')).forEach((timer) => {
        (timer as HTMLDivElement).style.border = '1px solid white';
      });
      (document.getElementById('logo') as HTMLImageElement).src =
        'renard_dark.png';
      (<HTMLButtonElement>document.getElementById('state')!).textContent =
        'Interface claire';
      this.state = 'dark';
    } else {
      document.body.style.background = 'white';
      document.body.style.color = 'black';
      document
        .getElementById('navbar-container')!
        .style.setProperty('background', '#faf9fd', 'important');
      document
        .getElementById('navbar-container')!
        .style.setProperty('color', 'black', 'important');
      Array.from(document.getElementsByClassName('timer')).forEach((timer) => {
        (timer as HTMLDivElement).style.border = '1px solid black';
      });
      (<HTMLButtonElement>document.getElementById('state')!).textContent =
        'Interface sombre';
      (document.getElementById('logo') as HTMLImageElement).src = 'renard.png';
      this.state = 'light';
    }
  }

  public connectWebSocket(): void {
    var client = new WebSocket("ws://127.0.0.1:7000/"); //10.211.55.3:7000
    client.onopen = ()=>{
      console.log('Connected to WebSocket server');
      client.send('Hello from the browser client!');
    }

  }
}
