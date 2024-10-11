import { Component } from '@angular/core'; 
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";
import { CommonModule } from '@angular/common';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { HttpClient, HttpErrorResponse, HttpClientModule } from '@angular/common/http';
import { UserDAO } from '../DAO/UserDAO';
import { PopupComponent } from '../popup/popup.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, UploadImageComponent, CommonModule, PopupComponent, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  // Formulaire de création de compte
  private _registerForm!: FormGroup;

  // Image sélectionnée par l'utilisateur pour l'avatar
  private _selectedImage: File | null = null;

  // Message d'erreur à afficher en cas d'échec de l'inscription
  private _errorMessage: string;

  // Instance de UserDAO pour la gestion des requêtes HTTP liées à l'utilisateur
  private _userDAO: UserDAO;

  // Contrôle la visibilité de la popup
  private _showPopup: boolean;   

  // Message à afficher dans la popup
  private _popupMessage: string;   

  // Titre de la popup
  private _popupTitle: string;     
  private lightState: string;

 

  constructor(private fb: FormBuilder, private http: HttpClient) {
    // Initialisation du UserDAO avec HttpClient
    this._userDAO = new UserDAO(this.http);
    this._popupTitle = '';
    this._popupMessage = '';
    this._showPopup = false;
    this._errorMessage = '';
    this.lightState = 'light';
  }

  // Getters et Setters

  get registerForm(): FormGroup {
    return this._registerForm;
  }

  get selectedImage(): File | null {
    return this._selectedImage;
  }

  set selectedImage(value: File | null) {
    this._selectedImage = value;
  }

  get errorMessage(): string {
    return this._errorMessage;
  }

  set errorMessage(value: string) {
    this._errorMessage = value;
  }

  get showPopup(): boolean {
    return this._showPopup;
  }

  set showPopup(value: boolean) {
    this._showPopup = value;
  }

  get popupMessage(): string {
    return this._popupMessage;
  }

  set popupMessage(value: string) {
    this._popupMessage = value;
  }

  get popupTitle(): string {
    return this._popupTitle;
  }

  set popupTitle(value: string) {
    this._popupTitle = value;
  }

  ngOnInit(): void {
    // Initialisation des variables
    this._registerForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      img: [null]
    });
  }

  onSubmit(): void {
    if (this._registerForm.valid) {
      const registerUserDTO = new RegisterUserDTO(
        this._registerForm.value.pseudo,
        this._registerForm.value.email,
        this._registerForm.value.password,
        this._selectedImage
      );

      this._userDAO.registerUser(registerUserDTO).subscribe({
        next: (response) => {
          this.errorMessage = '';  // Aucune erreur
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

  onImageSelected(image: File) {
    this.selectedImage = image;    
    this._registerForm.patchValue({ img: this._selectedImage });
  }

  openPopup() {
    this.showPopup = true;
  }

}
