import { IStrategy } from './IStrategy';
import { Game } from '../Model/Game';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';



export class CancelStrategy implements IStrategy {

    private router: Router;

    public constructor(){
        this.router = new Router();
    }
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void
    {
        if(idGame.value != ""){
            idGame.value = "";
            this.router.navigate(['/cancelled']);
        }
    }
}

