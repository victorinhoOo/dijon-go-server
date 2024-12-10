import { Component, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { PlayerListComponent } from '../player-list/player-list.component';


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
export class GridComponent implements OnInit{
  private size: number;
  

  public constructor(private route: ActivatedRoute){
    this.size = 0;
  }
  ngOnInit(): void {
    this.size = Number(this.route.snapshot.paramMap.get('size'));
  }

  public getSize(): number {
    return this.size - 1;
  }
}
