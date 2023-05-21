import { Ennemy } from "./Ennemy";
import { Player } from "./Player";

export class Map{
    Chunks:{[key:string]:Chunk} = {};


}


export class Chunk{
    Tiles:{[key:string]:Tile} = {}

}


export class Tile{
    Players:Player[] = [];
    Ennemies:Ennemy[] = [];

    TileType:TileType = TileType.GRASS;
}

export enum TileType{
    UNKNOWN = 0,
    GRASS = 1,
    WATER = 2,
}
