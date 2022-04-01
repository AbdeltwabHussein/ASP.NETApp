using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApp00.BL.Interface;
using webApp00.BL.Model;
using webApp00.BL.Repository;
using webApp00.DAL.Entity;

namespace webApp00.Controllers
{
    public class DepController : Controller
    {
        #region Field
        // tightly coupled
        //DepartmentRep dept ;

        // Loosly coupled
        private readonly IDepartmentRep Dept;
        private readonly IMapper mapper;
        #endregion

        #region ctor
        // making instance in ctor
        //public DepController()
        //{
        //    this.Dept = new DepartmentRep();
        //}

        // tightly coupled
        //public DepController(DepartmentRep Deptp)
        //{
        //    this.dept = Deptp;
        //}

        // Loosly coupled
        public DepController(IDepartmentRep _dept, IMapper _mapper)
        {
            // dept = _dept===this.dept = _dept;
            Dept = _dept;
            mapper = _mapper;
        }
        #endregion

        #region---Actions

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Index(string SearchValue = "")
        {

            if (SearchValue == "" || SearchValue == null)
            {
                var data = mapper.Map<IEnumerable<DepartmentVM>>(Dept.Get());
                return View(data);
            }
            else
            {
                var data = mapper.Map<IEnumerable<DepartmentVM>>(Dept.Search(SearchValue));
                return View(data);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentVM mdel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(mdel);
                    Dept.Create(data);
                    return RedirectToAction("Index");
                }
                return View(mdel);
            }
            catch (Exception ex)
            {

                return View(mdel);
            }
        }
     
        
        
        
        
        
        
        //-----------Edit
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = mapper.Map<DepartmentVM>(Dept.GetById(Id));
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(DepartmentVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(model);
                    Dept.Update(data);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {

                return View(model);
            }
        }
        //-----------Delete
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = mapper.Map<DepartmentVM>(Dept.GetById(Id));
            return View(data);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id)
        {
            try
            {
                var oldata = Dept.GetById(Id);
                Dept.Delete(oldata);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var data = mapper.Map<DepartmentVM>(Dept.GetById(Id));

                return View(data);
            }
        }

        //---------Details(retrieve specific data)

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = mapper.Map<DepartmentVM>(Dept.GetById(Id));
            return View(data);
        }

        //[HttpPost]
        //[ActionName("Details")]
        //public IActionResult ConfirmDelete(int Id)
        //{
        //    try
        //    {
        //        Dept.Delete(Id);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        var data = Dept.GetById(Id);

        //        return View(data);
        //    }
        //}

        #endregion

    }
}

