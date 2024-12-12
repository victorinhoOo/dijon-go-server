import Swal from 'sweetalert2';
import { User } from './Model/User';

export class GamePopupDisplayer {
    

    /**
     * 
     */
    public displayTimeOutPopup():void {
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

    /**
     * 
     * @param opponent 
     * @returns 
     */
    public displayMatchmakingPopup(opponent:User){
        return Swal.fire({
            title: `Une partie a été trouvée contre ${opponent.Username} (Rank: ${opponent.getRank()})`,
            text: 'Voulez-vous la rejoindre ?',
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonText: 'Oui',
            cancelButtonText: 'Non',
            customClass: {
              confirmButton: 'custom-yes-button',
              cancelButton: 'custom-no-button',
              timerProgressBar: "custom-timer-progress-bar"
              
            },
            timer: 10000,
            timerProgressBar: true
          });
    }

    /**
     * 
     * @param won 
     * @param player1score 
     * @param player2score 
     * @param user 
     * @returns 
     */
    public displayEndGamePopup(won:boolean, player1score:string, player2score:string, user:User){
      return Swal.fire({
        title: won ? 'Victoire ! 🌸' : 'Défaite 👺',
        html: `
        <div class="game-result">
          <p>Score final : ${player1score} - ${player2score}</p>
          <div class="elo-message">
            Rang : ${user.getRank()}
          </div>
        </div>
      `,
        icon: won ? 'success' : 'error',
        confirmButtonText: 'Fermer',
        customClass: {
          confirmButton: 'custom-ok-button',
        },
      });
    }
}