"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getEnnemy = exports.EnnemyType = exports.EnnemyEventType = exports.Ennemy = void 0;
const uuid_1 = require("uuid");
const Utils_1 = require("./Utils");
class Ennemy {
    constructor(Name, Prefab, Life, RepopDelay) {
        this.Prefab = "";
        this.Name = "";
        this.Id = "";
        this.Position = Utils_1.Vector2.zero;
        this.Life = 100;
        this.RepopDelay = 5;
        this._baseLife = 0;
        this._basePosition = Utils_1.Vector2.zero;
        this.Id = (0, uuid_1.v4)();
        this._baseLife = Life;
        this.Life = Life;
        this.Name = Name;
        this.Prefab = Prefab;
        this.RepopDelay = RepopDelay;
    }
    Hit(Damage) {
        console.log(`${this.Name} is hit with ${Damage} damage`);
        this.Life -= Damage;
        if (this.Life <= 0) {
            this.Life = 0;
            if (this.Event) {
                this.Event(EnnemyEventType.Die);
            }
            setTimeout(() => {
                this.Repop();
            }, this.RepopDelay * 1000);
        }
        else {
            if (this.Event) {
                this.Event(EnnemyEventType.Hit, this.Life);
            }
        }
    }
    Repop() {
        this.Life = this._baseLife;
        this.Position = this._basePosition;
        if (this.Event) {
            this.Event(EnnemyEventType.Repop, this.Life, this.Position);
        }
    }
}
exports.Ennemy = Ennemy;
var EnnemyEventType;
(function (EnnemyEventType) {
    EnnemyEventType["Idle"] = "idle";
    EnnemyEventType["Move"] = "move";
    EnnemyEventType["Hit"] = "hit";
    EnnemyEventType["Die"] = "die";
    EnnemyEventType["Repop"] = "repop";
})(EnnemyEventType = exports.EnnemyEventType || (exports.EnnemyEventType = {}));
const ENNEMIES = {};
var EnnemyType;
(function (EnnemyType) {
    EnnemyType[EnnemyType["Dummy"] = 0] = "Dummy";
})(EnnemyType = exports.EnnemyType || (exports.EnnemyType = {}));
ENNEMIES[EnnemyType.Dummy] = new Ennemy('Manequin', 'Dummy', 100, 5);
function getEnnemy(type, x, y, givenFunction) {
    let source = ENNEMIES[type];
    let result = Object.assign(new Ennemy(source.Name, source.Prefab, source.Life, source.RepopDelay), source);
    result.Position = new Utils_1.Vector2(x, y);
    result.Event = givenFunction;
    return result;
}
exports.getEnnemy = getEnnemy;
