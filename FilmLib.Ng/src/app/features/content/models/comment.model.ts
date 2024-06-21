import { IComment } from '../interfaces/comment.interface';

export class CommentModel {
  id: number;
  body: string;
  createdAt: string;
  filmId: string;
  userName: string;

  constructor(data: IComment) {
    this.id = data.id;
    this.body = data.body;
    this.createdAt = CommentModel.formatLocalTime(new Date(data.createdAt));
    this.filmId = data.filmId;
    this.userName = data.userName;
  }

  static formatLocalTime(date: Date): string {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
    };

    const formattedDate = date
      .toLocaleString('en-US', options)
      .replace(',', '');
    console.log(formattedDate);
    return formattedDate;
  }
}
