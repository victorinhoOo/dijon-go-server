import { Component, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { PlayerListComponent } from '../player-list/player-list.component';

const CELL_CLASS_POSITION = 0

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule, PlayerListComponent],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css',
})



/**
 * Composant de la grille de jeu
 */
export class GridComponent implements OnInit, AfterViewInit{
  private size: number;
  

  public constructor(private route: ActivatedRoute){
    this.size = 0;
  }

  /**
   * Récupère la taille de la grille dans l'URL
   */
  ngOnInit(): void {
    this.size = Number(this.route.snapshot.paramMap.get('size'));
  }

  /**
   * Augmente la taille des cellules et des pierres si la grille est trop petite
   */
  ngAfterViewInit(): void {
    if (this.size < 13) {
      let cells = document.querySelectorAll('.cell, .cell-bottom');
      let stones = document.getElementsByClassName('stone');
      let arrayCells = Array.from(cells);
      let arrayStones = Array.from(stones);
      arrayCells.forEach((cell) => {
        cell.classList.remove(cell.classList[CELL_CLASS_POSITION]);
        cell.classList.add('bigger-cell');
      });
      arrayStones.forEach((stone) => {
        stone.classList.remove('stone');
        stone.classList.add('bigger-stone');
      });
    }
  }

  /**
   * Récupère la taille de la grille
   * @returns la taille de la grille
   */
  public getSize(): number {
    return this.size-1;
  }
}
