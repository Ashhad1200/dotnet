using CURD.Data;
using CURD.Models;
using CURD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CURD.Controllers
{
    public class EmployeController : Controller
    {
        private readonly MVCDbContext mvcDbContext;

        public EmployeController(MVCDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDbContext.Employe.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeViewModels addEmployeRequest)
        {
            var employe = new Employe()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeRequest.Name,
                Email = addEmployeRequest.Email,
                Salary = addEmployeRequest.Salary,
                DateOfBirth = addEmployeRequest.DateOfBirth,
                Department = addEmployeRequest.Department
            };

            await mvcDbContext.Employe.AddAsync(employe);

            await mvcDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDbContext.Employe.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
                return await Task.Run(() => View("View",viewModel)); 
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDbContext.Employe.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mvcDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDbContext.Employe.FindAsync(model.Id);
            if (employee != null)
            {

                mvcDbContext.Employe.Remove(employee); 
                await mvcDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }
    }
}
