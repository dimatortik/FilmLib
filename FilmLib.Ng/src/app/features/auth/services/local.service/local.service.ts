import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalService {
  constructor(private client: HttpClient) {}

  public get(name: string): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(name);
    }
    return null;
  }

  public set(name: string, value: string) {
    if (typeof window !== 'undefined') {
      localStorage.setItem(name, value);
    }
  }

  public delete(name: string) {
    if (typeof window !== 'undefined') {
      localStorage.removeItem(name);
    }
  }
}
