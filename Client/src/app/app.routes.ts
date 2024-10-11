import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';
import { IndexComponent } from './index/index.component';
import { HomeScreenComponent } from './home-screen/home-screen.component';

export const routes: Routes = [
    {path:"", component: HomeScreenComponent},
    {path:"index", component: IndexComponent},
    {path:"game", component: GameScreenComponent},
    {path:"grid", component: GridComponent}
];
