import { Component, OnInit } from '@angular/core';
import  {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ProfileSettingsComponent } from '../profile-settings/profile-settings.component';
import { MatDialog } from '@angular/material/dialog';
import { UserCookieService } from '../Model/UserCookieService';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { User } from '../Model/User';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [MatIconModule,MatButtonModule, HttpClientModule,CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})

/**
 * Composant de la page de profil
 */
export class ProfileComponent {
  private token: string;
  private userPseudo: string;
  private userEmail: string;
  private rank: string;
  private avatar: string;
  private historyData: Array<{
    black:string;
    white:string;
    gameId: number;
    moveNumber: number;
    boardState: string;
    capturedBlack: number;
    capturedWhite: number;
    rule:string;
    size:string;
    player1: string;
    player2: string;
    winner: string;
    date: string;
  }> = [];

  // Getter pour accéder aux données
  public getHistoryData(): Array<any> {
    return this.historyData;
  }

  /**
   * Renvoi l'avatar de l'utilisateur
   */
  public get Avatar(): string {
    return this.avatar;
  }

  /**
   * Renvoi le pseudo de l'utilisateur
   */
  public get UserPseudo(): string {
    return this.userPseudo;
  }

  /**
   * Renvoi le mail de l'utilisateur
   */
  public get UserEmail(): string {
    return this.userEmail;
  }

  /**
   * Renvoi le rang de l'utilisateur
   */
  public get Rank(): string {
    return this.rank;
  }

    /**
   * Le constructeur initialise le composant en récupérant le jeton utilisateur, 
   * en vérifiant son authenticité, et en récupérant les informations de l'utilisateur 
   * à partir des cookies. Si le jeton n'est pas valide, l'utilisateur est redirigé vers 
   * la page de connexion.
   * */
  constructor(public dialog: MatDialog, private userCookieService: UserCookieService, private router: Router, private http: HttpClient) {
    // Récupère le token utilisateur
    this.token = this.userCookieService.getToken();
    //verfication du token utilisateur sinon redirection login
    if(!this.token)
    {
        this.router.navigate(['/login']);
    }
    // Récupère les informations de l'utilisateur pour l'affichage
    this.userPseudo = this.userCookieService.getUser()!.Username;
    this.userEmail = this.userCookieService.getUser()!.Email;
    this.rank = this.userCookieService.getUser()!.Rank;
    this.avatar = 'https://localhost:7065/profile-pics/' + this.userPseudo;        
  }


  ngOnInit(): void {
    // Appel de la méthode pour charger l'historique au moment de l'initialisation
    this.getHistory();
  }

  /**
   * Ouvre une popup profile settings pour modifier son profile 
   */
  public openDialog(): void {
    const dialogRef = this.dialog.open(ProfileSettingsComponent, {
      width: '80%',
      height: '85%',
      panelClass: 'custom-dialog-container'
    });
    dialogRef.afterClosed().subscribe(result => {
      // Récupère les informations de l'utilisateur après la modification
      this.userPseudo = this.userCookieService.getUser()!.Username;
      this.userEmail = this.userCookieService.getUser()!.Email;
      this.avatar = `https://localhost:7065/profile-pics/${this.userPseudo}?t=${new Date().getTime()}`; // cache-busting pour mettre à jour l'avatar
      this.rank = this.userCookieService.getUser()!.Rank;
    });
  }

  /**
   * permet d'afficher l'historique des parties de l'utilisateurs 
   */
  public getHistory(): void {
    const history: any[] = ProfileComponent.getFAKE_DAO_History();

    // Préparation des données pour affichage
    this.historyData = history.map(entry => ({
      gameId: entry.gamestate.game_id,
      moveNumber: entry.gamestate.move_number,
      boardState: entry.gamestate.board_state,
      capturedBlack: entry.gamestate.captured_black,
      capturedWhite: entry.gamestate.captured_white,
      player1: entry.savedgame.player1,
      player2: entry.savedgame.player2,
      black:entry.gamestate.black,
      white:entry.gamestate.white,
      size: entry.savedgame.size,
      rule:entry.savedgame.rule,
      winner: entry.savedgame.winner,
      date: new Date(entry.savedgame.date).toLocaleString(), // Format de date
    }));
  }

  // Simule un DAO avec des données en dur
  public static getFAKE_DAO_History(): Array<any> {
    return [
      {
        gamestate: {
          id: 1,
          game_id: 1,
          move_number: 42,
          black : 'clem',
          white :'test',
          board_state: "xxoooxxooxo",
          captured_black: 3,
          captured_white: 2,
        },
        savedgame: {
          id: 1,
          player1: "test",
          player2: "clem",
          size: '19x19',
          rule: "japonaise",
          winner: "clem",
          date: "2024-11-27T10:30:00",
        },
      },
      {
        gamestate: {
          id: 2,
          game_id: 2,
          black : 'clem',
          white :'test',
          move_number: 25,
          board_state: "ooxoxoxox",
          captured_black: 1,
          captured_white: 3,
        },
        savedgame: {
          id: 2,
          player1: "test",
          player2: "clem",
          size: '9x9',
          rule: "chinoise",
          winner: "test",
          date: "2024-11-25T14:00:00",
        },
      },
    ];
  }
}



