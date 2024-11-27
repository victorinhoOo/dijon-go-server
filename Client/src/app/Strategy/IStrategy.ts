import { Game } from '../Model/Game';
export interface IStrategy {
    execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void;
}