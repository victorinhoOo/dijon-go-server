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

    public get Username(): string {
        return this.username;
    }
    public get Email(): string {
        return this.email;
    }
  
}