export class Vector2 {
    x: number;
    y: number;

    constructor(x: number, y: number) {
        this.x = x;
        this.y = y;
    }

    static zero = new Vector2(0, 0);

    public toString = (): string => {
        return `${this.x}_${this.y}`;
    }
}