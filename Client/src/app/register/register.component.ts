import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";
import { CommonModule } from '@angular/common';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { UserDAO } from '../DAO/UserDAO';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, UploadImageComponent, CommonModule, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm!: FormGroup;
  selectedImage: File | null = null;
  errorMessage: string = ''; // Pour afficher les erreurs
  userDAO: UserDAO; // Instance de UserDAO

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.userDAO = new UserDAO(this.http); // Initialiser le DAO
  }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      img: [null]
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerUserDTO = new RegisterUserDTO(
        this.registerForm.value.pseudo,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.selectedImage
      );

      this.userDAO.registerUser(registerUserDTO).subscribe({
        next: (response) => {
          console.log('Inscription réussie :', response);
          this.errorMessage = ''; // Réinitialise l'erreur en cas de succès
        },
        error: (err: HttpErrorResponse) => {
          console.log('Erreur complète :', err); // Voir toute la réponse d'erreur dans la console
          if (err.status === 400 && err.error && typeof err.error === 'object' && err.error.message) {
            this.errorMessage = err.error.message;
          } else {
            this.errorMessage = 'Une erreur est survenue lors de l\'inscription';
          }
        }
      });
      
    } else {
      this.errorMessage = 'Formulaire non valide';
    }
  }

  onImageSelected(image: File) {
    this.selectedImage = image;
    this.registerForm.patchValue({ img: this.selectedImage });
  }
}
