import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';

export class UserDAO {
  
  // URL de base pour les requêtes vers l'API utilisateur
  private readonly url = 'https://localhost:7065/User/'; 

  /**
   * Constructeur de la classe UserDAO.
   * Initialise HttpClient pour effectuer des requêtes HTTP
   * @param http HttpClient utilisé pour envoyer les requêtes HTTP
   */
  constructor(private http: HttpClient) {}

  /**
   * Envoie une requête POST au serveur pour enregistrer un nouvel utilisateur
   * @param user Objet contenant les données de l'utilisateur à enregistrer (RegisterUserDTO)
   * @returns Un Observable qui émet la réponse du serveur ou une erreur
   */
  registerUser(user: RegisterUserDTO): Observable<any> {
    try {
      // Création d'un objet FormData pour envoyer les données utilisateur 
      const formData: FormData = new FormData();
      // Ajoute le pseudo, l'email et le mot de passe à la requête
      formData.append('Username', user.username);
      formData.append('Email', user.email);
      formData.append('Password', user.password);
      // Si l'utilisateur a sélectionné une image de profil elle est ajoutée 
      if (user.profilePic) {
        formData.append('ProfilePic', user.profilePic);
      }
      // Envoie la requête POST à l'URL spécifiée avec les données de l'utilisateur
      return this.http.post(this.url + 'Register', formData); // Retourne un Observable de la réponse HTTP
    } 
    catch (error) {
      console.error('Erreur lors de la préparation des données pour l\'inscription', error);
      throw error;// Lancer l'erreur pour qu'elle puisse être traitée par le composant appelant
    }

  }

  //Methode pour envoyer le login
  LoginUser(user: LoginUserDTO): Observable<any>{
      try{

        const formData: FormData = new FormData();
        formData.append('Username', user.username);
        formData.append('Password', user.password);
        return this.http.post(this.url + 'Login', formData); // Retourne un Observable
      }

      catch (error) 
      {
          console.error('Erreur lors de la préparation des données pour la connexion', error);
          throw error;
      }
  
  }
}

