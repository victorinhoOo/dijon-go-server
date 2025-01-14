/**
 * Gère les attributs d'un utilisateur connecté
 */
export class User {
    private username: string;
    private email: string;
    private elo: number;

    // Attribut privé contenant le dictionnaire des rangs
    private rankDictionary: { maxElo: number; rank: string }[];

    constructor(username: string, email: string, elo: number) {
        this.username = username;
        this.email = email;
        this.elo = elo;

        // Initialisation du dictionnaire des rangs
        this.rankDictionary = [
            { maxElo: 100, rank: "20 Kyu" },
            { maxElo: 200, rank: "19 Kyu" },
            { maxElo: 300, rank: "18 Kyu" },
            { maxElo: 400, rank: "17 Kyu" },
            { maxElo: 500, rank: "16 Kyu" },
            { maxElo: 600, rank: "15 Kyu" },
            { maxElo: 700, rank: "14 Kyu" },
            { maxElo: 800, rank: "13 Kyu" },
            { maxElo: 900, rank: "12 Kyu" },
            { maxElo: 1000, rank: "11 Kyu" },
            { maxElo: 1100, rank: "10 Kyu" },
            { maxElo: 1200, rank: "9 Kyu" },
            { maxElo: 1300, rank: "8 Kyu" },
            { maxElo: 1400, rank: "7 Kyu" },
            { maxElo: 1500, rank: "6 Kyu" },
            { maxElo: 1600, rank: "5 Kyu" },
            { maxElo: 1700, rank: "4 Kyu" },
            { maxElo: 1800, rank: "3 Kyu" },
            { maxElo: 1900, rank: "2 Kyu" },
            { maxElo: 2000, rank: "1 Kyu" },
            { maxElo: 2200, rank: "1 Dan" },
            { maxElo: 2400, rank: "2 Dan" },
            { maxElo: 2600, rank: "3 Dan" },
            { maxElo: 2800, rank: "4 Dan" },
            { maxElo: 3000, rank: "5 Dan" },
            { maxElo: 3200, rank: "6 Dan" },
            { maxElo: 3400, rank: "7 Dan" },
            { maxElo: 3600, rank: "8 Dan" },
        ];
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

    /**
     * Renvoie l'Elo du joueur
     */
    public get Elo(): number {
        return this.elo;
    }
    /**
    * set l'email de l'utilisateur utilise pour les tests
    */
    public set Email(value: string)
    {
        this.email = value;
    }
    /**
     * Renvoie le rang de l'utilisateur à partir de son Elo.
     */
    public getRank(): string {
        let rank = "9 Dan"; // Valeur par défaut si l'Elo est supérieur ou égal à 3600
        let index = 0;

     // parcour le tableau a la recherche du rang du joueur,sort quand le rang est trouvé ou tableau fini
        while (this.elo >= this.rankDictionary[index].maxElo && this.Elo < 3600) 
        {
            index++;
            rank = this.rankDictionary[index].rank; // Mise à jour du rang
        }
    
        return rank;
    }
    
}
