/**
 * Gère les attributs de l'objet d'inscription utilisateur (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre un client et un serveur
 */
export class RegisterUserDTO {
  private username: string;
  private email: string;
  private password: string;
  private profilePic: File | null;

  constructor(username: string, email: string, password: string, profilePic: File | null) {
    this.username = username;
    this.email = email;
    this.password = password;
    this.profilePic = profilePic;
  }
}
