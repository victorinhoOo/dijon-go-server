import { Component, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';



@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css',
})



/**
 * Composant de la grille de jeu
 */
export class GridComponent implements OnInit{
  private size: number;
  private playerAvatar: string;
  private playerPseudo: string;
  

  public constructor(private route: ActivatedRoute){
    this.size = 0;
    this.playerAvatar = "";
    this.playerPseudo = "";
  }
  ngOnInit(): void {
    this.size = Number(this.route.snapshot.paramMap.get('size'));
  }

  public getSize(): number {
    return this.size;
  }
}
