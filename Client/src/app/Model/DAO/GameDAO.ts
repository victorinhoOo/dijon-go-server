import { HttpClient } from "@angular/common/http";
import { map, Observable } from "rxjs";
import { GameInfoDTO } from '../DTO/GameInfoDTO';

export class GameDAO {

    /**
     * Constructeur de la classe GameDAO.
     * Initialise HttpClient pour effectuer des requêtes HTTP
     * @param http HttpClient utilisé pour envoyer les requêtes HTTP
     */
    constructor(private http: HttpClient) {}

    /**
     * Récupère la liste des parties disponibles
     * @returns Un Observable qui émet la liste des parties disponibles
     */
    public GetAvailableGames(): Observable<GameInfoDTO[]> {
        return this.http.get<{ games: GameInfoDTO[] }>('https://localhost:7065/Games/Available-games')
            .pipe(
                map(response => response.games) 
            );
    }
}
