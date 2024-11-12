export class Game{
    private currentTurn: string;
    private playerColor: string;

    public constructor(){
        this.currentTurn = ""
        this.playerColor = "";
    }

    public changeTurn():void{
        this.currentTurn = this.currentTurn == "black" ? "white" : "black";
    }

    public getCurrentTurn():string{
        return this.currentTurn;
    }

    public getPlayerColor():string{
        return this.playerColor;
    }

    public initCurrentTurn(){
        this.currentTurn = "black";
    }

    public setPlayerColor(color:string){
        this.playerColor = color;
    }

    public isPlayerTurn():boolean{
        return this.playerColor == this.currentTurn;
    }
}
