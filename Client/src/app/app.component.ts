import { AfterViewInit, Component, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridComponent } from './grid/grid.component';
import { RegisterComponent } from './register/register.component';  
import { UploadImageComponent } from './upload-image/upload-image.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ConnexionComponent } from './connexion/connexion.component';
import { ProfileComponent }  from './profile/profile.component'
import { IndexComponent } from './index/index.component';
import { FooterComponent } from "./footer/footer.component";
import { WebsocketService } from './websocket.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridComponent, RegisterComponent, UploadImageComponent, NavbarComponent, ProfileComponent, ConnexionComponent, IndexComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'

})
export class AppComponent implements AfterViewInit{
  title = 'Client';
  private state: string;

  public constructor(private websocketService:WebsocketService) {
    this.state = 'light';
  }


  public ngAfterViewInit(){
    document.getElementById('state-button')!.addEventListener('click', () => {
      this.changeLightState();
    });
    this.connectWebSocket();
  }


  public changeLightState():void{
    if(this.state == "light"){
      // Mode sombre
      document.body.style.background = "#302E2B";
      document.body.style.color = "white";
      document.getElementById("navbar-container")!.style.setProperty("background", "grey", "important");
      document.getElementById("navbar-container")!.style.setProperty("color", "white", "important");
      Array.from(document.getElementsByClassName("timer")).forEach(timer=>{
        (timer as HTMLDivElement).style.border = "1px solid white";
      });
      (document.getElementById("logo") as HTMLImageElement).src = "renard_dark.png";
      //(document.getElementById("renardRegister") as HTMLImageElement).src = "renard_dark.png";

      // Modification des couleurs des liens du footer
      const footerLinks = document.querySelectorAll("footer a");
      footerLinks.forEach(link => {
          (link as HTMLAnchorElement).style.color = "#989795";
          (link as HTMLAnchorElement).addEventListener("mouseover", () => {
              (link as HTMLAnchorElement).style.color = "white";
          });
          (link as HTMLAnchorElement).addEventListener("mouseout", () => {
              (link as HTMLAnchorElement).style.color = "#989795";
          });

      });
      (<HTMLButtonElement>document.getElementById("state")!).textContent = "Interface claire";
      this.state = "dark";
    }else{
      // Mode Clair
      document.body.style.background = "#f5f5f5";
      document.body.style.color = "black";
      document.getElementById("navbar-container")!.style.setProperty("background", "#faf9fd", "important");
      document.getElementById("navbar-container")!.style.setProperty("color", "black", "important");
      Array.from(document.getElementsByClassName("timer")).forEach(timer=>{
        (timer as HTMLDivElement).style.border = "1px solid black";
      });
      (<HTMLButtonElement>document.getElementById("state")!).textContent = "Interface sombre";
      (document.getElementById("logo") as HTMLImageElement).src = "renard.png";

      // Modification des couleurs des liens du footer
      const footerLinks = document.querySelectorAll("footer a");
        footerLinks.forEach(link => {
            (link as HTMLAnchorElement).style.color = "darkgray";
            (link as HTMLAnchorElement).addEventListener("mouseover", () => {
                (link as HTMLAnchorElement).style.color = "black";
            });
            (link as HTMLAnchorElement).addEventListener("mouseout", () => {
                (link as HTMLAnchorElement).style.color = "darkgray";
            });
        });
      //(document.getElementById("renardRegister") as HTMLImageElement).src = "renard.png";
      this.state = "light"
    }
    
  }

  public connectWebSocket(): void {
    this.websocketService.connectWebsocket();

  }
  
}
