/**
 * Gère les attributs de l'objet de connexion utilisateur (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre un client et un serveur
 */
export class LoginUserDTO {
    private username: string;
    private password: string;
  
    constructor(username: string, password: string) {
      this.username = username;
      this.password = password;
    }
  
}
  