using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.DomainModel;
using TaskAPI.Messages;

namespace TaskAPI.DomainService.MapProfile
{
    public class TaskMapProfile:Profile
    {
        public TaskMapProfile()
        {
            CreateMap<Tasks, TaskAdd>()
                .ForMember(dest => dest.TaskDescription, options =>
                   options.MapFrom(src => src.TaskDeatails))
                .ForMember(dest => dest.Priority, options =>
                  options.MapFrom(src => src.Priortiy))
                .ForMember(dest => dest.StartDate, options =>
                  options.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.EndDate))
                .ForMember(dest=>dest.TaskId, options=>
                    options.MapFrom(src=>src.TaskId))
                .ReverseMap();
            CreateMap<ParentTask, TaskAdd>()
                .ForMember(dest => dest.ParentTaskId, options =>
                  options.MapFrom(src => src.Parent_Task))
                .ReverseMap();
            CreateMap<Tasks, TaskMod>()
                .ForMember(dest => dest.TaskDescription, options =>
                   options.MapFrom(src => src.TaskDeatails))
                .ForMember(dest => dest.Priority, options =>
                  options.MapFrom(src => src.Priortiy))
                .ForMember(dest => dest.StartDate, options =>
                  options.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.EndDate))
                .ForMember(dest=>dest.TaskId, options=>
                options.MapFrom(src=>src.TaskId))
                .ReverseMap();
            CreateMap<ParentTask, TaskMod>()
                .ForMember(dest => dest.ParentTaskId, options =>
                  options.MapFrom(src => src.Parent_Task))
                .ReverseMap();
            CreateMap<Tasks, TaskListing>()
                .ForMember(dest => dest.TaskId, options =>
                  options.MapFrom(src => src.TaskId))
                .ForMember(dest => dest.TaskDescription, options =>
                  options.MapFrom(src => src.TaskDeatails))
                .ForMember(dest => dest.Priority, options =>
                  options.MapFrom(src => src.Priortiy))
                .ForMember(dest => dest.StartDate, options =>
                  options.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, options =>
                  options.MapFrom(src => src.EndDate))
                .ReverseMap();
            CreateMap<ParentTask, TaskListing>()
                .ForMember(dest => dest.ParentTaskId, options =>
                options.MapFrom(src => src.Parent_Task))
                .ForMember(dest => dest.ParentDescription, options =>
                  options.MapFrom(src => src.ParentTaskDescription))
                .ReverseMap();
            CreateMap<ParentTask, ParentTaskMsg>()
                .ForMember(dest => dest.Parent_ID, options =>
                  options.MapFrom(src => src.Parent_ID))
                .ForMember(dest => dest.ParentTask_ID, options =>
                  options.MapFrom(src => src.Parent_Task))
                .ForMember(dest => dest.Parent_Task_Description, options =>
                  options.MapFrom(src => src.ParentTaskDescription))
                .ReverseMap();

        }
    }
}
