import { IGameBoardDrawer } from './IGameBoardDrawer';

const X_STONE_COORDINATE_POSITION = 0;
const Y_STONE_COORDINATE_POSITION = 1;
const STONE_COLOR_POSITION = 2;

/**
 * Classe qui dessine le plateau de jeu
 */
export class GameBoardDrawer implements IGameBoardDrawer {

  /**
   * <inheritdoc/>
   */
  public drawBoardState(board: string): void {
    let lines = board.split('!');
    const colorMap: { [key: string]: string } = {
      White: 'white',
      Black: 'black',
      Empty: 'transparent',
    };

    for (let i = 1; i < lines.length; i++) {
      let stoneData = lines[i].split(',');
      let x = stoneData[X_STONE_COORDINATE_POSITION];
      let y = stoneData[Y_STONE_COORDINATE_POSITION];
      let color = stoneData[STONE_COLOR_POSITION];
      let stone = document.getElementById(`${x}-${y}`);
      this.discardKo(stone);

      if (colorMap[color]) {
        stone!.style.background = colorMap[color];
      } else if (color === 'Ko') {
        this.drawKo(stone);
      }
    }
  }

  /**
   * <inheritdoc />
   */
  public drawKo(stone: HTMLElement | null): void {
    stone!.style.borderRadius = '0';
    stone!.style.border = '5px solid #A7001E';
    stone!.style.boxSizing = 'border-box';
    stone!.style.background = 'transparent';
  }

  /**
   * <inheritdoc />
   */
  public discardKo(stone: HTMLElement | null): void {
    if (stone != null) {
        stone.style.border = 'none';
        stone.style.borderRadius = '50%';
      }
  }
}
