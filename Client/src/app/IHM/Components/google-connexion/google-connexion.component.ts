import { Component, OnInit } from '@angular/core';
import { CredentialResponse } from 'google-one-tap';
import { UserDAO } from '../../../Model/DAO/UserDAO';
import { HttpClient } from '@angular/common/http';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { User } from '../../../Model/User';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-google-connexion',
  standalone: true,
  imports: [],
  templateUrl: './google-connexion.component.html',
  styleUrls: ['./google-connexion.component.css']
})
/**
 * Composant de connexion Google
 */
export class GoogleConnexionComponent implements OnInit {

  private userDAO: UserDAO;
  private router: Router

  constructor(httpClient: HttpClient, private userCookieService: UserCookieService) { 
    this.userDAO = new UserDAO(httpClient);
    this.router = new Router();
  }

  ngOnInit(): void {
    this.initializeGoogleOneTap();
  }

  /**
   * Initialise Google One Tap et configure le callback pour gérer les informations de connexion.
   */
  private initializeGoogleOneTap(): void {
    const clientId = '995926287687-u9810k8cnmk5b5ifaeh2fmtb4o34kinh.apps.googleusercontent.com';

    (window as any).google.accounts.id.initialize({
      client_id: clientId,
      callback: (response: CredentialResponse) => this.handleCredentialResponse(response),
      auto_select: false,
      cancel_on_tap_outside: false
    });

    (window as any).google.accounts.id.renderButton(
      document.getElementById("buttonDiv"),
      { theme: "outline", size: "large", shape: "rectangular", width: "100%" }  
    );

    const buttonDiv = document.getElementById("buttonDiv");
    if (buttonDiv) {
      buttonDiv.style.marginTop = "20px";
      buttonDiv.style.width = "100%";
      buttonDiv.style.textAlign = "center";
      buttonDiv.style.display = "flex";
      buttonDiv.style.justifyContent = "center";
    }

    (window as any).google.accounts.id.prompt(); // Affiche le bouton de connexion Google 
  }

  /**
   * Gère la réponse renvoyé par Google après une connexion réussie.
   */
  private handleCredentialResponse(response: CredentialResponse): void {
    const idToken = response.credential;
    // Essaye de se connecter
    this.userDAO.GoogleLogin(idToken).subscribe({
      next: (response: {token: string}) => 
        {
          this.userCookieService.setToken(response.token); // Stocke le token dans les cookies
          
          //recuperation des infos l'utilisateur à partir de son token
          this.userDAO.GetUser(response.token).subscribe({
            next: (user: User) => 
            {
              this.userCookieService.setUser(user);
              // Redirige vers la page d'accueil
              this.router.navigate(['/index']);
            },
          });

        }
    });
  }
}
