/**
 * Gère les attributs de l'objet de mise à jour utilisateur (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre un client et un serveur
 */
export class UpdateUserDTO {
    private tokenuser: string;
    private username: string;
    private email: string;
    private oldpassword: string
    private password: string;
    private profilePic: File | null;

    /**
     * Renvoi le token de l'utilisateur
     */
    public get TokenUser(): string {
        return this.tokenuser;
    }
    /**
     * Renvoi le nom de l'utilisateur
     */
    public get Username(): string {
        return this.username;
    }
    /**
    * Renvoi l'email de l'utilisateur
    */
    public get Email(): string {
        return this.email;
    }
    
    /**
     * Renvoi l'ancien mot de passe de l'utilisateur
     */
    public get OldPassword(): string {
        return this.oldpassword;
    }

    /**
     * Renvoi le mot de passe de l'utilisateur
     */
    public get Password(): string {
        return this.password;
    }
    /**
     * Renvoi l'image de profil de l'utilisateur
     */
    public get ProfilePic(): File | null {
        return this.profilePic;
    }
  
    constructor(token: string, user: string, mail: string, oldpassword: string, password: string, profile: File) 
    {
        this.tokenuser = token;
        this.username = user;
        this.email = mail;
        this.oldpassword = oldpassword;
        this.password = password;
        this.profilePic = profile;

    }
}
