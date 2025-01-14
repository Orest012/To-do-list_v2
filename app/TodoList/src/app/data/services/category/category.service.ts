import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, of} from 'rxjs';
import { assignment } from '../../interfaces/assingment';
import { catchError } from 'rxjs';
import { createTask } from '../../interfaces/createTask';
import { category } from '../../interfaces/category';


@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7287';

  constructor(private http: HttpClient) { }
  getCategories() : Observable<category[]> {
    return this.http.get<category[]>(`${this.apiUrl}/GetAllCategories`).pipe(
      tap(response => {
        console.log(response)
      }),
      catchError(error => {
        console.error('Error occurred:', error);
        return of([]); 
      }));
  }
}

