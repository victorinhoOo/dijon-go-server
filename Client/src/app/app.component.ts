import { AfterViewInit, Component, OnInit, OnDestroy,ViewChild, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridComponent } from './IHM/Components/grid/grid.component';
import { RegisterComponent } from './IHM/Components/register/register.component';  
import { UploadImageComponent } from './IHM/Components/upload-image/upload-image.component';
import { NavbarComponent } from './IHM/Components/navbar/navbar.component';
import { ConnexionComponent } from './IHM/Components/connexion/connexion.component';
import { ProfileComponent }  from './IHM/Components/profile/profile.component'
import { IndexComponent } from './IHM/Components/index/index.component';
import { FooterComponent } from "./IHM/Components/footer/footer.component";
import { CommonModule } from '@angular/common';
import { HamburgerBtnComponent } from './IHM/Components/hamburger-btn/hamburger-btn.component';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridComponent, RegisterComponent, UploadImageComponent, NavbarComponent, ProfileComponent, ConnexionComponent, IndexComponent, FooterComponent, CommonModule,HamburgerBtnComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'

})
export class AppComponent implements AfterViewInit{

  @ViewChild(NavbarComponent) navbarComponent!: NavbarComponent;

  // Ajout d'une propriété pour gérer la visibilité de la navbar
  private _isNavbarVisible: boolean = false;
  private _isButtonClicked: boolean = false;
  private isBlack: boolean;
  // Getter pour obtenir la visibilité de la navbar
  public get isNavbarVisible(): boolean {
    return this._isNavbarVisible;
  }
  
  // Getter pour obtenir la visibilité du menu hamburger
  public get isButtonClicked(): boolean {
    return this._isButtonClicked;
  }
  
  /**
   * Construit l'application en initialisant la valeur du mode sombre
   */
  public constructor(private cdr: ChangeDetectorRef) {
    this.checkScreenSize();
    this.isBlack = false;
  }

  /**
   * Initialisation des écouteurs d'événements après le chargement de la page
   */
  public ngAfterViewInit()
  {   
   this.isBlack = false;
  }

  /**
   * Réagit au clic sur le bouton de changement de mode (sombre ou clair)
   */
  public changeLightState():void{
    if(this.isBlack){
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
    }
    else{
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
    }
  }
  
  /**
   * Méthode pour basculer la visibilité de la navbar
   */
  public toggleNavbar(): void {
    
    
    this._isNavbarVisible = !this._isNavbarVisible;
    this._isButtonClicked = !this._isNavbarVisible;
    if(this.isBlack){
      var textcolor = "#FFFFFF"
      document.getElementById("navbar-container")!.style.backgroundColor = textcolor;
    }
  }

   /**
    * Méthode pour fermer la navbar
    */
   public onCloseNavbar(): void {
    this._isNavbarVisible = false;
    this._isButtonClicked = true;
    this.changeLightState();
  }
  
  /**
   *  Méthode pour vérifier la taille de l'écran et ajuster la navbar
   */ 
  private checkScreenSize(): void {
    const screenWidth = window.innerWidth;
    this._isNavbarVisible = screenWidth >= 1025;
  }

  /**
   * Écouter les changements de taille de l'écran
   * @param event evenement de changement de taille de l'écran
   */
  @HostListener('window:resize', ['$event'])
  onResize(event: any): void {
    this.checkScreenSize();
  }

  /**
   * Méthode appelée lorsque l'événement de changement de thème est déclenché.
   * Elle met à jour l'état `isBlack` en fonction de la valeur de l'événement,
   * et applique les changements de style correspondants en appelant `changeLightState`.
   *
   * @param $event - Un booléen indiquant si le thème sombre (true) ou clair (false) est activé.
   */
  public onThemeChangeEvent($event: boolean){
      this.isBlack = $event;
      this.changeLightState();
      
   }  
}

