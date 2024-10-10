import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterUserDTO } from '../Model/DTO/RegisterUserDTO';
import { LoginUserDTO } from '../Model/DTO/LoginUserDTO';

export class UserDAO {
  private readonly url = 'https://localhost:7065/User/'; // URL de l'API

  constructor(private http: HttpClient) {}

  // Méthode pour envoyer les données via POST
  registerUser(user: RegisterUserDTO): Observable<any> {
    try{
        const formData: FormData = new FormData();
        formData.append('Username', user.username);
        formData.append('Email', user.email);
        formData.append('Password', user.password);
        if (user.profilePic) 
        {
            formData.append('ProfilePic', user.profilePic);
        }
        return this.http.post(this.url + 'Register', formData); // Retourne un Observable
    }
    catch (error) 
    {
        console.error('Erreur lors de la préparation des données pour l\'inscription', error);
        throw error;
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
