import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-hamburger-btn',
  standalone: true,
  imports: [],
  templateUrl: './hamburger-btn.component.html',
  styleUrl: './hamburger-btn.component.css'
})
export class HamburgerBtnComponent {
  @Output() toggleNavbar = new EventEmitter<void>();

  onToggleNavbar(): void {
    this.toggleNavbar.emit(); 
  }
}
