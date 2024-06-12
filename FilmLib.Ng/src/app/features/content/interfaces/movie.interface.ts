import { IActor } from "./actor.interface";
import { IGenre } from "./genre.interface";

export interface IMovie {
    id: string;
    titleImageLink: string;
    title: string;
    description?: string;
    views: number;
    rating: number;
    year?: number;
    country?: string;
    director?: string;
    videoLink: string;
    actors?: Array<IActor>;
    genres?: Array<IGenre>;
  }