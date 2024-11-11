import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hamburger-btn',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hamburger-btn.component.html',
  styleUrls: ['./hamburger-btn.component.css']
})
export class HamburgerBtnComponent {
  @Output() toggleNavbar: EventEmitter<boolean> = new EventEmitter<boolean>();
  isButtonClicked = false;
  

  onToggleNavbar(): void {
    this.toggleNavbar.emit(); 
    this.isButtonClicked = true;
  }
}
