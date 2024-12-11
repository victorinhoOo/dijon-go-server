import Swal from 'sweetalert2';
import { User } from './Model/User';

export class MatchmakingPopupDisplayer {
    

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
}