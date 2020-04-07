using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskAPI.DomainService;
using TaskAPI.Messages;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        ITaskService taskService;
        ILogger<TaskController> logger;
        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            this.taskService = taskService;
            this.logger = logger;
        }
        // GET: api/Task
        /// <summary>
        /// Gets all the task that matches the search criteria
        /// </summary>
        /// <param name="searchMsg"> criteria for which the details needs to be fetched</param>
        /// <returns>List of tasks</returns>
        [HttpGet]
        [Route("GetTaskAllCriteria")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TaskListing>> GetTaskAllCriteria(int tId, string desc, int pId,
            int prtyFrm, int prtyTo, string sDt, string eDt)
        {
            var searchMsg = new SearchMsg
            {
                TaskId = tId,
                FromDate = (string.IsNullOrWhiteSpace(sDt)) ? DateTime.MinValue : DateTime.Parse(sDt),
                ToDate = (string.IsNullOrWhiteSpace(eDt)) ? DateTime.MinValue : DateTime.Parse(eDt),
                ParentTaskId = pId,
                PriorityFrom = prtyFrm,
                PriorityTo = prtyTo

            };
            var taskListings = taskService.GetTaskMatchAll(searchMsg);
            if (taskListings == default)
                return NotFound("No task found");
            else
                return Ok(taskListings);

        }
        /// <summary>
        /// Get task that match any search criteria
        /// </summary>
        /// <param name="searchMsg">search parameters</param>
        /// <returns>List of task</returns>
        [HttpGet]
        [Route("GetTaskAnyCriteria")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TaskListing>> GetTaskAnyCriteria(int tId,string desc, int pId, 
            int prtyFrm, int prtyTo, string sDt, string eDt)
        {
            var searchMsg = new SearchMsg
            {
                TaskId = tId,
                TaskDescription =desc,
                FromDate = (string.IsNullOrWhiteSpace(sDt))?DateTime.MinValue:DateTime.Parse(sDt),
                ToDate = (string.IsNullOrWhiteSpace(eDt))? DateTime.MinValue:DateTime.Parse(sDt),
                ParentTaskId = pId,
                PriorityFrom = prtyFrm,
                PriorityTo = prtyTo

            };
            var taskListings = taskService.GetTaskMatchAny(searchMsg);
            if (taskListings == default)
                return NotFound("No task found");
            else
                return Ok(taskListings);

        }
        /// <summary>
        /// Get all the parent task
        /// </summary>
        /// <returns>returns all the parent Tak</returns>
        [HttpGet]
        [Route("GetAllParentTask")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ParentTaskMsg>>>GetAllParent()
        {
            var parentTask = await taskService.GetAllParentTask();
            if (parentTask == default)
                return NotFound("No task found");
            else
                return Ok(parentTask);
        }
        // POST: api/Task
        /// <summary>
        /// Add task with given details
        /// </summary>
        /// <param name="taskAdd">Parameters to add task</param>
        /// <returns>state of operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<bool>> Post([FromBody] TaskAdd taskAdd)
        {
            if (taskAdd==null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await taskService.AddTask(taskAdd);
            if (result)
                return Created($"api/Task/{taskAdd.TaskId}", result);
            else
                return StatusCode(500, "Unable to create task");
        }

        /// <summary>
        /// Modifies the Task
        /// </summary>
        /// <param name="taskMod"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>> Put([FromBody] TaskMod taskMod)
        {
            if (taskMod == null)
            {
                ModelState.AddModelError("ParameterEmpty", "Input parameter are all empty");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await taskService.ModifyTask(taskMod);
            if (result)
                return Accepted(result);
            else
                return StatusCode(500, "Modification failed");
        }
        /// <summary>
        /// Ends a task
        /// </summary>
        /// <param name="taskID">Task that needs to ended</param>
        /// <returns> Success or failure in ending the task</returns>
        [HttpPut]
        [Route("EndTask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<bool>>EndTask(int taskID)
        {
            if (taskID<=0)
            {
                ModelState.AddModelError("ParameterEmpty", "InvalidTask ID");
                return BadRequest(ModelState);
            }
            try
            {
                var result = await taskService.EndTask(taskID);
                if (result)
                    return Accepted(result);
                else
                    return StatusCode(500, "Modification failed");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Get all task 
        /// </summary>
        /// <returns>Lsit of task</returns>
        [HttpGet]
        [Route("GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TaskListing>> GetAllTasks()
        {
            var taskListings = taskService.GetAllTask();
            if (taskListings == default)
                return NotFound("No task found");
            else
                return Ok(taskListings);
        }

    }
}
