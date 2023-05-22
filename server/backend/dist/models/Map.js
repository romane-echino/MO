"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TileType = exports.Tile = exports.Chunk = exports.Terrain = void 0;
class Terrain {
    constructor() {
        this.Chunks = {};
        this.Chunks = {};
    }
    Load(data) {
        this.Chunks['a1'] = {
            Tiles: data
        };
    }
}
exports.Terrain = Terrain;
class Chunk {
    constructor() {
        this.Tiles = {};
    }
}
exports.Chunk = Chunk;
class Tile {
    constructor() {
        this.TileType = TileType.GRASS;
    }
}
exports.Tile = Tile;
var TileType;
(function (TileType) {
    TileType[TileType["UNKNOWN"] = 0] = "UNKNOWN";
    TileType[TileType["GRASS"] = 1] = "GRASS";
    TileType[TileType["WATER"] = 2] = "WATER";
})(TileType = exports.TileType || (exports.TileType = {}));
