import { Observable } from "./Observable";

/**
 * Interface IObserver
 * 
 * Cette interface définit un observateur qui peut être mis à jour avec un objet Observable.
 */
export interface IObserver {
    /**
     * Met à jour l'observateur avec un objet Observable.
     * 
     * @param object - L'objet Observable qui a changé.
     */
    update(object: Observable): void;
}