export interface GameRoomEventDispatch<TDispatchContent extends Typed> {
    dispatchKey: string;
    dispatchFactory: () => TDispatchContent;
}

export class GameRoomEventListenerBase {
    constructor(listeningKey: string, onTrigger: (listeningContent: any) => void) {
        this.listeningKey = listeningKey;
        this.onTrigger = onTrigger;
    }

    listeningKey: string;
    onTrigger: (listeningContent: any) => void;
}

export class GameRoomEventListener<TListeningContent extends Typed> extends GameRoomEventListenerBase {
    constructor(listeningKey: string, onTrigger: (TListeningContent: any) => void) {
        super(listeningKey, onTrigger);
        this.onTrigger = onTrigger;
    }

    onTrigger: (listeningContent: TListeningContent) => void;
}

export interface Typed {
    $type: string;
}