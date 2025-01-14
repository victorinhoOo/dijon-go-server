import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { UploadImageComponent } from '../upload-image/upload-image.component';
import { UpdateUserDTO } from '../../../Model/DTO/UpdateUserDTO';
import { UserCookieService } from '../../../Model/services/UserCookieService';
import { UserDAO } from '../../../Model/DAO/UserDAO';
import { HttpClient, HttpErrorResponse, HttpClientModule } from '@angular/common/http';
import { User } from '../../../Model/User';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile-settings',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImageComponent, HttpClientModule],
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
  private oldPwdEmpty: boolean; //si l'ancien pwd est vide
  private confirmPwdIsGood: boolean; //si le pwd et = au confirm pwd
  private isStrongPassword: boolean; //si le pwd est strong

  /**
 * Est vrai si le mdp et sa confirmation sont vrai
 */
  public get ConfirmPwdIsGood(): boolean {
    return this.confirmPwdIsGood;
  }
  /**
   * Verifie si le password est stong
   */
  public get IsStrongPassword(): boolean {
    return this.isStrongPassword;
  }

  /**
   * renvoie si l'ancien mot de passe est vide ou non
   */
  public get OldPwdEmpty(): boolean {
    return this.oldPwdEmpty;
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
  constructor(private fb: FormBuilder, private userCookieService: UserCookieService, private http: HttpClient, private dialogRef: MatDialogRef<ProfileSettingsComponent> ) {
    this.userDAO = new UserDAO(this.http);
    this.token = this.userCookieService.getToken();
    this.userPseudo = this.userCookieService.getUser()!.Username;
    this.userEmail = this.userCookieService.getUser()!.Email;
    this.oldPwdEmpty = false;
    this.confirmPwdIsGood = true;
    this.isStrongPassword = true;
  }

  /**
   * Lancé à la fin de l'initialisation du composant, crée le formulaire de paramétrage du profil
   */
  ngOnInit(): void {
    this.profileForm = this.fb.group({
      pseudo: [''],
      oldpwd: ['', Validators.required],
      pwd: ['', [Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]], //8 caractères, une maj, une min et 1 chiffre minimum
      Cpwd: [''],
      img: [null],
      email: ['']
    });
    //reinitialise les attributs pour si la page est rafraichie
    this.InitializePwdForm();
  }

  /**
   * Méthode appelée lors de la soumission du formulaire de paramétrage du profil
   * Envoie les informations du formulaire au serveur pour mettre à jour le profil
   * Met ensuite à jour les informations de l'utilisateur dans les cookies puis ferme la popup
   */
  public onSubmit(): void {
    this.InitializePwdForm();

    if (this.profileForm.valid && this.CheckIfConfirmPwdIsGood()) {
      const user = new UpdateUserDTO(
        this.token,
        this.profileForm.value.pseudo,
        this.profileForm.value.email,
        this.profileForm.value.oldpwd,
        this.profileForm.value.pwd,
        this.selectedImage,
      );

      this.userDAO.UpdateUser(user).subscribe({
        next: () => {
          this.userDAO.GetUser(this.token).subscribe({
            next: (user: User) => {
              this.userCookieService.setUser(user);
              const Toast = Swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true
              });
              Toast.fire({
                icon: "success",
                title: "Modification réussie",
              });
              this.dialogRef.close();
            },
            error: (err: HttpErrorResponse) => {
              this.showErrorAlert(err.message);
            }
          });
        },
        error: (err: HttpErrorResponse) => {
          this.showErrorAlert(err.message);
        }
      });
    } else {
      //verification de la saisie de l'ancien pwd
      if (this.profileForm.value.oldpwd === '') {
        this.oldPwdEmpty = true;
      }
      //verification de la solidité du pwd
      if (this.profileForm.get('pwd')?.hasError('pattern')) {
        this.isStrongPassword = false;
      }
      //verification de la confirmation du pwd
      if (!this.CheckIfConfirmPwdIsGood()) {
        this.confirmPwdIsGood = false;
      }
    }
  }

  /**
   * Affiche un popup d'erreur 
   * @param message message de l'erreur à afficher
   */
  private showErrorAlert(message: string) {
    Swal.fire({
      title: 'Erreur',
      text: message,
      icon: 'error',
      confirmButtonText: 'Fermer',
      showCloseButton: true,
      customClass: {
        confirmButton: 'custom-ok-button'
      },
    });
  }

  // Récupère l'image uploadée par l'utilisateur
  public onImageSelected(image: any) {
    this.selectedImage = image;
  }

  /**
 * Renvoie vrai si le password et confimpassword sont pareil
 */
  public CheckIfConfirmPwdIsGood(): boolean {
    let result = false;
    if (this.profileForm.value.pwd == this.profileForm.value.Cpwd)
      result = true;
    return result;
  }

  /**
   * Inialise les attributs liés au password du formulaire
   */
  public InitializePwdForm(): void {
    this.confirmPwdIsGood = true;
    this.isStrongPassword = false;
  }
}
