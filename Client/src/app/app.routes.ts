import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';
import { IndexComponent } from './index/index.component';

export const routes: Routes = [
    {path:"", component: IndexComponent},
    {path:"grid", component: GridComponent}
];
