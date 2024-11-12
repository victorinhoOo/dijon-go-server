/**
 * Gère les attributs d'un utilisateur connecté
 */
export class User {
    private username: string;
    private email: string;
  
    constructor(username: string, email: string) {
      this.username = username;
      this.email = email;
    }

    /**
     * Renvoi le nom d'utilisateur de l'utilisateur
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
  
}