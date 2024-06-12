import { IActor } from '../interfaces/actor.interface';

export class ActorModel implements IActor{
    id: string;
    imageLink: string;
    name: string;
    description: string;

    constructor(data: IActor){
        this.id = data.id;
        this.imageLink = data.imageLink;
        this.name = data.name;
        this.description = data.description;
    }
}