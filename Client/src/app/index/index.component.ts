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
export class IndexComponent implements OnInit {

   private _token: string = '';
   private _userPseudo: string = '';
   private _img: string = 'https://localhost:7065/profile-pics/'; //stock le src de l'image

     // Getter pour token
  public get token(): string 
  {
    return this._token;
  }

  // Setter pour token
  public set token(value: string) 
  {
    this._token = value;
  }

  // Getter pour userPseudo
  public get userPseudo(): string 
  {
    return this._userPseudo;
  }

  // Setter pour userPseudo
  public set userPseudo(value: string) 
  {
    this._userPseudo = value;
  }

  //Get pour le lien de l'image   
  public get img(): string{
    return this._img;
  }

  //Set le lien de l'image
  public set img(value: string)
  {
    this._img = value;
  }
  constructor(private authService: AuthService, private router: Router) {}
  
  // Méthode pour remplir le leaderboard avec des données fictives
  populateLeaderboard(): void {
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

  ngOnInit() {
    //recuperation du token, du pseudo de l'utilisateur
    this.userPseudo = this.authService.getUserPseudo();
    this.token = this.authService.getToken();

    //recuperation de l'image de l'utilisateur
    this._img += this._userPseudo; 
    
    //verfication du token utilisateur sinon redirection login
    if(!this.token)
    {
        this.router.navigate(['/login']);
    }
    this.populateLeaderboard();
  }
}
