/**
 * Gère les attributs d'un utilisateur connecté
 */
export class User {
    private username: string;
    private email: string;
    private elo: number;

    constructor(username: string, email: string, elo: number) {
        this.username = username;
        this.email = email;
        this.elo = elo;
    }

    public get Username(): string {
        return this.username;
    }

    public get Email(): string {
        return this.email;
    }

    public get Elo(): number {
        return this.elo;
    }

    /**
     * Calcule le rang de l'utilisateur en fonction de son Elo.
     * Les rangs sont de 20 Kyu à 9 Dan.
     */
    public get Rank(): string {
        let rank: string = '';

        // Détermination du rang en fonction de l'Elo à l'aide d'un switch
        switch (true) {
            case (this.elo < 100):
                rank = "20 Kyu";
                break;
            case (this.elo < 200):
                rank = "19 Kyu";
                break;
            case (this.elo < 300):
                rank = "18 Kyu";
                break;
            case (this.elo < 400):
                rank = "17 Kyu";
                break;
            case (this.elo < 500):
                rank = "16 Kyu";
                break;
            case (this.elo < 600):
                rank = "15 Kyu";
                break;
            case (this.elo < 700):
                rank = "14 Kyu";
                break;
            case (this.elo < 800):
                rank = "13 Kyu";
                break;
            case (this.elo < 900):
                rank = "12 Kyu";
                break;
            case (this.elo < 1000):
                rank = "11 Kyu";
                break;
            case (this.elo < 1100):
                rank = "10 Kyu";
                break;
            case (this.elo < 1200):
                rank = "9 Kyu";
                break;
            case (this.elo < 1300):
                rank = "8 Kyu";
                break;
            case (this.elo < 1400):
                rank = "7 Kyu";
                break;
            case (this.elo < 1500):
                rank = "6 Kyu";
                break;
            case (this.elo < 1600):
                rank = "5 Kyu";
                break;
            case (this.elo < 1700):
                rank = "4 Kyu";
                break;
            case (this.elo < 1800):
                rank = "3 Kyu";
                break;
            case (this.elo < 1900):
                rank = "2 Kyu";
                break;
            case (this.elo < 2000):
                rank = "1 Kyu";
                break;
            case (this.elo < 2200):
                rank = "1 Dan";
                break;
            case (this.elo < 2400):
                rank = "2 Dan";
                break;
            case (this.elo < 2600):
                rank = "3 Dan";
                break;
            case (this.elo < 2800):
                rank = "4 Dan";
                break;
            case (this.elo < 3000):
                rank = "5 Dan";
                break;
            case (this.elo < 3200):
                rank = "6 Dan";
                break;
            case (this.elo < 3400):
                rank = "7 Dan";
                break;
            case (this.elo < 3600):
                rank = "8 Dan";
                break;
            default:
                rank = "9 Dan";
                break;
        }

        return rank;
    }
}
