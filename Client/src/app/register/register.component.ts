import { Component } from '@angular/core'; 
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";
import { CommonModule } from '@angular/common';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { HttpClient, HttpErrorResponse, HttpClientModule } from '@angular/common/http';
import { UserDAO } from '../DAO/UserDAO';
import { PopupComponent } from '../popup/popup.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, UploadImageComponent, CommonModule, PopupComponent, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
/**
 * Composant d'inscription utilisateur
 */
export class RegisterComponent {

  private registerForm!: FormGroup;
  private selectedImage: File | null = null;
  private userDAO: UserDAO;
  private showPopup: boolean;   
  private popupMessage: string;   
  private popupTitle: string;     

  /**
   * Getter pour le formulaire d'inscription
   */
  public get RegisterForm(): FormGroup {
    return this.registerForm;
  }

  public set RegisterForm(value: FormGroup) {
    this.registerForm = value;
  }

  /**
   * Getter pour le message d'erreur
   */
  public get ShowPopup(): boolean {
    return this.showPopup;
  }
  /**
   * Getter pour le message d'erreur
   */
  public get PopupTitle(): string {
    return this.popupTitle;
  }
  /**
   * Getter pour le titre de la popup
   */
  public get PopupMessage(): string {
    return this.popupMessage;
  }

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    // Initialisation du UserDAO avec HttpClient
    this.userDAO = new UserDAO(this.http);
    this.popupTitle = '';
    this.popupMessage = '';
    this.showPopup = false;
  }

  /**
   * Initialisation du formulaire de création de compte
   */
  public ngOnInit(): void {
    // Initialisation des variables
    this.registerForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      img: [null]
    });
  }

  /**
   * Méthode appelée lors de la soumission du formulaire d'inscription
   * Créé un objet RegisterUserDTO avec les valeurs du formulaire et envoie transmet au serveur
   * Redirige l'utilisateur vers la page de connexion en cas de succès sinon affiche un message d'erreur dans un popup
   */
  public onSubmit(): void {
    if (this.registerForm.valid) {
      const registerUserDTO = new RegisterUserDTO(
        this.registerForm.value.pseudo,
        this.registerForm.value.email,
        this.registerForm.value.password,
        this.selectedImage
      );
      this.userDAO.registerUser(registerUserDTO).subscribe({
        next: (response: { message: string }) => {
          this.popupMessage = response.message;  // Aucune erreur
          this.popupTitle = 'Inscription réussie';
          this.openPopup();  // Affiche le popup en cas de succès
          this.router.navigate(['/login']);
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 400 && err.error && typeof err.error === 'object' && err.error.message) {
            this.popupMessage = err.error.message;  // Message d'erreur personnalisé
          } else {
            this.popupMessage = 'Une erreur est survenue lors de l\'inscription';
          }
          this.popupTitle = 'Erreur lors de l\'inscription';
          this.openPopup();  // Affiche le popup en cas d'erreur
        }
      });
      
    } else {
      this.popupMessage = 'Formulaire non valide. Veuillez corriger les erreurs.';
      this.popupTitle = 'Erreur';
      this.openPopup();  // Affiche le popup en cas de formulaire invalide
    }
  }

  /**
   * Méthode appelée lors de la sélection d'une image
   * @param image Image sélectionnée par l'utilisateur
   */
  public onImageSelected(image: File) {
    this.selectedImage = image;    
    this.registerForm.patchValue({ img: this.selectedImage });
  }

  /**
   * Ouvre la popup
   */
  private openPopup() {
    this.showPopup = true;
  }

}
