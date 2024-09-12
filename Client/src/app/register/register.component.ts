import { Component } from '@angular/core';
import { EmailValidator, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [UploadImageComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  
  profileForm!: FormGroup;

  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      
    });}
    
  onSubmit(): void {
    if (this.profileForm.valid) {
      console.log(this.profileForm.value);  
    }
  }
}
