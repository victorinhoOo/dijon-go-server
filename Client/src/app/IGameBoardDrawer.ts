/**
 * Interface pour dessiner le plateau de jeu
 */
export interface IGameBoardDrawer {
    
    /**
     * Dessine l'état du plateau de jeu
     * @param board état du plateau de jeu
     */
    drawBoardState(board:string):void;

    /**
     * Dessine le symbole du Ko
     * @param stone emplacer ou ajouter le Ko
     */
    drawKo(stone: HTMLElement | null):void;

    /**
     * Supprime le symbole du Ko
     * @param stone emplacement ou supprimer le Ko
     */
    discardKo(stone: HTMLElement | null): void;
}