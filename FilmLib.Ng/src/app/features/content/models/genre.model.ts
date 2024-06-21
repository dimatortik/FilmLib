import { IGenre } from "../interfaces/genre.interface";

export class GenreModel implements IGenre{
    id: number;
    title: string;
    description: string;

    constructor(data: IGenre){
        this.id = data.id;
        this.title = data.title;
        this.description = data.description;
    }
  }