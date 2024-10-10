import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';
import { UserDAO } from '../DAO/UserDAO';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../Model/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-connexion',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, HttpClientModule],
  templateUrl: './connexion.component.html',
  styleUrls: ['./connexion.component.css']
})
export class ConnexionComponent {
  connexionForm!: FormGroup;
  private dao: UserDAO;
  errorMessage: string = ''; // Pour afficher les erreurs
  successMessage: string = ''; // Pour afficher les messages de succès

  constructor(private fb: FormBuilder, private http: HttpClient, private authService: AuthService,private router: Router) {
    this.dao = new UserDAO(this.http);
  }

  ngOnInit(): void {

    //si je n'ai pas de token
  //  if(!this.authService.getToken())
  {
      this.connexionForm = this.fb.group({
        pseudo: ['', Validators.required],
        pwd: ['', Validators.required]
      });
    }
    //sinon rediriger vers la page d'accueil
    //else{
     // this.router.navigate(['/']);
   // }
  }

  onSubmit(): void {
    if (this.connexionForm.valid) {
      const loginUserDTO = new LoginUserDTO(
        this.connexionForm.value.pseudo,
        this.connexionForm.value.pwd
      );

      this.dao.LoginUser(loginUserDTO).subscribe({
        next: (response) => {
          this.authService.setToken(response.token); // Stocker le token
          this.authService.setUserPseudo(loginUserDTO.username);
          this.successMessage = 'Connexion réussie !';
          this.errorMessage = ''; // Réinitialise l'erreur

          //rediriger vers page d'acceuil
          this.router.navigate(['/'])
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 400 && err.error && typeof err.error === 'object' && err.error.message) {
            this.errorMessage = err.error.message; // Message d'erreur spécifique
          } else {
            this.errorMessage = 'Une erreur est survenue lors de la connexion'; // Message d'erreur générique
          }
          this.successMessage = ''; // Réinitialise le message de succès
        }
      });

    } else {
      this.errorMessage = 'Formulaire non valide'; // Message d'erreur si le formulaire est invalide
      this.successMessage = ''; // Réinitialise le message de succès
    }
  }
}
