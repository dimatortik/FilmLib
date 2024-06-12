import {MovieModel} from '../../features/content/models/movie.model';

export class PaginationModel<T> {

    items: Array<MovieModel>;
    page: number;
    pageSize: number;
    totalCount: number;
    hasNext: boolean;
    hasPrevious: boolean;


    constructor(data: any) {
        this.items = data.items.map((item: any) => new MovieModel(item)); 
        this.page = data.page;
        this.pageSize = data.pageSize;
        this.totalCount = data.totalCount;
        this.hasNext = data.hasNext;
        this.hasPrevious = data.hasPrevious;
    }
}