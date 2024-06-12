import { IGenre } from "../interfaces/genre.interface";

export class GenreModel implements IGenre{
    id: number;
    name: string;
    description: string;

    constructor(data: IGenre){
        this.id = data.id;
        this.name = data.name;
        this.description = data.description;
    }
  }