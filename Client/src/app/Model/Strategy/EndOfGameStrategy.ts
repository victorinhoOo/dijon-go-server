import { IStrategy } from './IStrategy';
import { Game } from '../Game';
import { UserCookieService } from '../services/UserCookieService';
import { UserDAO } from '../DAO/UserDAO';
import { HttpClient } from '@angular/common/http';
import { User } from '../User';
import { firstValueFrom } from 'rxjs';

const PLAYER1_SCORE_INDEX = 2;
const PLAYER2_SCORE_INDEX = 3;
const WON_INDEX = 4;

/**
 * Implémente la stratégie de fin de partie
 */
export class EndOfGameStrategy implements IStrategy {

    private userDao: UserDAO;
 

    public constructor(private userCookieService: UserCookieService, private http: HttpClient) {
        this.userDao = new UserDAO(this.http);
    }
    public async execute(data: string[], idGame: {value: string}, game:Game):Promise<void>{
        let won = data[WON_INDEX] == "True";
        idGame.value = "";
        let token = this.userCookieService.getToken();
        let user = await firstValueFrom(this.userDao.GetUser(token));
        game.endGame(data[PLAYER1_SCORE_INDEX], data[PLAYER2_SCORE_INDEX], won, user);
    }
}
