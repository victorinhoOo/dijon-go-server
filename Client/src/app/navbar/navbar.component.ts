import { AfterViewInit, Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements AfterViewInit {
  private lightState: string;

  public constructor(private router: Router) {
    this.lightState = 'light';
  }
  
  public ngAfterViewInit(): void {
    let button = document.getElementById("profile-button");
    button!.addEventListener("click", () => {
    this.router.navigate(["profile"]);
    });
  }
}
