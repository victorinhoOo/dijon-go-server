import { Routes } from '@angular/router';
import { GameScreenComponent } from './game-screen/game-screen.component';
import { GridComponent } from './grid/grid.component';
import { ConnexionComponent } from './connexion/connexion.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';

export const routes: Routes = [
    {path:"", component: GameScreenComponent},
    {path:"grid", component: GridComponent},
    {path:"login", component: ConnexionComponent},
    {path:"profile", component: ProfileComponent},
    {path:"register", component:RegisterComponent}
];
