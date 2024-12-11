import { Component, OnInit } from '@angular/core';
import { UserCookieService } from '../Model/UserCookieService';
import { GameInfoDTO } from '../Model/DTO/GameInfoDTO';
import { GameDAO } from '../Model/DAO/GameDAO';
import { environment } from '../environment';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
  ],
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {

  private userPseudo: string;
  private avatar: string;
  private historyData: GameInfoDTO[];

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

  constructor(
    private userCookieService: UserCookieService, 
    private gameDAO: GameDAO,
    private router: Router
  ) {
    this.userPseudo = this.userCookieService.getUser()!.Username;
    this.avatar = "";
    this.historyData = [];
  }
  ngOnInit(): void {
    this.userPseudo = this.userCookieService.getUser()!.Username;
    this.avatar = environment.apiUrl + "/profile_pics/" + this.userPseudo;
    this.getHistory();
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
    });
  }

  public replayGame(game: any){
    this.router.navigate(['replay', game.id, game.size]);
  }

}
