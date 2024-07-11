﻿using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class TasksService : ITasksService
{
    private readonly ITasksRepo _tasksRepo;

    public TasksService(ITasksRepo tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }


    public async Task<List<TasksDto?>> GetAllUserTasks(string userId)
    {
        var tasks = await _tasksRepo.GetAllUserTasks(userId);
        return tasks.Select(t => t.TasksMapToTasksDto()).ToList();
    }

    public async Task<TasksDto?> AddTask(Tasks? task, int projectId, string userId)
    {
        try
        {
            var taskEntity = await _tasksRepo.AddTask(task, projectId, userId);
            return taskEntity.TasksMapToTasksDto();
        }
        catch (DbUpdateException ex)
        {
            throw new KeyNotFoundException("The provided key is not valid.");
        }
    }

    public async Task<TasksDto?> GetTaskById(int id)
    {
        try
        {
            var task = await _tasksRepo.GetTaskById(id);

            return task.TasksMapToTasksDto();
        }
        catch (Exception e)
        {
            throw new KeyNotFoundException("The provided key is not valid.");
        }
    }

    public async Task<TasksDto?> UpdateTask(int id, Tasks? task)
    {
        var taskUpdate = await _tasksRepo.UpdateTask(id, task);
        return taskUpdate.TasksMapToTasksDto();
    }

    public async Task<bool> DeleteTask(int id)
    {
        var task = await _tasksRepo.DeleteTask(id);
        return task;
    }
}