import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

const MAP_HEIGHT = 200;
const MAP_WIDTH = 200;
const MAX_SNAKE_SEGMENTS = 255;

export class Vector2NO extends Schema {
    @type("number") x = Vector2NO.getRandomPosition(MAP_HEIGHT);
    @type("number") z = Vector2NO.getRandomPosition(MAP_WIDTH);

    static getRandomPosition(value)
    {
        return Math.floor(Math.random() * value - value / 2);
    }
}

export class AppleNO extends Vector2NO {}

export class PlayerNO extends Vector2NO {
    @type("string") name;
    @type("uint8") size = 0;
    @type("string") color = "FFFFFF";
    @type("uint16") score = 0;

    constructor (name: string, color: string) {
        super();
        this.name = name;
        this.color = color;
    }
}

export class StateNO extends Schema {
    @type({ map: PlayerNO }) players = new MapSchema<PlayerNO>();
    @type({ map: AppleNO }) apples = new MapSchema<AppleNO>();
    lastAppleId = 0;

    createPlayer(playerId: string, data: any) {
        const player = new PlayerNO(data.name, data.color);
        this.players.set(playerId, player);
    }

    removePlayer(playerId: string) {
        if (this.players.has(playerId))
        {
            this.players.delete(playerId);
        }
    }

    movePlayer (playerId: string, movement: any) {
        const player = this.players.get(playerId);
        player.x = movement.x;
        player.z = movement.z;
    }

    createApple()
    {
        const appleId = (this.lastAppleId++).toString();
        this.apples.set(appleId, new AppleNO());
    }

    createAppleAtPosition(position)
    {
        const appleId = (this.lastAppleId++).toString();
        const apple = new AppleNO();
        apple.x = position.x;
        apple.z = position.z;
        this.apples.set(appleId, apple);
    }

    destroyedPlayers = [];

    createApplesOnDestroy(playerId, positions)
    {
        if (this.destroyedPlayers.includes(playerId)) return;
        this.destroyedPlayers.push(playerId);

        positions.forEach(position => {
            this.createAppleAtPosition(position);
        });
    }

    collectApple(playerId: string, appleId: string)
    {
        const player = this.players.get(playerId);
        if (player === undefined) return;

        player.score++;
        player.size = Math.min(Math.floor(player.score / 3), MAX_SNAKE_SEGMENTS);
        
        const apple = this.apples.get(appleId);
        apple.x = Math.floor(Math.random() * MAP_WIDTH - MAP_WIDTH / 2);
        apple.z = Math.floor(Math.random() * MAP_WIDTH - MAP_WIDTH / 2);
    }
}

export class SnakeRoom extends Room<StateNO> {
    maxClients = 4;
    initialApplesCount = 100;

    onCreate (options) {
        console.log("SnakeRoom created!", options);

        this.setState(new StateNO());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("collect", (client, data) => {
            this.state.collectApple(client.sessionId, data.id);
        });

        this.onMessage("death", (client, data) => {
            this.state.removePlayer(client.sessionId);
        });

        this.onMessage("destroy", (client, dataJson) => {
            const data = JSON.parse(dataJson);
            this.state.createApplesOnDestroy(data.playerId, data.positions);
        });

        for (let index = 0; index < this.initialApplesCount; index++) {
            this.state.createApple();
        }
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, options: any) {
        this.state.createPlayer(client.sessionId, options);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
