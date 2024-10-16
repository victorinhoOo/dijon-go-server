import { AfterViewInit, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridComponent } from './grid/grid.component';
import { NavbarComponent } from './navbar/navbar.component';
import { IndexComponent } from './index/index.component';
import { CommonModule } from '@angular/common';
import { HamburgerBtnComponent } from './hamburger-btn/hamburger-btn.component';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridComponent, NavbarComponent, IndexComponent, CommonModule,HamburgerBtnComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit{

  // Ajout d'une propriété pour gérer la visibilité de la navbar
  private _isNavbarVisible: boolean = false;

  // Getter pour obtenir la visibilité de la navbar
  public get isNavbarVisible(): boolean {
    return this._isNavbarVisible;
  }

  title = 'Client';
  private state: string;

  public constructor(){
    this.state = "light"
    this.checkScreenSize();
  }

  ngAfterViewInit(): void {
    document.getElementById("state-button")!.addEventListener("click",()=>{
      this.changeLightState();
    })
  }

  public changeLightState():void{
    if(this.state == "light"){
      document.body.style.background = "#302E2B";
      document.body.style.color = "white";
      document.getElementById("navbar-container")!.style.setProperty("background", "grey", "important");
      document.getElementById("navbar-container")!.style.setProperty("color", "white", "important");
      Array.from(document.getElementsByClassName("timer")).forEach(timer=>{
        (timer as HTMLDivElement).style.border = "1px solid white";
      });
      (document.getElementById("logo") as HTMLImageElement).src = "renard_dark.png";
      (<HTMLButtonElement>document.getElementById("state")!).textContent = "Interface claire";
      this.state = "dark";
    }else{
      document.body.style.background = "#e9e9e9";
      document.body.style.color = "black";
      document.getElementById("navbar-container")!.style.setProperty("background", "#faf9fd", "important");
      document.getElementById("navbar-container")!.style.setProperty("color", "black", "important");
      Array.from(document.getElementsByClassName("timer")).forEach(timer=>{
        (timer as HTMLDivElement).style.border = "1px solid black";
      });
      (<HTMLButtonElement>document.getElementById("state")!).textContent = "Interface sombre";
      (document.getElementById("logo") as HTMLImageElement).src = "renard.png"
      this.state = "light"
    }
    
  }

  // Méthode pour basculer la visibilité de la navbar
  public toggleNavbar(): void {
    this._isNavbarVisible = !this._isNavbarVisible;
  }

   // Méthode pour gérer la fermeture de la navbar
   public onCloseNavbar(): void {
    this._isNavbarVisible = false;
  }
  
  // Méthode pour vérifier la taille de l'écran et ajuster la navbar
  private checkScreenSize(): void {
    const screenWidth = window.innerWidth;
    this._isNavbarVisible = screenWidth >= 768;
    
   
  }

  // Écouter les changements de taille de l'écran
  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.checkScreenSize();
  }

}
