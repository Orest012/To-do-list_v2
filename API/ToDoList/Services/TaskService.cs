﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.DTO;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssignmentReadDTO> CreateTask(AssignmentCreateDTO newTask, int userId)
        {

            var newAssignment = new Assignment
            {
                Title = newTask.Title,
                Description = newTask.Description,
                DueDate = newTask.DueDate,
                IsCompleted = false,
                UserId = userId,
                //CategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Assignments.Add(newAssignment);
            await _dbContext.SaveChangesAsync();

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

            return new AssignmentReadDTO
            {
                TaskId = newAssignment.TaskId,
                Title = newAssignment.Title,
                Description = newAssignment.Description,
                IsCompleted = newAssignment.IsCompleted,
                DueDate = newAssignment.DueDate,
                UserName = user.Username, 
                //CategoryId = newAssignment.CategoryId,
                CreatedAt = newAssignment.CreatedAt,
                UpdatedAt = newAssignment.UpdatedAt
            };
        }

        public async Task DeleteTask(int taskId)
        {
            var task = await _dbContext.Assignments.FirstOrDefaultAsync(u => u.TaskId == taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Завдання з ID {taskId} не знайдено.");
            }

            _dbContext.Assignments.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public List<AssignmentReadDTO> GetAllTasks(string id)
        {

            return _dbContext.Assignments.Where(u=> u.UserId == Convert.ToInt32(id))
                .Include(a => a.User) 
                .Select(a => new AssignmentReadDTO
                {
                    TaskId = a.TaskId,
                    Title = a.Title,
                    Description = a.Description,
                    IsCompleted = a.IsCompleted,
                    DueDate = a.DueDate,
                    UserName = a.User.Username,
                    CategoryName = a.CategoryName,
                    //CategoryId = a.CategoryId,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToList();
        }

        public async Task<List<AssignmentSummaryDTO>> GetShortInformation(string id)
        {
            return await _dbContext.Assignments.Where(u => u.UserId == Convert.ToInt32(id)).Select(u => new AssignmentSummaryDTO
            {
                TaskId = u.TaskId,
                Title = u.Title,
                IsCompleted = u.IsCompleted,
                DueDate = u.DueDate
            }).ToListAsync();
        }

        public async Task<AssignmentReadDTO> GetTaskById(int taskId)
        {
            var assignment = await _dbContext.Assignments
                .Include(a => a.User) 
                .FirstOrDefaultAsync(a => a.TaskId == taskId);

            if (assignment == null)
            {
                throw new KeyNotFoundException($"Завдання з ID {taskId} не знайдено.");
            }

            return new AssignmentReadDTO
            {
                TaskId = assignment.TaskId,
                Title = assignment.Title,
                Description = assignment.Description,
                IsCompleted = assignment.IsCompleted,
                DueDate = assignment.DueDate,
                UserName = assignment.User.Username,
               // CategoryId = assignment.CategoryId,
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };
        }

        public async Task<AssignmentReadDTO> UpdateTask(AssignmentUpdateDTO newTask)
        {
            var task = _dbContext.Assignments.FirstOrDefault(u => u.TaskId == newTask.TaskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Завдання з ID {newTask.TaskId} не знайдено.");
            }

            task.Title = newTask.Title;
            task.Description = newTask.Description;
            task.IsCompleted = newTask.IsCompleted;
            task.DueDate = newTask.DueDate;
            task.UpdatedAt = DateTime.UtcNow;
            if (newTask.categoryName != null)
            {
                task.CategoryName = newTask.categoryName;
            }
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == task.UserId);


            _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();

            return (new AssignmentReadDTO
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                UserName = user.Username,
                //CategoryId = task.CategoryId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });
            
        }
    }
}
