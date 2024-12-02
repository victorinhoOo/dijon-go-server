import { Component } from '@angular/core';
import { GridComponent } from '../grid/grid.component';

@Component({
  selector: 'app-game-screen',
  standalone: true,
  imports: [GridComponent],
  templateUrl: './game-screen.component.html',
  styleUrl: './game-screen.component.css'
})
/**
 * Composant de l'écran de jeu
 */
export class GameScreenComponent {
}
