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
        this.MaxLife = 0;
        this.InitialPosition = Utils_1.Vector2.zero;
        this.Id = (0, uuid_1.v4)();
        this.MaxLife = Life;
        this.Life = Life;
        this.Name = Name;
        this.Prefab = Prefab;
        this.RepopDelay = RepopDelay;
    }
    Hit(Damage) {
        if (this.Life <= 0) {
            return;
        }
        console.log(`${this.Name} is hit with ${Damage} damage`);
        this.Life -= Damage;
        if (this.Life <= 0) {
            this.Life = 0;
            if (this.Event) {
                this.Event(EnnemyEventType.Die, this.Id);
            }
            setTimeout(() => {
                this.Repop();
            }, this.RepopDelay * 1000);
        }
        else {
            if (this.Event) {
                this.Event(EnnemyEventType.Hit, this.Id, this.Life);
            }
        }
    }
    Repop() {
        this.Life = this.MaxLife;
        this.Position = this.InitialPosition;
        if (this.Event) {
            this.Event(EnnemyEventType.Repop, this.Id, this.Life, this.Position);
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
    result.InitialPosition = new Utils_1.Vector2(x, y);
    result.Position = result.InitialPosition;
    result.Event = givenFunction;
    return result;
}
exports.getEnnemy = getEnnemy;
