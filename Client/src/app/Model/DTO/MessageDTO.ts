/**
 * Gère les attributs des messages entre deux joueurs
 */
export class MessageDTO {
    private sender: string;
    private receiver: string;
    private content: string;
    private timestamp: Date;
    private id: string;

    constructor(sender: string, receiver: string, content: string, timestamp?: Date) {
        this.sender = sender;
        this.receiver = receiver;
        this.content = content;
        if(timestamp){
            this.timestamp = timestamp;
        }else{
            this.timestamp = new Date();
        }
        // Génère un identifiant unique pour chaque message à partir du timestamp, de l'expéditeur, du destinataire et du contenu
        const time = this.timestamp.getTime();
        const roundedTimestamp = Math.floor(time / 1000) * 1000; // Arrondi à la seconde près
        const trimmedTimestamp = roundedTimestamp.toString().slice(0, 10); // Prend seulement les 10 premiers caractères
        this.id = `${trimmedTimestamp}-${sender}-${receiver}-${content}`;
    }

    /**
     * Renvoi l'expéditeur du message
     * @returns expéditeur du message
     */
    public Sender(): string {
        return this.sender;
    }

    /**
     * Renvoi le destinataire du message
     * @returns destinataire du message
     */
    public Receiver(): string {
        return this.receiver;
    }

    /**
     * Renvoi le contenu du message
     * @returns contenu du message
     */
    public Content(): string {
        return this.content;
    }

    /**
     * Renvoi le timestamp du message
     * @returns timestamp du message
     */
    public Timestamp(): Date {
        return this.timestamp;
    }

    /**
     * Renvoi l'identifiant du message (uniquement dans le frontend, permet de distinguer les messages entre eux)
     * @returns identifiant du message
     */
    public Id(): string {
        return this.id;
    }
}

