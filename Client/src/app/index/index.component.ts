import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-index',
  standalone: true,
  imports: [NavbarComponent, MatIcon],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  
  // Méthode pour remplir le leaderboard avec des données fictives
  populateLeaderboard(): void {
    const leaderboard = document.querySelector('.leaderboard');
    // temporaire
    const fakeEntries = [
      '1) Victor - 9 dan',
      '2) Mathis - 7 dan',
      '3) Clément -  2 dan',
      '4) Louis - 1 kyu',
      '5) Adam - 20 kyu'
    ];
    leaderboard!.innerHTML = '';

    // Ajoute des entrées dans la div "leaderboard"
    fakeEntries.forEach(entry => {
      const p = document.createElement('p');
      p.textContent = entry;
      leaderboard!.appendChild(p);
    });
  }

  ngOnInit() {
    this.populateLeaderboard();
  }
}
