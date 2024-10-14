import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';
import { UserDAO } from '../Model/DAO/UserDAO';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from '../Model/UserCookieService';
import { Router } from '@angular/router';
import { User } from '../Model/User';

@Component({
  selector: 'app-connexion',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, HttpClientModule],
  templateUrl: './connexion.component.html',
  styleUrls: ['./connexion.component.css']
})
/**
 * Composant de connexion utilisateur
 */
export class ConnexionComponent {

  private errorMessage: string = ''; // Pour afficher les erreurs
  private successMessage: string = ''; // Pour afficher les messages de succès
  private connexionForm: FormGroup | undefined;
  private dao: UserDAO;

  public get ConnexionForm(): FormGroup {
    return this.connexionForm!;
  }
  public set ConnexionForm(value: FormGroup) {
    this.connexionForm = value;
  }

  constructor(private fb: FormBuilder, private http: HttpClient, private authService: AuthService,private router: Router) {
    this.dao = new UserDAO(this.http);
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
      this.dao.LoginUser(loginUserDTO).subscribe({
        next: (response: { token: string }) => {

          this.authService.setToken(response.token); // Stocke le token dans les cookies
          // Afficher un message de succès
          this.successMessage = 'Connexion réussie !';
          this.errorMessage = ''; // Réinitialise l'erreur

          //recuperation des infos l'utilisateur à partir de son token
          this.dao.GetUser(response.token).subscribe({
            next: (user: User) => {
              this.authService.setUser(user);
              // Redirige vers la page d'accueil
              this.router.navigate(['/index']);
            },
            error: (err) => {
              this.errorMessage = 'Erreur lors de la récupération de l\'utilisateur:', err.message;
            }
          });
        },
        error: (err) => {
          this.errorMessage = `Erreur lors de la connexion : ${err.message}`;
          this.successMessage = ''; // Réinitialise le message de succès
        }
      });
    }
  }  
}
