import { Component } from '@angular/core'; 
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UploadImageComponent } from "../upload-image/upload-image.component";
import { CommonModule } from '@angular/common';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { HttpClient, HttpErrorResponse, HttpClientModule } from '@angular/common/http';
import { UserDAO } from '../Model/DAO/UserDAO';
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
  private confirmPwdIsGood :boolean;
  private registerSucces :boolean; //l'inscription 
  private isStrongPassword :boolean;

  /**
   * Verifie si le password est stong
   */
  public get IsStrongPassword() : boolean
  {
    return this.isStrongPassword;
  }

  /**
   * Indique si l'inscription
   */
  public get RegisterSucces() :boolean
  {
    return  this.registerSucces;
  }
  /**
   * Est vrai si le mdp et sa confirmation sont vrai
   */
  public get ConfirmPwdIsGood() :boolean
  {
    return this.confirmPwdIsGood;
  }
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
 * Setter pour l'ouverture de la popup
 */
  public set ShowPopup(value :boolean)
  {
    this.showPopup = value;
  }
  /**
 * Getter pour le titre de la popup
 */
  public get PopupMessage(): string {
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
   * Getter pour le message d'erreur
   */
  public get PopupTitle(): string {
    return this.popupTitle;
  }


  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    // Initialisation du UserDAO avec HttpClient
    this.userDAO = new UserDAO(this.http);
    this.popupTitle = '';
    this.popupMessage = '';
    this.showPopup = false;
    this.confirmPwdIsGood = true;
    this.registerSucces = false;
    this.isStrongPassword = false;
  }

  /**
   * Initialisation du formulaire de création de compte
   */
  public ngOnInit(): void {
    // Initialisation des variables
    this.registerForm = this.fb.group({
      pseudo: ['', Validators.required],
      password: ['', [Validators.required,Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]], // 8 caractères, une maj, une min et 1 chiffre min
      email: ['', [Validators.required, Validators.email]],
      confirmPassword: ['', Validators.required],
      img: [null]
    });
      //reinitialise les attributs au rechargement de la page d'inscription
      this.InitializePwdForm();
  }

  /**
   * Méthode appelée lors de la soumission du formulaire d'inscription
   * Créé un objet RegisterUserDTO avec les valeurs du formulaire et envoie transmet au serveur
   * Redirige l'utilisateur vers la page de connexion en cas de succès sinon affiche un message d'erreur dans un popup
   */
  public onSubmit(): void {

    //reinitialise les attributs a chaque  nouvel envoie de formulaire
    this.InitializePwdForm();

  //si le formulaire est correctement remplie
  if (this.registerForm.valid && this.registerForm.value.password == this.registerForm.value.confirmPassword ) 
      {
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
          this.registerSucces = true;
        },
        error: (err: HttpErrorResponse) => 
        {
          this.PopupMessage = err.message;
          this.popupTitle = 'Erreur lors de l\'inscription :';
          this.registerSucces = false; //reinitialise le succe du formulaire si l'utilisateur reesaie de se register
        }
      });
      
    } 
  else //erreur dans le formulaire
    {
      this.popupMessage = 'Formulaire non valide. Veuillez corriger les erreurs.';
      this.popupTitle = 'Erreur :';

      if(this.registerForm.value.password != this.registerForm.value.confirmPassword)
      {
        this.confirmPwdIsGood = false;
      }
      //verifie la validiter du format du password
      if (this.registerForm.get('password')?.hasError('pattern')) 
      {
        this.isStrongPassword = false;
      }
      this.registerSucces = false; //reinitialise le succe du formulaire si l'utilisateur reesaie de se register
    }
    this.ShowPopup = true; //ouverture pop up
  }

  /**
   * Méthode appelée lors de la sélection d'une image
   * @param image Image sélectionnée par l'utilisateur
   */
  public onImageSelected(image: File) {
    this.selectedImage = image;    
    this.registerForm.patchValue({ img: this.selectedImage });
  }

  public handlePopupClose(): void {
    this.showPopup = false;
  }

    /**
   * Inialise les attributs liés au password du formulaire
   */
    public  InitializePwdForm() :void
    {
      this.confirmPwdIsGood = true;
      this.isStrongPassword = true;
    }

}
