import { IStrategy } from './IStrategy';
import Swal from 'sweetalert2';
import { Game } from '../Model/Game';


export class TimeoutStrategy implements IStrategy {
    public execute(data: string[], state: { end: boolean, won: string, player1score: string, player2score: string}, idGame: {value: string}, game:Game):void {
        Swal.close();
      
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
      });

      Toast.fire({
        icon: 'error',
        title: 'Aucun adversaire trouvé'
      });
    }
}
