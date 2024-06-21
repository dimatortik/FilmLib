import { IMovie } from "../interfaces/movie.interface";
import { IActor } from "../interfaces/actor.interface";
import { IGenre } from "../interfaces/genre.interface";


export class MovieModel implements IMovie {
    id: string;
    titleImageLink: string;
    title: string;
    description: string;
    views: number;
    rating: number;
    year: number;
    country: string;
    director: string;
    filmVideoLink: string;
    actors: IActor[];
    genres: IGenre[];


    constructor(data: any) {
        this.id = data.id;
        this.titleImageLink = data.titleImageLink;
        this.title = data.title;
        this.description = data.description;
        this.views = data.views;
        this.rating = data.rating;
        this.year = data.year;
        this.country = data.country;
        this.director = data.director;
        this.filmVideoLink = data.filmVideoLink;
        this.actors = data.actors;
        this.genres = data.genres;
    }

  }