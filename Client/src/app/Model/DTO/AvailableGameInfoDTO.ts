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

    constructor(id: number, size: number, rule: string ,creatorName:string, komi : number, name: string,handicap:number) {
        this.id = id;

        this.size = size;
        this.rule = rule;
        this.creatorName = creatorName;
        this.komi = komi;
        this.name = name;
        this.handicap = handicap;

    }

    /**
     * Renvoie l'identifiant de la partie
     */
    public Id(): number {
        return this.id;
    }


    /**
     * Renvoie le nom d'utilisateur du joueur 1
     */
    public UsernamePlayer1(): string {
        return this.usernamePlayer1;
    }

    /**
     * Renvoie le nom d'utilisateur du joueur 2
     */
    public UsernamePlayer2(): string {
        return this.usernamePlayer2;
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

    public CreatorName(): string {
        return this.creatorName;
    }
    public Name(): string{
        return this.name;
    }
    public Handicap(): number{
        return this.handicap;
    }
}