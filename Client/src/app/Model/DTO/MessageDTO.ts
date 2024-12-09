/**
 * Gère les attributs des messages entre deux joueurs
 */
export class MessageDTO {
    private sender: string;
    private receiver: string;
    private content: string;
    private timestamp: Date;

    constructor(sender: string, receiver: string, content: string, timestamp: Date = new Date()) {
        this.sender = sender;
        this.receiver = receiver;
        this.content = content;
        this.timestamp = timestamp;
    }

    public Sender(): string {
        return this.sender;
    }

    public Receiver(): string {
        return this.receiver;
    }

    public Content(): string {
        return this.content;
    }

    public Timestamp(): Date {
        return this.timestamp;
    }
}

