﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.DTO;
using ToDoList.Interfaces;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("/GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Не вдалося ідентифікувати користувача");

            var tasks = _taskService.GetAllTasks(userId);
            return Ok(tasks);
        }

        [HttpGet]
        [Route("/GetTaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskById(id);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetShortInformation")]
        public async Task<IActionResult> GetShortInformation()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Не вдалося ідентифікувати користувача");

            var tasks = await _taskService.GetShortInformation(userId);
            return Ok(tasks);
        }


        [HttpDelete]
        [Route("/DeleteTask/{id}")]
        public async Task<IActionResult> DeteleTask(int id)
        {
            if (id == 0)
            {
                return BadRequest("Помилка запиту");
            }
            
            await _taskService.DeleteTask(id);
            return Ok();
            
        }

        [HttpPost]
        [Route("/CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] AssignmentCreateDTO newTaskDto) {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Не вдалося ідентифікувати користувача");

                var createdTask = await _taskService.CreateTask(newTaskDto, Convert.ToInt32(userId));
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.TaskId }, createdTask);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("/UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] AssignmentUpdateDTO updateDTO)
        {
            try
            {
                var updatedTask = await _taskService.UpdateTask(updateDTO);
                return Ok(updatedTask); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }

        }
    }
}

