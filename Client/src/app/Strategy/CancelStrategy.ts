import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';



export class CancelStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void
    {
        if(idGame.value != ""){
            alert("Redirection menu")
            idGame.value = "";
        }
    }
}
