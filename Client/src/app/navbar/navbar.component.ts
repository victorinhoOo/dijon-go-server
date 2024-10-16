import { AfterViewInit, ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { UserCookieService } from '../Model/UserCookieService';
import { NgIf } from '@angular/common';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Output,EventEmitter } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule, NgIf,CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit, OnDestroy {

   // Émettre un événement pour notifier la fermeture
   @Output() closeNavbar = new EventEmitter<void>();

  // Attribut privé pour la visibilité de la navbar
  private isNavbarVisible: boolean = true;

  // Getter pour obtenir l'état de la navbar
  public get IsNavbarVisible(): boolean {
    return this.isNavbarVisible;
  }

  // Setter pour modifier l'état de la navbar
  public set SetIsNavbarVisible(value: boolean) {
    this.isNavbarVisible = value;
  }
  
  private lightState: string;
  private tokenUser: string;
  private tokenSubscription!: Subscription;

  public get TokenUser(): string {
    return this.tokenUser;
  }

  public constructor(private router: Router, private userCookieService: UserCookieService, private cdr: ChangeDetectorRef) {
    this.lightState = 'light';
    this.isNavbarVisible = true;
    this.tokenUser = '';
  }

  public ngOnInit(): void {
    // Abonnement à l'Observable qui émet les changements du token
    this.tokenSubscription = this.userCookieService.getTokenObservable().subscribe(token => {
      this.tokenUser = token;
      this.cdr.detectChanges(); // Force la détection des changements
      this.setupEventListeners(); // Configurer les gestionnaires d'événements
    });
  }

  private setupEventListeners(): void {
    const profileButton = document.getElementById("profile-button");
    const playButton = document.getElementById("play-button");
    const logoutButton = document.getElementById("logout-button");
    const loginButton = document.getElementById("login-button");
    const registerButton = document.getElementById("register-button");

    if (profileButton) {
      profileButton.addEventListener("click", () => {
        this.router.navigate(["profile"]);
      });
    }

    if (playButton) {
      playButton.addEventListener("click", () => {
        this.router.navigate(["index"]);
      });
    }

    if (logoutButton) {
      logoutButton.addEventListener("click", () => {
        this.userCookieService.deleteToken();
        this.userCookieService.deleteUser();
        this.router.navigate([""]);  // Redirection vers la page d'accueil
        this.cdr.detectChanges();    // Force la mise à jour du composant
      });
    }

    if(loginButton){
      loginButton.addEventListener("click", () => {
        this.router.navigate(["login"]);
      });
    }

    if(registerButton){
      registerButton.addEventListener("click", () => {
        this.router.navigate(["register"]);
      });
    }
  }

  public ngOnDestroy(): void {
    this.tokenSubscription.unsubscribe(); // Se désabonner de l'Observable pour éviter les fuites de mémoire
  }

    

  close(): void {
    this.isNavbarVisible = false;
    this.closeNavbar.emit(); // Émettre l'événement pour signaler la fermeture
  }

  toggleNavbar(): void {
    this.isNavbarVisible = !this.isNavbarVisible;
  }

 
}