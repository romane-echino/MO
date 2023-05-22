"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Vector2 = void 0;
class Vector2 {
    constructor(x, y) {
        this.toString = () => {
            return `${this.x}_${this.y}`;
        };
        this.x = x;
        this.y = y;
    }
}
Vector2.zero = new Vector2(0, 0);
exports.Vector2 = Vector2;
