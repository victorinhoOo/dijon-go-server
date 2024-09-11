import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; // ReactiveFormsModule
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent {

  profileForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: ['', Validators.required],
      pwd: ['', [Validators.required, Validators.minLength(6)]],
      
    });
    
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      console.log(this.profileForm.value);  
    }
  }
}
