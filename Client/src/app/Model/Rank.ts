/**
 * Gère les attributs d'un utilisateur connecté
 */
export class Rank {
    private elo: number;
  
    constructor(elo: number) {
      this.elo = elo;
      // case
    }
    
    public get Elo(): number{
        return this.elo;
    }

    public get RankName(): string{
        
    }
}