import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-rankprogress',
  standalone: true,
  imports: [MatIcon,CommonModule],
  templateUrl: './rankprogress.component.html',
  styleUrl: './rankprogress.component.css'
})
export class RankprogressComponent {
    
  // Attribut privé pour stocker la progression limite de la barre
  private targetProgress: number;

  // Attribut privé pour stocker le Elo de l'utilisateur
  private elo: number;

  //Classe CSS pour la ligne de la barre de progression
  private color : string; 

  //Classe CSS pour l'arrière-plan de la barre de progression
  private backColor : string; 

  /**
   * Getter public pour récupérer la classe CSS actuelle de la barre de progression.
   * @returns La classe CSS associée à la ligne de progression.
   */
  public get getColor():string{
    return this.color
  }
  
  /**
   * Constructeur du composant.
   * Initialise les valeurs par défaut et calcule la progression cible.
   * @param userCookieService Service pour gérer les cookies utilisateur.
   */
  public constructor(private userCookieService: UserCookieService) {
    this.color = 'progress-low';
    this.backColor ='progress-low-back';
    this.elo = this.userCookieService.getUser()!.Elo || 0;
    this.targetProgress = this.progressRate(this.elo); //initialisation de le poucentage de progression 
  }

   /**
   * Calcule la progression cible en fonction du Elo de l'utilisateur.
   * @param elo Niveau Elo de l'utilisateur.
   * @returns La progression cible (en pourcentage).
   */
  private progressRate(elo: number): number {
    // Définit les limites Elo pour chaque rang
    const rankLimits = [
      { rank: "20 Kyu", minElo: 0, maxElo: 100 },
      { rank: "19 Kyu", minElo: 100, maxElo: 200 },
      { rank: "18 Kyu", minElo: 200, maxElo: 300 },
      { rank: "17 Kyu", minElo: 300, maxElo: 400 },
      { rank: "16 Kyu", minElo: 400, maxElo: 500 },
      { rank: "15 Kyu", minElo: 500, maxElo: 600 },
      { rank: "14 Kyu", minElo: 600, maxElo: 700 },
      { rank: "13 Kyu", minElo: 700, maxElo: 800 },
      { rank: "12 Kyu", minElo: 800, maxElo: 900 },
      { rank: "11 Kyu", minElo: 900, maxElo: 1000 },
      { rank: "10 Kyu", minElo: 1000, maxElo: 1100 },
      { rank: "9 Kyu", minElo: 1100, maxElo: 1200 },
      { rank: "8 Kyu", minElo: 1200, maxElo: 1300 },
      { rank: "7 Kyu", minElo: 1300, maxElo: 1400 },
      { rank: "6 Kyu", minElo: 1400, maxElo: 1500 },
      { rank: "5 Kyu", minElo: 1500, maxElo: 1600 },
      { rank: "4 Kyu", minElo: 1600, maxElo: 1700 },
      { rank: "3 Kyu", minElo: 1700, maxElo: 1800 },
      { rank: "2 Kyu", minElo: 1800, maxElo: 1900 },
      { rank: "1 Kyu", minElo: 1900, maxElo: 2000 },
      { rank: "1 Dan", minElo: 2000, maxElo: 2200 },
      { rank: "2 Dan", minElo: 2200, maxElo: 2400 },
      { rank: "3 Dan", minElo: 2400, maxElo: 2600 },
      { rank: "4 Dan", minElo: 2600, maxElo: 2800 },
      { rank: "5 Dan", minElo: 2800, maxElo: 3000 },
      { rank: "6 Dan", minElo: 3000, maxElo: 3200 },
      { rank: "7 Dan", minElo: 3200, maxElo: 3400 },
      { rank: "8 Dan", minElo: 3400, maxElo: 3600 },
      { rank: "9 Dan", minElo: 3600, maxElo: Infinity }
    ];

    let currentRank = "";
    let currentMinElo = 0;
    let currentMaxElo = 0;
    // Recherche du rang correspondant au Elo
    for (let i = 0; i < rankLimits.length; i++) {
      if (elo >= rankLimits[i].minElo && elo < rankLimits[i].maxElo) {
        currentRank = rankLimits[i].rank;
        currentMinElo = rankLimits[i].minElo;
        currentMaxElo = rankLimits[i].maxElo;
        break;
      }
    }
    // Calcul du pourcentage de progression dans le rang
    const targetProgress = (elo - currentMinElo) / (currentMaxElo - currentMinElo) * 100;
    return targetProgress;
  }

  /**
   * Détermine la classe CSS pour la ligne de progression en fonction de la progression cible.
   * @returns La classe CSS correspondante.
   */
  public getProgressClass(): string {
    if (this.targetProgress < 33) {
      this.color = 'progress-low';
    } else if (this.targetProgress < 66) {
      this.color = 'progress-medium';
    } else {
      this.color = 'progress-high';
    }
    return this.color;
  }
}