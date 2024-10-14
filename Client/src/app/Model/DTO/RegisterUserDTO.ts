/**
 * Gère les attributs de l'objet d'inscription utilisateur (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre un client et un serveur
 */
export class RegisterUserDTO {
  private username: string;
  private email: string;
  private password: string;
  private profilePic: File | null;

  /**
   * Renvoi le nom de l'utilisateur
   */
  public get ProfilePic(): File | null {
    return this.profilePic;
  }

  /**
   * Renvoi le mot de passe de l'utilisateur
   */
  public get Password(): string {
    return this.password;
  }

  /**
   * Renvoi l'email de l'utilisateur
   */
  public get Email(): string {
    return this.email;
  }

  /**
   * Renvoi le nom de l'utilisateur
   */
  public get Username(): string {
    return this.username;
  }

  constructor(username: string, email: string, password: string, profilePic: File | null) {
    this.username = username;
    this.email = email;
    this.password = password;
    this.profilePic = profilePic;
  }
}
