import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { UploadImageComponent } from '../upload-image/upload-image.component';
import { UpdateUserDTO } from '../Model/DTO/UpdateUserDTO';
import { AuthService } from '../Model/UserCookieService';
import { UserDAO } from '../Model/DAO/UserDAO';
import { HttpClient, HttpErrorResponse, HttpClientModule } from '@angular/common/http';
import { User } from '../Model/User';
import { PopupComponent } from '../popup/popup.component';

@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImageComponent, HttpClientModule, PopupComponent],
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
/**
 * Composant de paramétrage du profil utilisateur
 */
export class ProfileSettingsComponent {

  private profileForm!: FormGroup;
  private userDAO: UserDAO;
  private token: string;
  private selectedImage: any;
  private userPseudo: string;
  private userEmail: string;
  private showPopup: boolean;   
  private popupMessage: string;   
  private popupTitle: string;

   /**
   * Getter pour l'ouverture de la popup
   */
    public get ShowPopup(): boolean {
      return this.showPopup;
    }
    /**
     * Setter pour l'ouverture de la popup
     */
    public set ShowPopup(value :boolean)
    {
      this.showPopup = value;
    }
    /**
     * Getter pour le message d'erreur
     */
    public get PopupTitle(): string {
      return this.popupTitle;
    }

    /**
     * Setter pour le titre de la popup
     */
    public set PopupTitle(value: string)
    {
      this.popupTitle = value;
    }

    /**
     * Getter pour le message de la popup
     */
    public get PopupMessage() : string
    {
      return this.popupMessage;
    }
    /**
     * Setter pour le message de la popup
     */
    public set PopupMessage(value :string)
    {
      this.popupMessage = value;
    }

  /**
   * Getter pour userPseudo
   * @returns le pseudo de l'utilisateur
   * */
  public get UserPseudo(): string {
    return this.userPseudo;
  }
  /**
   * Getter pour userEmail
   * @returns l'email de l'utilisateur
   */
  public get UserEmail(): string {
    return this.userEmail;
  }

  // Getter pour profileForm
  public get ProfileForm(): FormGroup {
    return this.profileForm;
  }

  // Getter pour selectedImage
  public get SelectedImage(): any {
    return this.selectedImage;
  }

  // Setter pour selectedImage
  public set SelectedImage(value: any) {
    this.selectedImage = value;
    this.profileForm.patchValue({ img: this.selectedImage }); // Met à jour le formulaire avec l'image
  }
  /**
   * Initialise le composant en créant un objet UserDAO et en récupérant les informations de l'utilisateurice 
   */
  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<ProfileSettingsComponent>, private authService: AuthService, private http: HttpClient) {
    this.userDAO = new UserDAO(this.http);
    this.token = this.authService.getToken();
    this.userPseudo = this.authService.getUser().Username;
    this.userEmail = this.authService.getUser().Email;
    this.popupTitle = '';
    this.popupMessage = '';
    this.showPopup = false;
  }

  /**
   * Lancé à la fin de l'initialisation du composant, crée le formulaire de paramétrage du profil
   */
  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: [this.userPseudo],
      pwd: ['',],
      img: [null],
      email: [this.UserEmail]
    });
  }

  /**
   * Méthode appelée lors de la soumission du formulaire de paramétrage du profil
   * Envoie les informations du formulaire au serveur pour mettre à jour le profil
   * Met ensuite à jour les informations de l'utilisateur dans les cookies puis ferme la popup
   */
  public onSubmit(): void {
    if (this.profileForm.valid) 
      {
        // Création d'un objet UpdateUserDTO avec les valeurs du formulaire
        const user = new UpdateUserDTO
          (
            this.token,
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
            this.PopupTitle =" Succès :";
            this.PopupMessage = "Les modifications ont bien été validées";


            // Met à jour les informations de l'utilisateur dans les cookies
            this.userDAO.GetUser(this.token).subscribe({
              next: (user: User) => {
                this.authService.setUser(user);
                window.location.reload();
              },
              error: (err: HttpErrorResponse) => {
                this.PopupTitle = 'Erreur :';
                this.popupMessage =  err.message;
              }
            });
          },
          error: (err: HttpErrorResponse) => {
            // En cas d'erreur
            this.PopupTitle = ' Erreur :';
            this.PopupMessage = err.message;
          }
        });
  }
   else {
      // Si le formulaire est invalide
      this.PopupTitle = "Erreur dans le formulaire"
      this.PopupMessage = 'Formulaire non valide';
    }
    this.ShowPopup = true;
  }

  // Récupère l'image uploadée par l'utilisateur
  onImageSelected(image: any) {
    this.selectedImage = image; 
  }
}
