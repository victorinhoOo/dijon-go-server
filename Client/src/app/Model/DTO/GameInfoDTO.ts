/**
 * Gère les attributs des informations d'une partie terminée pour le transfert de données entre le serveur et le client
 */
export class GameInfoDTO {
    private id: number;
    private usernamePlayer1: string;
    private usernamePlayer2: string;
    private size: number;
    private rule: string;
    private scorePlayer1: number;
    private scorePlayer2: number;
    private won: boolean;
    private date: Date;

    constructor(id: number, usernamePlayer1: string, usernamePlayer2: string, size: number, rule: string, scorePlayer1: number, scorePlayer2: number, won: boolean, date: Date) {
        this.id = id;
        this.usernamePlayer1 = usernamePlayer1;
        this.usernamePlayer2 = usernamePlayer2;
        this.size = size;
        this.rule = rule;
        this.scorePlayer1 = scorePlayer1;
        this.scorePlayer2 = scorePlayer2;
        this.won = won;
        this.date = date;
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

    /**
     * Renvoie le score du joueur 1
     */
    public ScorePlayer1(): number {
        return this.scorePlayer1;
    }

    /**
     * Renvoie le score du joueur 2
     */
    public ScorePlayer2(): number {
        return this.scorePlayer2;
    }

    /**
     * Indique si le joueur a gagné
     */
    public Won(): boolean {
        return this.won;
    }

    /**
     * Renvoie la date à laquelle la partie a été jouée
     */
    public Date(): Date {
        return this.date;
    }
}