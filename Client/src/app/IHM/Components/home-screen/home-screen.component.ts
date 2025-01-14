import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-screen',
  standalone: true,
  imports: [MatIcon],
  templateUrl: './home-screen.component.html',
  styleUrl: './home-screen.component.css',
})
/**
 * Composant de la page d'accueil
 */
export class HomeScreenComponent {

  constructor(private userCookieService: UserCookieService, private router: Router){
    if (this.userCookieService.getToken() != '') {
      this.router.navigate(['/index']);
    }
  }
}
