import { Routes } from '@angular/router';
import { GameScreenComponent } from './IHM/Components/game-screen/game-screen.component';
import { GridComponent } from './IHM/Components/grid/grid.component';
import { ConnexionComponent } from './IHM/Components/connexion/connexion.component';
import { ProfileComponent } from './IHM/Components/profile/profile.component';
import { RegisterComponent } from './IHM/Components/register/register.component';
import { IndexComponent } from './IHM/Components/index/index.component';
import { HomeScreenComponent } from './IHM/Components/home-screen/home-screen.component';
import { ReplayScreenComponent } from './IHM/Components/replay-screen/replay-screen.component';

export const routes: Routes = [
    {path:"", component: HomeScreenComponent},
    {path:"grid", component: GridComponent},
    {path:"login", component: ConnexionComponent},
    {path:"profile", component: ProfileComponent},
    {path:"register", component:RegisterComponent},
    {path:"game", component: GameScreenComponent},
    {path: "index", component: IndexComponent},
    {path: "cancelled", component: IndexComponent},
    {path:"game/:size/:rule", component: GameScreenComponent},
    {path: "replay/:id/:size", component: ReplayScreenComponent}
];
