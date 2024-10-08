import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';
import { ConnexionComponent } from './connexion/connexion.component';

export const routes: Routes = [
    {path:"", component: GameScreenComponent},
    {path:"grid", component: GridComponent},
    {path:"connexion", component: ConnexionComponent}
];
