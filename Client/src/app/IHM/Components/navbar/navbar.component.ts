import { AfterViewInit, ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { NgIf } from '@angular/common';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Output,EventEmitter, } from '@angular/core';
import { WebsocketService } from '../../../Model/services/websocket.service';
import { GameDAO } from '../../../Model/DAO/GameDAO';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule, NgIf,CommonModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
/**
 * Composant de la barre latérale
 */
export class NavbarComponent implements OnInit, OnDestroy {

   // Émettre un événement pour notifier la fermeture
   @Output() closeNavbar = new EventEmitter<void>();
   @Output() changeColor: EventEmitter<boolean> = new EventEmitter<boolean>();
   
  // Attribut privé pour la visibilité de la navbar
  private isNavbarVisible: boolean = true;
  private tokenUser: string;
  private tokenSubscription!: Subscription;
  private lightIsBlack: boolean
  private websocketService: WebsocketService;

  /**
   * Renvoi le token de l'utilisateur connecté
   */
  public get TokenUser(): string {
    return this.tokenUser;
  }

  public constructor(private router: Router, private userCookieService: UserCookieService, private cdr: ChangeDetectorRef, websocketService: WebsocketService, private gameDAO: GameDAO) {
    this.websocketService = websocketService;
    this.lightIsBlack = false;
    this.isNavbarVisible = true;
    this.tokenUser = '';
  }

  /**
   * Initialisation du composant, récupère le token de l'utilisateur et s'abonne à ses changements
   */
  public ngOnInit(): void {
    // Abonnement à l'Observable qui émet les changements du token
    this.tokenSubscription = this.userCookieService.getTokenObservable().subscribe(token => {
      this.tokenUser = token;
      this.cdr.detectChanges(); // Force la détection des changements
      this.setupEventListeners(); // Configurer les gestionnaires d'événements
    });
  }

  /**
   * Initialise les écouteurs d'événements sur les différents boutons de la navbar
   */
  private setupEventListeners(): void {
    const profileButton = document.getElementById("profile-button");
    const playButton = document.getElementById("play-button");
    const logoutButton = document.getElementById("logout-button");
    const loginButton = document.getElementById("login-button");
    const registerButton = document.getElementById("register-button");
    const stateButton = document.getElementById("state-button");

    if (profileButton) {
      profileButton.addEventListener("click", () => {
        this.router.navigate(["profile"]);
        if (!this.isPC())
          this.close();
      });
    }

    if (playButton) {
      playButton.addEventListener("click", () => {
        this.router.navigate(["index"]);
        if (!this.isPC())
          this.close();
      });
    }

    if (logoutButton) {
      logoutButton.addEventListener("click", () => {
        this.userCookieService.deleteToken();
        this.userCookieService.deleteUser();
        this.websocketService.disconnectWebsocket();
        this.router.navigate([""]);  // Redirection vers la page d'accueil
        this.cdr.detectChanges();    // Force la mise à jour du composant
        if (!this.isPC())
          this.close();
      });
    }

    if(loginButton){
      loginButton.addEventListener("click", () => {
        this.router.navigate(["login"]);
        if (!this.isPC())
          this.close();
      });
    }

    if(registerButton){
      registerButton.addEventListener("click", () => {
        this.router.navigate(["register"]);
        if (!this.isPC())
          this.close();
      });
    }    
    
  }

  /**
   * Rejoue la dernière partie jouée
   */
  public async replayLastGame():Promise<void>{
    let token = this.userCookieService.getToken();
    let id = await firstValueFrom(this.gameDAO.GetLastGameId(token));
    let game = await firstValueFrom(this.gameDAO.GetGameById(id));
    let size = game["size"];
    await this.router.navigate(['/replay', id, size]);
    window.location.reload();
  }

  /**
   * Detruction de la page
   */
  public ngOnDestroy(): void {
    this.tokenSubscription.unsubscribe(); // Se désabonner de l'Observable pour éviter les fuites de mémoire
  }

  /**
   * Méthode pour fermer la navbar 
   * Elle met `isNavbarVisible` à false, 
   * émet un événement pour notifier la fermeture
   */
  public close(): void {
    this.isNavbarVisible = false;
    this.closeNavbar.emit(); // Émettre l'événement pour signaler la fermeture
  }

  /**
   * Activation de la navbar pour les tablettes et mobiles
   */
  private toggleNavbar(): void {
    this.isNavbarVisible = !this.isNavbarVisible;
  }

  /**
   * Méthode pour basculer entre le thème clair et le thème sombre.
   * Elle met à jour l'état `lightState` en fonction de l'état actuel, 
   * émet un événement pour notifier le changement de couleur et affiche le nouveau thème dans la console.
   */
  public switchTheme(): void 
  {  
      this.lightIsBlack = !this.lightIsBlack;
      this.changeColor.emit(this.lightIsBlack);
  }

  /**
   * Détermine la largeur de l'écran
   * @returns true si la largeur de l'écran est supérieure ou égale à 1025 pixels, false sinon 
   */
  public isPC(): boolean {
    return window.innerWidth >= 1025; 
  }

}