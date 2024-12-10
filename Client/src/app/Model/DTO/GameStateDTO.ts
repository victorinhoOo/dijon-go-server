export class GameStateDTO {
    private board: string;
    private capturedBlack: number;
    private capturedWhite: number;
    private moveNumber: number;

    public constructor(board: string, capturedBlack: number, capturedWhite: number, moveNumber: number)
    {
        this.board = board;
        this.capturedBlack = capturedBlack;
        this.capturedWhite = capturedWhite;
        this.moveNumber = moveNumber;
    }

    public Board():string{
        return this.board;
    }
    public CapturedBlack():number{
        return this.capturedBlack;
    }
    public CapturedWhite():number{
        return this.capturedWhite;
    }
    public MoveNumber():number{
        return this.moveNumber;
    }
}