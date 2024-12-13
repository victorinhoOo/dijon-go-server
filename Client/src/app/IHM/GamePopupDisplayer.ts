import Swal from 'sweetalert2';
import { User } from '../Model/User';
/**
 * Classe permettant d'afficher les popups du jeu
 */
export class GamePopupDisplayer {
    

    /**
     * Affiche une popup de timeout
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
     * Affiche une popu indiquant qu'une partie a été trouvée
     * @param opponent information sur l'adversaire
     * @returns la popup
     */
    public displayMatchmakingPopup(opponent:User){
        return Swal.fire({
            title: `Une partie a été trouvée contre ${opponent.Username} (Rang: ${opponent.getRank()})`,
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
     * Affiche la popup de fin de partie
     * @param won vrai si le joueur a gagné, faux sinon
     * @param player1score score du joueur 1
     * @param player2score score du joueur 2
     * @param user information sur je joueur
     * @returns la popup
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