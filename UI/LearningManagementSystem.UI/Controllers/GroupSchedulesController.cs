using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LearningManagementSystem.UI.Controllers
{
    public class GroupSchedulesController : Controller
    {
        private readonly ILearningManagementSystem _learningManagementSystem;

        public GroupSchedulesController(ILearningManagementSystem learningManagementSystem)
        {
            _learningManagementSystem = learningManagementSystem;
        }

        // GET
        public async Task<IActionResult> Index(Guid groupId)
        {
            // Fetch the schedules based on the DayOfWeek
            var responses = await _learningManagementSystem.GroupScheduleList(new RequestFilter()
            {
                FilterField = "GroupId",
                FilterGuidValue = groupId,
            });
            return Json(responses);
        }
    }
}