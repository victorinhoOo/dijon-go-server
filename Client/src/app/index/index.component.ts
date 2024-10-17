import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../Model/UserCookieService';
import { ActivatedRoute, Router } from '@angular/router';
import { PopupComponent } from '../popup/popup.component';
import { GameDAO } from '../Model/DAO/GameDAO';
import { HttpClient } from '@angular/common/http';
import { GameInfoDTO } from '../Model/DTO/GameInfoDTO';
import { WebsocketService } from '../websocket.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-index',
  standalone: true,
  imports: [NavbarComponent, MatIcon, PopupComponent],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css'],
})
/**
 * Composant de la page d'accueil (connecté)
 */
export class IndexComponent implements OnInit {
  private token: string;
  private userPseudo: string;
  private avatar: string;
  private userRank: string;
  private showPopup: boolean;
  private popupContent: SafeHtml;
  private popupTitle: string;
  private gameDAO: GameDAO;

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

  /**
   * Getter pour le rang de l'utilisateur
   */
  public get UserRank(): string {
    return this.userRank;
  }

  /**
   * Getter pour le titre du popup
   */
  public get ShowPopup(): boolean {
    return this.showPopup;
  }

  /**
   * Getter pour le titre du popup
   */
  public get PopupContent(): SafeHtml {
    return this.popupContent;
  }

  public get PopupTitle(): string {
    return this.popupTitle;
  }

  /**
   * Initialisation du composant
   */
  constructor(
    private userCookieService: UserCookieService,
    private router: Router,
    private httpClient: HttpClient,
    private websocketService: WebsocketService,
    private domSanitizer: DomSanitizer,
    private route: ActivatedRoute
  ) {
    this.avatar = 'https://localhost:7065/profile-pics/';
    this.token = '';
    this.userPseudo = '';
    this.userRank = '9 dan';
    this.showPopup = false;
    this.popupContent = '';
    this.popupTitle = '';
    this.gameDAO = new GameDAO(httpClient);
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
      '5) Adam - 20 kyu',
    ];
    leaderboard!.innerHTML = '';

    // Ajoute des entrées dans la div "leaderboard"
    fakeEntries.forEach((entry) => {
      const p = document.createElement('p');
      p.textContent = entry;
      leaderboard!.appendChild(p);
    });
  }

  /**
   * Initialisation du composant
   */
  public async ngOnInit() {

    if(this.route.snapshot.paramMap.get('id')!=null){
      let id = this.route.snapshot.paramMap.get('id');
      await this.websocketService.connectWebsocket();
      this.websocketService.joinGame(Number(id));
      this.router.navigate(["game"]);
    }
    else{
      this.websocketService.disconnectWebsocket();
    }
    // Récupère le token utilisateur
    this.token = this.userCookieService.getToken();
    //verfication du token utilisateur sinon redirection login
    if (!this.token) {
      this.router.navigate(['/login']);
    }
    //recuperation du pseudo de l'utilisateur
    this.userPseudo = this.userCookieService.getUser().Username;

    //recuperation de l'image de l'utilisateur à partir de son pseudo
    this.avatar += this.userPseudo;

    const joinGamesLink = document.getElementById('joinGames');
    if (joinGamesLink) {
      joinGamesLink.addEventListener('click', (event) => {
        this.showPopup = true;
        this.initializePopupContent();
      });
    }

    let createGameLink = document.getElementById('create-game');
    if (createGameLink) {
      createGameLink.addEventListener('click',async() => {
        await this.websocketService.connectWebsocket();
        console.log("ahaéhhh");
        this.websocketService.createGame();
        this.router.navigate(['game']);
      });
    }
    this.populateLeaderboard();
  }

  /**
   * Initialise le contenu de la popup avec les parties disponibles
   */
  private initializePopupContent() {
    this.gameDAO.GetAvailableGames().subscribe({
      next: (games: GameInfoDTO[]) => {
        let content = '';
        games.forEach((game) => {
          this.popupTitle = 'Parties disponibles';
          content += `<a href="/${game["id"]}">${game['title']} ${game['size']}x${game['size']}</a>`;
        });
        this.popupContent = this.domSanitizer.bypassSecurityTrustHtml(content);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  public handlePopupClose(): void {
    this.showPopup = false;
  }

  public openPopup() {
    this.showPopup = true;
  }

  public joinGame():void{
    
  }

  public connectWebSocket(): void {
    this.websocketService.connectWebsocket();
  }

  public deconnectWebSocket():void{
    this.websocketService.disconnectWebsocket();
  }
}
