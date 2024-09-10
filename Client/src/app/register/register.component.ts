import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  avatarFile: File | null = null;
  
  onFileSelected(event: any): void {
    this.avatarFile = event.target.files[0];
  }

  onSubmit(): void {
    console.log('Form submitted');
  }
}
