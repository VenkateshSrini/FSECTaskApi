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
        [HttpGet(Name ="GetTaskAllCriteria")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TaskListing>> GetTaskAllCriteria([FromBody] SearchMsg searchMsg)
        {
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
        [HttpGet(Name = "GetTaskAnyCriteria")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TaskListing>> GetTaskAnyCriteria([FromBody] SearchMsg searchMsg)
        {
            var taskListings = taskService.GetTaskMatchAny(searchMsg);
            if (taskListings == default)
                return NotFound("No task found");
            else
                return Ok(taskListings);

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
        /// Modifies the 
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

       
    }
}
