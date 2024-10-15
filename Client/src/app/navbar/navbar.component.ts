import { AfterViewInit, ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { UserCookieService } from '../Model/UserCookieService';
import { NgIf } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule, NgIf],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit, OnDestroy {
  private lightState: string;
  private tokenUser: string;
  private tokenSubscription!: Subscription;

  public get TokenUser(): string {
    return this.tokenUser;
  }

  public constructor(private router: Router, private userCookieService: UserCookieService, private cdr: ChangeDetectorRef) {
    this.lightState = 'light';
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
}
