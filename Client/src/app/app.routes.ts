import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';

export const routes: Routes = [
    {path:":size", component: GameScreenComponent},
    {path:"grid", component: GridComponent}
];
