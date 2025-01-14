import { Component } from '@angular/core';
import { assignment } from '../../data/interfaces/assingment';
import { HomeService } from '../../data/services/home/home.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { createTask } from '../../data/interfaces/createTask';
import { CategoryService } from '../../data/services/category/category.service';
import { category } from '../../data/interfaces/category';
import { AuthService } from '../../data/services/login/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
   router = inject(Router);
  tasks: assignment[] = [];
  categories: category[] = [];
  selectedCategory: string | null = null;
  showForm : boolean = false;
  showCreateForm : boolean = false;
  filteredTasks: assignment[] = []; 
  categorySearch: string = ''; 
  currentPage: number = 1;
  itemsPerPage: number = 5;

  
  constructor(private homeService: HomeService, private categoryService : CategoryService, private authService : AuthService) {
  }


  newTask: createTask = {
    title: '',
    description: '',
    dueDate: new Date(),
    // userId: 0, 
    // categoryId: 0
  };

  selectedTask: assignment = {
    taskId : 0,
    title : '',
    description : '',
    dueDate : new Date(),
    isCompleted: false,
    categoryName : '' 
  };

  onEditTask(task: assignment): void {
    this.selectedTask = { ...task }; 
    this.showForm = true;
    this.loadCategories();
  }

  AddTask(){
    this.showCreateForm = true;
  }

  onSubmit(): void {
    if (this.selectedTask) {
      const index = this.tasks.findIndex(t => t.taskId === this.selectedTask?.taskId);
      if (index !== -1) {
        this.tasks[index] = { ...this.selectedTask }; 
        console.log("--------- " + this.selectedTask.categoryName);
        this.homeService.updateTask(this.selectedTask).subscribe({
          next: (res) => {
            console.log('Tasks updated:', res);
          },
          error: (error) => {
            console.error('Error while fetching tasks:', error);
          }
        })
      }
    }
  }
  
  onGetTasks(): void {
    this.homeService.getTask().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        console.log('Tasks loaded:', this.tasks);
      },
      error: (error) => {
        console.error('Error while fetching tasks:', error);
      }
    });
  }

  ngOnInit(): void{
    this.loadTasks();
    this.loadCategories();

    // this.homeService.getTask().subscribe({
    //   next: (tasks) => {
    //     this.tasks = tasks;
    //     console.log('Tasks loaded:', this.tasks);
    //   },
    //   error: (error) => {
    //     console.error('Error while fetching tasks:', error);
    //   }
    // });
  }

  onTaskComplete(task : assignment) : void{
    this.homeService.updateTask({ taskId: task.taskId, isCompleted: task.isCompleted, title : task.title, description : task.description, dueDate : task.dueDate}).subscribe({
      next: (updatedTask) => {
        console.log(`Task "${updatedTask.title}" updated successfully!`);
      },
      error: (error) => {
        console.error('Error updating task:', error);
      }
    });
  }

 
  onCreateTask(task : createTask){
    this.homeService.createTask({title : task.title, description : task.description, dueDate : task.dueDate}).subscribe({
      next: (res) => {
        console.log(`Task "${res.title}" created successfully!`);
        this.showCreateForm = false;
        this.ngOnInit();
      },
      error: (error) => {
        console.error('Error creating task:', error);
      }
    })
  }

  onDeleteTask(id : number){
    this.homeService.deleteTask(id).subscribe({
      next : (res) => {
        console.log(`Task "${id}" deleted successfully!`);
        this.ngOnInit();
      },
      error: (error) => {
        console.error('Error deleting task:', error);
      }
    })
  }

  loadTasks(): void {
    this.homeService.getTask().subscribe((tasks) => {
      this.tasks = tasks;
      this.filteredTasks = tasks;
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        console.log('Categories loaded:', this.categories);
      },
      error: (error) => {
        console.error('Error fetching categories:', error);
      }
    });
  }

  filterTasks(): void {
    if (this.selectedCategory) {
      this.filteredTasks = this.tasks.filter(task => task.categoryName === this.selectedCategory);
    } else {
      this.filteredTasks = this.tasks;
    }
  }

  filterByCategoryName(): void {
    if (this.categorySearch) {
      const searchQuery = this.categorySearch.toLowerCase();
      this.filteredTasks = this.tasks.filter(task =>
        (task.categoryName && task.categoryName.toLowerCase().includes(searchQuery)) ||
        (task.title && task.title.toLowerCase().includes(searchQuery))
      );
    } else {
      this.filteredTasks = this.tasks;
    }
  }


  get totalPages(): number {
    return Math.ceil(this.filteredTasks.length / this.itemsPerPage);
  }

  getPaginatedTasks(): any[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredTasks.slice(startIndex, endIndex);
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  onLogout(){
    this.authService.logout();
    this.router.navigate(['login'])
  }

}
