import { IObserver } from "./IObserver"

/**
 * Classe abstraite Observable qui gère une liste d'observateurs.
 * Les classes dérivées doivent implémenter la logique de notification.
 */
export abstract class Observable{
    private observers: Array<IObserver>

    public constructor(){
        this.observers = new Array<IObserver>();
    }

    /**
     * Enregistre un nouvel observateur.
     * @param observer L'observateur à enregistrer.
     */
    public register(observer:IObserver){
        this.observers.push(observer);
    }

    /**
     * Notifie tous les observateurs d'un changement.
     * @param object L'objet Observable qui a changé.
     */
    protected notifyChange(object:Observable){  
        this.observers.forEach(obs=>{
            obs.update(object);
        })
    }
}