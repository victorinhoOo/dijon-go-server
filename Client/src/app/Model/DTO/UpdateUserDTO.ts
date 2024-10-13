/**
 * Gère les attributs de l'objet de mise à jour utilisateur (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre un client et un serveur
 */
export class UpdateUserDTO {
    private tokenuser: string;
    private username: string;
    private email: string;
    private password: string;
    private profilePic: File | null;
  
  
    constructor(token: string, user: string, mail: string,password: string, profile: File) 
    {
        this.tokenuser = token;
        this.username = user;
        this.email = mail;
        this.password = password;
        this.profilePic = profile;

    }
}
