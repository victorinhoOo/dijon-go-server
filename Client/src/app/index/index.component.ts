import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { MatIcon } from '@angular/material/icon';
import { AuthService } from '../Model/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index',
  standalone: true,
  imports: [NavbarComponent, MatIcon],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
/**
 * Composant de la page d'accueil (connecté)
 */
export class IndexComponent implements OnInit {

   private token: string;
   private userPseudo: string;
   private avatar: string; 
   private userRank: string;

   /**
    * Getter pour le lien d'affichage de l'avatar
    */
  public get Avatar(): string {
    return this.avatar;
  }
  /**
   * Getter pour le pseudo de l'utilisateur
   */
  public get UserPseudo(): string {
    return this.userPseudo;
  }

  public get UserRank(): string {
    return this.userRank;
  }

  /**
   * Initialisation du composant
  */
  constructor(private authService: AuthService, private router: Router) {
    this.avatar = 'https://localhost:7065/profile-pics/';
    this.token = '';
    this.userPseudo = '';
    this.userRank = '9 dan';
  }
  
  // Méthode pour remplir le leaderboard avec des données fictives (todo:  remplacer par des données réelles)
  private populateLeaderboard(): void {
    const leaderboard = document.querySelector('.leaderboard');
    // temporaire
    const fakeEntries = [
      '1) Victor - 9 dan',
      '2) Mathis - 7 dan',
      '3) Clément -  2 dan',
      '4) Louis - 1 kyu',
      '5) Adam - 20 kyu'
    ];
    leaderboard!.innerHTML = '';

    // Ajoute des entrées dans la div "leaderboard"
    fakeEntries.forEach(entry => {
      const p = document.createElement('p');
      p.textContent = entry;
      leaderboard!.appendChild(p);
    });
  }

  /**
   * Initialisation du composant
   */
  public ngOnInit() {
    // Récupère le token utilisateur
    this.token = this.authService.getToken();
    //verfication du token utilisateur sinon redirection login
    if(!this.token)
    {
        this.router.navigate(['/login']);
    }
    //recuperation du pseudo de l'utilisateur
    this.userPseudo = this.authService.getUser().Username;

    //recuperation de l'image de l'utilisateur à partir de son pseudo
    this.avatar += this.userPseudo; 
    
    this.populateLeaderboard();
  }
}
