import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";
import { CommonModule } from '@angular/common';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { HttpClient, HttpErrorResponse, HttpClientModule  } from '@angular/common/http';
import { UserDAO } from '../DAO/UserDAO';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, UploadImageComponent, CommonModule, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  // Formulaire de création de compte
  registerForm!: FormGroup;

  // Image sélectionnée par l'utilisateur pour l'avatar
  selectedImage: File | null = null;

  // Message d'erreur à afficher en cas d'échec de l'inscription
  errorMessage: string = '';

  // Instance de UserDAO pour la gestion des requêtes HTTP liées à l'utilisateur
  userDAO: UserDAO;

  /**
   * Constructeur de RegisterComponent.
   * Initialise le DAO et le FormBuilder.
   * 
   * @param fb FormBuilder pour la création de formulaires réactifs
   * @param http HttpClient pour les requêtes HTTP
   */
  constructor(private fb: FormBuilder, private http: HttpClient) {
    // Initialisation du UserDAO avec HttpClient
    this.userDAO = new UserDAO(this.http);
  }

  /**
   * ngOnInit est appelé une fois que le composant est initialisé
   * Crée le formulaire avec ses champs et leurs validateurs
   */
  ngOnInit(): void {
    // Initialisation du formulaire avec des validateurs
    this.registerForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      img: [null]  // Champ pour l'image de profil
    });
  }

  /**
   * Méthode appelée lors de la soumission du formulaire
   * Elle envoie les données d'inscription au serveur si le formulaire est valide
   */
  onSubmit(): void {
    if (this.registerForm.valid) {
      // Création d'un objet RegisterUserDTO avec les valeurs du formulaire
      const registerUserDTO = new RegisterUserDTO(
        this.registerForm.value.pseudo,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.selectedImage
      );

      // Appel de la méthode du DAO pour enregistrer l'utilisateur
      this.userDAO.registerUser(registerUserDTO).subscribe({
        next: (response) => {
          console.log('Inscription réussie :', response);
          this.errorMessage = '';  // Aucune erreur
        },
        error: (err: HttpErrorResponse) => {
          // En cas d'erreur
          console.log('Erreur complète :', err);
          // S'il y a un message d'erreur provenant du serveur
          if (err.status === 400 && err.error && typeof err.error === 'object' && err.error.message) {
            // Affiche le message d'erreur du serveur
            this.errorMessage = err.error.message;
          } else {
            // Affiche un message d'erreur générique si le serveur ne fournit pas d'information détaillée
            this.errorMessage = 'Une erreur est survenue lors de l\'inscription';
          }
        }
      });

    } else {
      // Si le formulaire est invalide
      this.errorMessage = 'Formulaire non valide';
    }
  }

  /**
   * Méthode appelée lors de la sélection d'une image par l'utilisateur
   * Elle met à jour la propriété selectedImage et le champ correspondant dans le formulaire
   * @param image Fichier sélectionné par l'utilisateur
   */
  onImageSelected(image: File) {
    this.selectedImage = image;    
    this.registerForm.patchValue({ img: this.selectedImage });// Mise à jour du champ 'img' dans le formulaire avec l'image sélectionnée
  }
}
