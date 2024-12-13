/**
 * Gère les attributs des informations de partie qu'il est possible de rejoindre (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre le serveur et le client
 */
export class AvailableGameInfoDTO {
    private id: number;
    private size: number;
    private rule: string;
    private creatorName : string;
    private komi : number;
    private name: string;
    private handicap: number;

    private handicapColor: string;

    constructor(id: number, size: number, rule: string ,creatorName:string, komi : number, name: string,handicap:number,handicapColor: string) {
        this.id = id;

        this.size = size;
        this.rule = rule;
        this.creatorName = creatorName;
        this.komi = komi;
        this.name = name;
        this.handicap = handicap;
        this.handicapColor = handicapColor;

    }

    /**
     * Renvoie l'identifiant de la partie
     */
    public Id(): number {
        return this.id;
    }

    /**
     * Renvoie la taille de la grille de jeu
     */
    public Size(): number {
        return this.size;
    }

    /**
     * Renvoie les règles de la partie
     */
    public Rule(): string {
        return this.rule;
    }

    /**
     * Renvoie le nom du createur de la partie
     */
    public CreatorName(): string {
        return this.creatorName;
    }
    /**
     * Renvoie le titre de la partie
     */
    public Name(): string{
        return this.name;
    }
    /**
     * Renvoie le handicap de la partie
     */
    public Handicap(): number{
        return this.handicap;
    }
    /**
     * Renvoie la couleur de la personne sur qui affecté le handicap
     */
    public HandicapColor(): string{
        return this.handicapColor;
    }
}