using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IDesignation _designation;
        private readonly IDepartment _department;
        private readonly IEmployee _employee;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="designation"></param>
        /// <param name="employee"></param>
        public EmployeeController(IDesignation designation,IDepartment department, IEmployee employee)
        {
            this._employee = employee;
            this._designation = designation;
            this._department = department;
        }

        /// <summary>
        /// employee list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get employee list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetEmployeeList()
        {
            DataTableResultModel<EmployeeModel> resultModel = await _employee.GetEmployeeList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new employee.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddEmployee()
        {
            ViewBag.DesignationList = await _designation.GetDesignationSelectList();
            ViewBag.DepartmentList = await _department.GetDepartmentSelectList();

            return PartialView("_AddEmployee", new EmployeeModel());
        }

        /// <summary>
        /// edit employee.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditEmployee(int employeeId)
        {
            ViewBag.DesignationList = await _designation.GetDesignationSelectList();
            ViewBag.DepartmentList = await _department.GetDepartmentSelectList();

            EmployeeModel employeeModel = await _employee.GetEmployeeById(employeeId);

            return PartialView("_AddEmployee", employeeModel);
        }

        /// <summary>
        /// save employee.
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveEmployee(EmployeeModel employeeModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (employeeModel.EmployeeId > 0)
                {
                    // update record.
                    if (true == await _employee.UpdateEmployee(employeeModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _employee.CreateEmployee(employeeModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete employee by employeeid.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteEmployee(int employeeId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _employee.DeleteEmployee(employeeId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
