import React, { KeyboardEvent } from "react";
import './index.css';
import axios from "axios";
import { Menu, Popover } from "@headlessui/react";

interface IAppProps {

}

interface IAppState {
    mode: 'draw' | 'drag';
    dragging: boolean;
    painting: boolean;
    map: Map | undefined;
    spriteManifest: SpriteManifest | undefined;
    sprites: { [key: string]: SpriteData }

    currentSprite: SpriteData | null;
}

interface SpriteData {
    key: string;
    base64: string;
    tileIndex: number;
    sheetName: string;
}

interface Map {
    Chunks: {
        [position: string]: {
            Tiles: {
                [position: string]: {
                    SpriteSheet: string;
                    TileType: number;
                }
            }
        }
    }
}



interface SpriteManifest {
    sheets: { [spriteKey: string]: string }
}

export default class App extends React.Component<IAppProps, IAppState>{
    cellSize: number = 128;
    grid: number = 20;

    viewport = React.createRef<HTMLDivElement>();
    canvas = React.createRef<HTMLCanvasElement>();

    mouseX: number = 0;
    mouseY: number = 0;

    constructor(props: IAppProps) {
        super(props);

        this.state = {
            mode: 'draw',
            dragging: false,
            painting:false,
            map: undefined,
            spriteManifest: undefined,
            sprites: {},
            currentSprite: null
        }
    }

    componentDidMount(): void {
        console.log('didmount');
        
        window.addEventListener('keydown', (ev) => {
            //@ts-ignore
            this.keyDown(ev);
        });

        window.addEventListener('keyup', (ev) => {
            console.log('keyup');
            //@ts-ignore
            this.keyUp(ev);
        });

        window.addEventListener('mousedown', (ev) => {
            //@ts-ignore
            this.mouseDown(ev);
        });

        window.addEventListener('mouseup', (ev) => {
            //@ts-ignore
            this.mouseUp(ev);
        });

        window.addEventListener('mousemove', (ev) => {
            //@ts-ignore
            this.mouseMove(ev);
        });

        window.addEventListener('mouseleave', (ev) => {
            this.setState({
                mode: 'draw',
                dragging: false,
                painting:false
            })
        });


        this.getMap();
        this.getManifest();

        setTimeout(() => {
            document.getElementById('0_0')?.scrollIntoView({
                behavior: 'smooth', block: 'center',
                inline: 'center'
            });
        }, 1000);
    }

    async getManifest() {
        let manifest: SpriteManifest = await axios.get('/sprites/manifest.json').then(result => {
            return result.data
        });

        let sprites: { [key: string]: SpriteData } = {};
        for (let sheetKey of Object.keys(manifest.sheets)) {
            let url = manifest.sheets[sheetKey];
            console.log('image!', url);

            let sprite = new Image();
            sprite.src = url;

            sprite.onload = () => {
                console.log('loaded!', url, sprite.width);

                var ctx = this.canvas.current?.getContext('2d');
                var canvasY = 0;
                let index = 0;
                let colSize = Math.floor(sprite.width / this.cellSize);
                let rowSize = Math.floor(sprite.height / this.cellSize);

                for (var col = 0; col < colSize; col++) {
                    for (var row = 0; row < rowSize; row++) {
                        var sourceY = col * this.cellSize;
                        var sourceX = row * this.cellSize;
                        // testing: calc a random position to draw this sprite
                        // on the canvas
                        var canvasX = 0;//Math.random() * 150 + 20;
                        //canvasY += this.cellSize + 5;
                        // drawImage with changing source and canvas x/y positions

                        ctx!.drawImage(sprite,
                            sourceX, sourceY, this.cellSize, this.cellSize,
                            canvasX, canvasY, this.cellSize, this.cellSize
                        );


                        let spriteKey = `${sheetKey}_${index}`;
                        sprites[spriteKey] = {
                            base64: this.canvas.current?.toDataURL() ?? '',
                            key: spriteKey,
                            sheetName: sheetKey,
                            tileIndex: index
                        }
                        //ctx!.getImageData(0, 0, this.cellSize, this.cellSize);

                        ctx?.clearRect(0, 0, this.cellSize, this.cellSize);
                        index++;
                    }
                }
            }
        }

        console.log('sprites', sprites);


        this.setState({ spriteManifest: manifest, sprites: sprites })
    }

    async getMap() {
        let result: Map = await axios.get('https://mo-server.herokuapp.com/map').then(result => {
            return result.data
        })

        this.setState({ map: result })
    }

    keyDown(e: React.KeyboardEvent) {
        e.preventDefault();
        if (e.key === ' ') {
            
            this.setState({ mode: "drag" })
        }
        
    }

    changeCurrentSprite(delta: number) {
        if(this.state.mode !== 'draw')
            return;
             
        if (this.state.currentSprite) {
            let nextTile = this.state.currentSprite.tileIndex + delta
            console.log('changeCurrentSprite', this.state.currentSprite.tileIndex, delta, nextTile);

            if (nextTile >= 0 && nextTile < 64) {
                let key = Object.keys(this.state.sprites)[nextTile];
                console.log('changeCurrentSprite', key);
                this.setState({ currentSprite: this.state.sprites[key] })
            }
        }
        else if (this.state.sprites) {
            let key = Object.keys(this.state.sprites)[0]
            this.setState({ currentSprite: this.state.sprites[key] })
        }
    }

    keyUp(e: React.KeyboardEvent) {
        e.preventDefault();
        if (e.key === ' ') {
            this.setState({ mode: "draw" })
        }
        else if (e.key === 'ArrowUp') {
            this.changeCurrentSprite(-8);
        }
        else if (e.key === 'ArrowDown') {
            this.changeCurrentSprite(8);
        }
        else if (e.key === 'ArrowRight') {
            this.changeCurrentSprite(1);
        }
        else if (e.key === 'ArrowLeft') {
            this.changeCurrentSprite(-1);
        }
    }

    mouseMove(e: React.MouseEvent) {
        if (this.state.dragging) {
            let x = this.viewport.current?.scrollLeft ?? 0;
            let y = this.viewport.current?.scrollTop ?? 0;

            x += (this.mouseX - e.clientX)
            y += (this.mouseY - e.clientY)

            this.viewport.current?.scrollTo({
                left: x,
                top: y
            });

            this.mouseX = e.clientX;
            this.mouseY = e.clientY;
        }
    }

    mouseDown(e: React.MouseEvent) {
        if (this.state.mode === 'drag') {
            this.setState({ dragging: true })
            this.mouseX = e.clientX;
            this.mouseY = e.clientY;
        }
        if (this.state.mode === 'draw') {
            this.setState({ painting: true })
        }
    }

    mouseUp(e: React.MouseEvent) {
        if (this.state.mode === 'drag') {
            this.setState({ dragging: false })
        }
        if (this.state.mode === 'draw') {
            this.setState({ painting: false })
        }
    }

    getCellStyle(index: number): React.CSSProperties {
        let result: React.CSSProperties = {};

        let top = Math.floor(index / this.grid);
        let left = (index - (top * this.grid));

        result.width = `${this.cellSize}px`;
        result.height = `${this.cellSize}px`;
        result.top = `${top * this.cellSize}px`;
        result.left = `${left * this.cellSize}px`;

        result.background = `url("${this.getImage(index)}")`;

        return result;
    }

    getCursor(): string {
        if (this.state.mode === 'draw')
            return 'default';

        if (this.state.mode === 'drag') {
            if (this.state.dragging) {
                return 'grabbing'
            }
            else {
                return 'grab';
            }
        }

        return 'default'
    }

    getIndex(index: number): string {
        let top = Math.floor(index / this.grid);
        let left = (index - (top * this.grid));

        return `${top - (this.grid / 2)}_${left - (this.grid / 2)}`;
    }

    getValue(index: number): { tileIndex: number, sheetName: string } | null {
        if (this.state.map) {
            let mapIndex = this.getIndex(index);
            let tileIndex = this.state.map.Chunks['a1'].Tiles[mapIndex].TileType;
            let sheetName = 'Medow';
            return {
                tileIndex: tileIndex,
                sheetName: sheetName
            }
        }

        return null;
    }

    getImage(index: number): string {
        let tileInfo = this.getValue(index);

        if (tileInfo) {
            return this.state.sprites[`${tileInfo.sheetName}_${tileInfo.tileIndex}`]?.base64;
        }
        return '';
    }

    setSprite(index: number, painting:boolean = false) {
        if(this.state.dragging)
        if(painting && !this.state.painting)
            return;

        if (this.state.map && this.state.currentSprite) {


            let mapIndex = this.getIndex(index);
            let map = this.state.map;

            map.Chunks['a1'].Tiles[mapIndex] = {
                SpriteSheet: this.state.currentSprite.sheetName,
                TileType: this.state.currentSprite.tileIndex
            }

            this.setState({ map: map });
        }

    }

    render(): React.ReactNode {
        return (
            <div className="select-none" style={{ cursor: this.getCursor() }}>


                <canvas ref={this.canvas} width="128" height="128" className="hidden"></canvas>

                <div ref={this.viewport} className="overflow-x-auto overflow-y-auto fixed inset-x-0 bottom-0 top-14" onKeyDown={(e) => { e.preventDefault() }}>
                    {/* Content */}
                    <div className="relative" style={{ width: `${this.grid * this.cellSize}px`, height: `${this.grid * this.cellSize}px` }}>
                        {[...Array(this.grid * this.grid)].map((e, i) => {
                            return (
                                <div key={i} id={this.getIndex(i)} style={this.getCellStyle(i)}
                                    onMouseDown={() => this.setSprite(i)}
                                    onMouseEnter={() => this.setSprite(i, true)}
                                    className="group absolute flex flex-col gap-2 border-l border-b border-slate-300 items-center justify-center">

                                    <div className="group-hover:visible hidden">{this.getIndex(i)}</div>
                                </div>
                            )
                        })}
                    </div>

                </div>


                <div className="bg-slate-300 p-2 flex items-center justify-between relative">
                    <div className="flex gap-2">
                        <Popover>
                            <Popover.Button className="bg-slate-600 px-4 py-2 text-white">Sprites</Popover.Button>

                            <Popover.Panel className="absolute z-10 w-screen max-w-lg left-0 bg-white shadow-md">
                                {({ close }) => (
                                    <div className="grid grid-cols-8 gap-2 p-2">
                                        {Object.keys(this.state.sprites).map((key, index) => {
                                            let same = false;
                                            if (key === this.state.currentSprite?.key) {
                                                same = true;
                                            }
                                            return (
                                                <img
                                                    className={`${same ? 'border-slate-500 border-4' : 'border-2 border-transparent'}`}
                                                    key={index}
                                                    id={key}
                                                    src={this.state.sprites[key].base64}
                                                    onClick={() => { this.setState({ currentSprite: this.state.sprites[key] }); close(); }} />
                                            )
                                        })}
                                    </div>
                                )}
                            </Popover.Panel>
                        </Popover>
                        {this.state.currentSprite &&
                            <img src={this.state.currentSprite.base64} width={40} height={40} />
                        }

                    </div>
                    <button className="bg-slate-600 px-4 py-2 text-white">Publish</button>
                </div>
            </div>
        )
    }
}