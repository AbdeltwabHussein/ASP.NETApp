using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApp00.BL.Interface;
using webApp00.BL.Model;
using webApp00.DAL.Entity;
using webApp00.BL.Helper;
using webApp00.Resources;
using Microsoft.Extensions.Localization;

namespace webApp00.Controllers
{
    public class EmpController : Controller
    {
        #region Field
        // tightly coupled
        //DepartmentRep dept ;

        // Loosly coupled
        private readonly IEmployeeRep Empt;
        private readonly IDepartmentRep Dept;
        private readonly ICountryRep country;
        private readonly ICityRep city;
        private readonly IDistrictRep district;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResource> localizer;
        #endregion

        #region ctor

        // Loosly coupled
        public EmpController(IEmployeeRep _empt, IDepartmentRep _dept, ICountryRep _country, ICityRep _city, IDistrictRep _district, IMapper _mapper, IStringLocalizer<SharedResource> localizer)
        {
            Empt = _empt;
            // dept = _dept===this.dept = _dept;
            Dept = _dept;
            country = _country;
            city = _city;
            district = _district;
            mapper = _mapper;
            this.localizer = localizer;
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
                var data = mapper.Map<IEnumerable<EmployeeVM>>(Empt.Get());

                ViewBag.msg = "Success";
                ViewBag.msg = localizer["Success"];


                return View(data);
            }
            else
            {
                var data = mapper.Map<IEnumerable<EmployeeVM>>(Empt.Search(SearchValue));
                return View(data);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName");
            ViewBag.CountryList = new SelectList(country.Get(), "Id", "CountryName");

            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM model)
         {
            try
            {
                if (ModelState.IsValid)
                {
                    #region Photo عملتها بدالة
                    ////Get the full path
                    ////var photoPath= "C:\Users\AbdE.Hussien\source\repos\webApp00\wwwroot\files\Photos\"
                    //// دا ينفع فالمسار بتاع السرفر المحلي لكن بالنسبة لمسار السرفر الحقيقي فدا مينفعش فهنستعمل 
                    //var photoPath = Directory.GetCurrentDirectory() + "/wwwroot/files/Photos";

                    ////Get File Name
                    ////var PhotoName = model.Photourl.FileName;
                    ////var PhotoName = Guid.NewGuid()+ model.Photourl.FileName;
                    //var PhotoName = Guid.NewGuid() + Path.GetFileName(model.Photourl.FileName);

                    ////merge folder path with photo name
                    ////var finalphotopath =photoPath+ PhotoName;
                    //var finalphotopath = Path.Combine(photoPath, PhotoName);

                    ////Save File
                    ////stream>> Data over time
                    //using (var stream = new FileStream(finalphotopath, FileMode.Create))
                    //{
                    //    model.Photourl.CopyTo(stream);
                    //}
                    #endregion

                    #region CV  عملتها بدالة
                    ////Get photo path
                    //var cvPath = Directory.GetCurrentDirectory() + "/wwwroot/files/Docs";
                    ////Get photo Name
                    //var cvName = Guid.NewGuid() + Path.GetFileName(model.CVurl.FileName);
                    ////merge folder path with photo name
                    //var finalCVpath = Path.Combine(cvPath, cvName);
                    ////Save File
                    //using (var stream = new FileStream(finalCVpath, FileMode.Create))
                    //{
                    //    model.CVurl.CopyTo(stream);
                    //}
                    #endregion

                    #region photo and cv with UploadFile function that I made in FileUploader class in the folder Helper in BL
                    var photoname = FileUploader.UploadFile("/files/Photos/", model.Photourl);
                    var cvname = FileUploader.UploadFile("/files/Docs/", model.CVurl);
                    #endregion

                    //mapping
                    var data = mapper.Map<Employee>(model);

                    #region Send Name to DB
                    //Sending Name of Photo
                    data.Photo = photoname;
                    //Sending Name of CV
                    data.CV = cvname;
                    #endregion

                    Empt.Create(data);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName");
                ViewBag.CountryList = new SelectList(country.Get(), "Id", "CountryName");

                return View(model);
            }
        }
        //-----------Edit
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = mapper.Map<EmployeeVM>(Empt.GetById(Id));
            ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName", data.DepartmentId);
            ViewBag.CountryList = new SelectList(country.Get(), "Id", "CountryName");
            ViewBag.CityList = new SelectList(city.Get(), "Id", "CityName");
            ViewBag.DistrictList = new SelectList(district.Get(), "Id", "DistrictName");

            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Employee>(model);
                    Empt.Update(data);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName", model.DepartmentId);
                ViewBag.CountryList = new SelectList(country.Get(), "Id", "CountryName");
                ViewBag.CityList = new SelectList(city.Get(), "Id", "CityName");
                ViewBag.DistrictList = new SelectList(district.Get(), "Id", "DistrictName");

                return View(model);
            }
        }
        //-----------Delete
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = mapper.Map<EmployeeVM>(Empt.GetById(Id));
            ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName", data.DepartmentId);
            return View(data);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id)
        {
            try
            {
                var oldata = Empt.GetById(Id);

                #region Deleting Files of Employees from server عملتها بدالة
                //if (System.IO.File.Exists(Directory.GetCurrentDirectory() + "/wwwroot/files/Photos/" + oldata.Photo))
                //{
                //    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/files/Photos/" + oldata.Photo);
                //}
                //if (System.IO.File.Exists(Directory.GetCurrentDirectory() + "/wwwroot/files/Docs/" + oldata.CV))
                //{
                //    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/files/Docs/" + oldata.CV);
                //}
                #endregion

                FileUploader.RemoveFile("/files/Photos/", oldata.Photo);
                FileUploader.RemoveFile("/files/Docs/", oldata.CV);


                Empt.Delete(oldata);
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
            var data = mapper.Map<EmployeeVM>(Empt.GetById(Id));
            ViewBag.DprtmntList = new SelectList(Dept.Get(), "Id", "DepartmentName", "data.DepartmentId");
            ViewBag.DistrictList = new SelectList(district.Get(), "Id", "DistrictName");


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

        #region Ajax Call
        //Get All city Data By CountryId

        [HttpPost]
        public JsonResult GetCityByCountryId(int cntryid)
        {
            var data = city.Get().Where(x => x.CountryId == cntryid);
            return Json(data);
        }

        //Get All city Data By CountryId

        [HttpPost]
        public JsonResult GetDistrictByCityId(int ctyid)
        {
            var data = district.Get().Where(x => x.CityId == ctyid);
            return Json(data);
        }
        #endregion
    }
}
