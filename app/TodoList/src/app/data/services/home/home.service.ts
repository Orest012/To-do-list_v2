import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, of} from 'rxjs';
import { assignment } from '../../interfaces/assingment';
import { catchError } from 'rxjs';
import { createTask } from '../../interfaces/createTask';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private apiUrl = 'https://localhost:7287';

  constructor(private http: HttpClient) { }

  getTask(): Observable<assignment[]> {
    return this.http.get<assignment[]>(this.apiUrl + "/GetAllTasks").pipe(
      tap(response => {
        console.log(response)
      }),
      catchError(error => {
        console.error('Error occurred:', error);
        return of([]); 
      })
    );
  }

  updateTask(task: Partial<assignment>) : Observable<assignment>{
    return this.http.patch<assignment>(`${this.apiUrl}/UpdateTask`, task);
  }

  createTask(newTask: createTask) : Observable<createTask>{
    console.log(newTask);
    return this.http.post<createTask>(`${this.apiUrl}/CreateTask`, newTask);
  }

  deleteTask(id: number): Observable<assignment> {
    return this.http.delete<assignment>(`${this.apiUrl}/DeleteTask/${id}`);
  }  
}
