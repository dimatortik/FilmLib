import { IActor } from '../interfaces/actor.interface';

export class ActorModel implements IActor {
  id: string;
  actorImageLink: string;
  name: string;
  description: string;

  constructor(data: IActor) {
    this.id = data.id;
    this.actorImageLink = data.actorImageLink;
    this.name = data.name;
    this.description = data.description;
  }
}
