export interface IGameBoardDrawer {
    
    drawBoardState(board:string):void;

    drawKo(stone: HTMLElement | null):void;

    discardKo(stone: HTMLElement | null): void;
}