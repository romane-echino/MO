import { Ennemy } from "./Ennemy";
import { Player } from "./Player";

export class Terrain{
    Chunks:{[key:string]:Chunk} = {};

    constructor(){
        this.Chunks = {};
    }

    Load(data:{[key:string]:Tile}){
        this.Chunks['a1'] = {
            Tiles:data
        }
    }
}


export class Chunk{
    Tiles:{[key:string]:Tile} = {}

}


export class Tile{
    //Players:Player[] = [];
    //Ennemies:Ennemy[] = [];

    TileType:TileType = TileType.GRASS;
}

export enum TileType{
    UNKNOWN = 0,
    GRASS = 1,
    WATER = 2,
}
