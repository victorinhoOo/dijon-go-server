import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { UploadImageComponent } from '../upload-image/upload-image.component';
import { UpdateUserDTO } from '../Model/DTO/UpdateUserDTO';
import { AuthService } from '../Model/AuthService';
import { UserDAO } from '../DAO/UserDAO';
import { HttpClient, HttpErrorResponse, HttpClientModule  } from '@angular/common/http';

@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImageComponent, HttpClientModule],
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent {

  private _profileForm!: FormGroup;
  private userDAO: UserDAO;
  private _selectedImage: any; // Stocke l'image upload
  private _errorMessage: string = ''; // Message d'erreur à afficher en cas d'échec

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<ProfileSettingsComponent>, private auth: AuthService,  private http: HttpClient) 
  {
    this.userDAO = new UserDAO(this.http);
  }

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: ['', Validators.required],
      pwd: ['', Validators.required],
      img: [null],
      email: ['', Validators.required]
    });
  }

  // Getter pour profileForm
  public get profileForm(): FormGroup {
    return this._profileForm;
  }

  // Setter pour profileForm
  public set profileForm(value: FormGroup) {
    this._profileForm = value;
  }

  // Getter pour selectedImage
  public get selectedImage(): any {
    return this._selectedImage;
  }

  // Setter pour selectedImage
  public set selectedImage(value: any) {
    this._selectedImage = value;
    this.profileForm.patchValue({ img: this._selectedImage }); // Met à jour le formulaire avec l'image
  }

  // Getter pour errorMessage
  public get errorMessage(): string {
    return this._errorMessage;
  }

  // Setter pour errorMessage
  public set errorMessage(value: string) {
    this._errorMessage = value;
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      // Création d'un objet UpdateUserDTO avec les valeurs du formulaire
      const user = new UpdateUserDTO
      (
        this.auth.getToken(),
        this.profileForm.value.pseudo,
        this.profileForm.value.email,
        this.profileForm.value.password,
        this.selectedImage,
      );
      console.log(user);
      // Appel de la méthode du DAO pour mettre à jour l'utilisateur
      this.userDAO.UpdateUser(user).subscribe({
        next: (response) => {
          console.log('Mise à jour réussie :', response);
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
            this.errorMessage = 'Une erreur est survenue lors de la mise à jour du profil';
          }
        }
      });
    } else {
      // Si le formulaire est invalide
      this.errorMessage = 'Formulaire non valide';
    }
    this.dialogRef.close();
  }
  
  // Récupère l'image uploadée
  onImageSelected(image: any) {
    this.selectedImage = image; // Utilisation du setter
  }
}
