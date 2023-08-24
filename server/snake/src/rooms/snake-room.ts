import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

const MAP_HEIGHT = 200;
const MAP_WIDTH = 200;

export class PlayerNO extends Schema {
    @type("number") x = Math.floor(Math.random() * MAP_WIDTH - MAP_WIDTH / 2);
    @type("number") z = Math.floor(Math.random() * MAP_HEIGHT - MAP_HEIGHT / 2);
    @type("uint8") d = 2;
    @type("string") c = "FFFFFF";
}

export class StateNO extends Schema {
    @type({ map: PlayerNO })
    players = new MapSchema<PlayerNO>();

    createPlayer(sessionId: string, data: any) {
        const player = new PlayerNO();
        player.c = data.c;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        const player = this.players.get(sessionId);
        player.x = movement.x;
        player.z = movement.z;
    }
}

export class SnakeRoom extends Room<StateNO> {
    maxClients = 4;

    onCreate (options) {
        console.log("SnakeRoom created!", options);

        this.setState(new StateNO());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });
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
