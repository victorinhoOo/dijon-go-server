import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';
import { ConnexionComponent } from './connexion/connexion.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { IndexComponent } from './index/index.component';
import { HomeScreenComponent } from './home-screen/home-screen.component';

export const routes: Routes = [
    {path:"", component: HomeScreenComponent},
    {path:"grid", component: GridComponent},
    {path:"login", component: ConnexionComponent},
    {path:"profile", component: ProfileComponent},
    {path:"register", component:RegisterComponent},
    {path:"game", component: GameScreenComponent},
    {path: "index", component: IndexComponent},
    {path:":size", component: GameScreenComponent}
];
