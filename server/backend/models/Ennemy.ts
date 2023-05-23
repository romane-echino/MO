import { v4 as uuidv4 } from 'uuid';
import { Vector2 } from './Utils';

export class Ennemy {
    Prefab: string = "";
    Name: string = "";
    Id: string = "";

    Position: Vector2 = Vector2.zero;

    Life: number = 100;
    RepopDelay: number = 5;

    Event?: (type: EnnemyEventType, id: string, ...args: any[]) => void;

    MaxLife: number = 0;
    InitialPosition: Vector2 = Vector2.zero;

    constructor(Name: string, Prefab: string, Life: number, RepopDelay: number) {
        this.Id = uuidv4()
        this.MaxLife = Life;
        this.Life = Life;
        this.Name = Name;
        this.Prefab = Prefab;
        this.RepopDelay = RepopDelay;
    }

    public Hit(Damage: number) {
        if(this.Life <= 0){
            return;
        }
        
        console.log(`${this.Name} is hit with ${Damage} damage`)
        this.Life -= Damage;
        if (this.Life <= 0) {
            this.Life = 0;
            if (this.Event) {
                this.Event(
                    EnnemyEventType.Die,
                    this.Id
                )
            }

            setTimeout(() => {
                this.Repop();
            }, this.RepopDelay * 1000);
        }
        else {
            if (this.Event) {
                this.Event(
                    EnnemyEventType.Hit,
                    this.Id,
                    this.Life

                )
            }
        }




    }

    public Repop() {
        this.Life = this.MaxLife;
        this.Position = this.InitialPosition;

        if (this.Event) {
            this.Event(
                EnnemyEventType.Repop,
                this.Id,
                this.Life,
                this.Position
            )
        }
    }

}

export enum EnnemyEventType {
    Idle = 'idle',
    Move = 'move',
    Hit = 'hit',
    Die = 'die',
    Repop = 'repop'
}

const ENNEMIES: { [name: number]: Ennemy } = {};

export enum EnnemyType {
    Dummy
}

ENNEMIES[EnnemyType.Dummy] = new Ennemy(
    'Manequin',
    'Dummy',
    100,
    5
);


export function getEnnemy(type: EnnemyType, x: number, y: number, givenFunction: (type: EnnemyEventType, id: string, ...args: any[]) => void): Ennemy {
    let source = ENNEMIES[type];
    let result: Ennemy = Object.assign(
        new Ennemy(
            source.Name,
            source.Prefab,
            source.Life,
            source.RepopDelay),
        source);

    result.InitialPosition = new Vector2(x, y)
    result.Position = result.InitialPosition;
    result.Event = givenFunction;
    return result;
}



