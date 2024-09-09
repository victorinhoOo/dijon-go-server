import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private lightState: string;

  public constructor() {
    this.lightState = 'light';
  }

  public changeLightState() {
    let buttons = Array.from(document.getElementsByTagName('button'));

    if (this.lightState == 'light') {
      document.getElementsByTagName('body')[0].style.background = 'black';
      document.getElementById('container')!.style.background = '#373434';
      document.getElementById("container")!.style.setProperty("color", "white", "important");
      (<HTMLImageElement>document.getElementById("logo")!).src = "renard_dark.png";
      document.getElementById("state")!.textContent = "Interface claire";
      this.lightState = 'dark';
    } else {
      document.getElementsByTagName('body')[0].style.background = 'white';
      document.getElementById('container')!.style.background = '#faf9fd';
      document.getElementById("container")!.style.setProperty("color", "black", "important");
      (<HTMLImageElement>document.getElementById("logo")!).src = "renard.png";
      document.getElementById("state")!.textContent = "Interface sombre";
      this.lightState = 'light';
    }
  }
}
