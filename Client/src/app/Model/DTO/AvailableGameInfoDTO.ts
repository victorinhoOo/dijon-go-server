/**
 * Gère les attributs des informations de partie qu'il est possible de rejoindre (DTO = Data Transfer Object) sert d'objet de transfert de 
 * données entre le serveur et le client
 */
export class AvailableGameInfoDTO {
    private id: number;
    private title: string;
    private size: number;
    private rule: string;

    constructor(id: number, title: string, size: number, rule: string) {
        this.id = id;
        this.title = title;
        this.size = size;
        this.rule = rule;
    }

    /**
     * Renvoie l'identifiant de la partie
     */
    public Id(): number {
        return this.id;
    }
    /**
     * Renvoie le titre de la partie
     */
    public Title(): string {
        return this.title;
    }

    /**
     * Renvoie la taille de la grille de jeu
     */
    public Size(): number {
        return this.size;
    }

    public Rule(): string {
        return this.rule;
    }
}