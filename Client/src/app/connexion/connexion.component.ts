import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';
import { UserDAO } from '../Model/DAO/UserDAO';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../Model/UserCookieService';
import { Router } from '@angular/router';
import { User } from '../Model/User';
import { PopupComponent } from '../popup/popup.component';

@Component({
  selector: 'app-connexion',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, HttpClientModule, PopupComponent],
  templateUrl: './connexion.component.html',
  styleUrls: ['./connexion.component.css']
})
/**
 * Composant de connexion utilisateur
 */
export class ConnexionComponent {

  private connexionForm: FormGroup | undefined;
  private dao: UserDAO;
  private showPopup: boolean;   
  private popupMessage: string;   
  private popupTitle: string;

    /**
   * Getter pour le formulaire de connexion
   */
  public get ConnexionForm(): FormGroup {
    return this.connexionForm!;
  }

   /**
   * Setter pour le message d'erreur
   */
  public set ConnexionForm(value: FormGroup) {
    this.connexionForm = value;
  }
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
     * constructor de la page de connexion
     * @param fb le createur de formulaire
     * @param http l'adresse
     * @param authService les informations d'authentification
     * @param router le routage des pages
     */
  
  constructor(private fb: FormBuilder, private http: HttpClient, private authService: AuthService,private router: Router) 
  {
    this.dao = new UserDAO(this.http);
    this.popupTitle = '';
    this.popupMessage = '';
    this.showPopup = false;
  }

  /**
   * Initialisation du formulaire de connexion
   */
  public ngOnInit(): void {
  // si je n'ai pas de token utilisateur alors je crée le formulaire de connexion
  if(!this.authService.getToken())
  {
      this.connexionForm = this.fb.group({
        pseudo: ['', Validators.required],
        pwd: ['', Validators.required]
      });
    }
    //sinon rediriger vers la page d'accueil
    else{
      this.router.navigate(['/index']);
    }
  }

  /**
   * Méthode appelée lors de la soumission du formulaire de connexion
   * Essaye de se connecter avec les informations fournies
   * puis récupère les informations utilisateur à partir du token retourné
   * affiche un message de succès si la connexion est réussie et redirige vers la page d'accueil
   * sinon affiche un message d'erreur
   */
  public onSubmit(): void {
    if (this.ConnexionForm.valid) {
      const loginUserDTO = new LoginUserDTO(
        this.ConnexionForm.value.pseudo,
        this.ConnexionForm.value.pwd
      ); 
      // Essaye de se connecter
      const token = this.dao.LoginUser(loginUserDTO).subscribe({
        next: (response) => 
          {
            this.popupMessage = response.message;  // Aucune erreur
            this.PopupTitle = 'Connexion réussie';
            this.authService.setToken(response.message); // Stocke le token dans les cookies
            
            //recuperation des infos l'utilisateur à partir de son token
            this.dao.GetUser(response).subscribe({
              next: (user: User) => 
              {
                this.authService.setUser(user);
                // Redirige vers la page d'accueil
                this.router.navigate(['/index']);
              },
              error: (err :HttpErrorResponse) => 
              {
                //affiche le message d'erreur du serveur
                this.popupMessage = err.message;
                this.PopupTitle = 'Erreur';
              }
            });
          },
        error: (err :HttpErrorResponse) =>
         {
          //affiche le message d'erreur du serveur
          this.PopupMessage = err.message;
         }
      });

    }
    this.ShowPopup = true;
  }  
}
