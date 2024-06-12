import { IComment } from "../interfaces/comment.interface";

export class CommentModel implements IComment{
    id: number;
    body: string;
    createdAt: string;
    filmId: string;
    userId: string;

    constructor(data: IComment){
        this.id = data.id;
        this.body = data.body;
        this.createdAt = data.createdAt;
        this.filmId = data.filmId;
        this.userId = data.userId;
    }
  }