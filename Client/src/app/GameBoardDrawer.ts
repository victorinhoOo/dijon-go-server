import { Injectable } from '@angular/core';
import { IGameBoardDrawer } from './IGameBoardDrawer';

export class GameBoardDrawer implements IGameBoardDrawer {
  public drawBoardState(board: string): void {
    let lines = board.split('!');
    const colorMap: { [key: string]: string } = {
      White: 'white',
      Black: 'black',
      Empty: 'transparent',
    };

    for (let i = 1; i < lines.length; i++) {
      let stoneData = lines[i].split(',');
      let x = stoneData[0];
      let y = stoneData[1];
      let color = stoneData[2];
      let stone = document.getElementById(`${x}-${y}`);
      this.discardKo(stone);

      if (colorMap[color]) {
        stone!.style.background = colorMap[color];
      } else if (color === 'Ko') {
        this.drawKo(stone);
      }
    }
  }
  public drawKo(stone: HTMLElement | null): void {
    stone!.style.borderRadius = '0';
    stone!.style.border = '5px solid #A7001E';
    stone!.style.boxSizing = 'border-box';
    stone!.style.background = 'transparent';
  }
  public discardKo(stone: HTMLElement | null): void {
    if (stone != null) {
        stone.style.border = 'none';
        stone.style.borderRadius = '50%';
      }
  }
}
