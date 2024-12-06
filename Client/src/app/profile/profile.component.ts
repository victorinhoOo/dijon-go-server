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
import { GameDAO } from '../Model/DAO/GameDAO';
import { GameInfoDTO } from '../Model/DTO/GameInfoDTO';
import { Game } from '../Model/Game';

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
  private historyData: GameInfoDTO[] = [];
  private gameDAO: GameDAO;

  // Getter pour accéder aux données
  public getHistoryData(): GameInfoDTO[] {
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
    this.rank = this.userCookieService.getUser()!.getRank();
    this.avatar = 'https://localhost:7065/profile-pics/' + this.userPseudo;        
    this.gameDAO = new GameDAO(this.http);
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
      this.rank = this.userCookieService.getUser()!.getRank();
    });
  }

  /**
   * permet d'afficher l'historique des parties de l'utilisateurs 
   */
  public getHistory(): void {
    const token = this.userCookieService.getToken();
    this.gameDAO.GetGamesPlayed(token).subscribe((history: any[]) => {
      // Convertir les objets JSON en instances de GameInfoDTO
      this.historyData = history.map(
        (game) =>
          new GameInfoDTO(
            game.id,
            game.usernamePlayer1,
            game.usernamePlayer2,
            game.size,
            game.rule,
            game.scorePlayer1,
            game.scorePlayer2,
            game.won,
            new Date(game.date) // Conversion explicite en Date si nécessaire
          )
      );
      console.log(this.historyData); // Vérifiez ici que les objets ont bien été convertis
    });
  }

  public replayGame(game: any){

  }
  
}



