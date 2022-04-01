using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApp00.BL.Model;

namespace webApp00.Controllers
{
    public class RolesController : Controller
    {
        #region Fields
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        #endregion

        #region Actions
        public IActionResult Index()
        {
            var data = roleManager.Roles;
            return View(data);
        }

        [HttpGet]
        public IActionResult CreateNewRole()
        {
            return View();
        }
        [HttpPost]
        //public IActionResult CreateNewRole(CreateRoleVM model)
        //{
        //    roleManager.CreateAsync()
        //    return View();
        //}
        //public async Task<IActionResult> CreateNewRole(CreateRoleVM model)
        //{
        //    await roleManager.CreateAsync()
        //    return View();
        //}
        public async Task<IActionResult> CreateNewRole(CreateRoleVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //mapping
                    var data = new IdentityRole()
                    {
                        Name = model.RoleName,
                        NormalizedName = model.RoleName.ToUpper()
                    };
                    var result = await roleManager.CreateAsync(data);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        //return View(model);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {

                return View(model);

            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var data = await roleManager.FindByIdAsync(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var role = new IdentityRole()
                    //{
                    //    Id=model.Id,
                    //    Name=model.Name,
                    //    NormalizedName=model.NormalizedName.ToUpper()
                    //};                    

                    var role = await roleManager.FindByIdAsync(model.Id);
                    role.Id = model.Id;
                    role.Name = model.Name;
                    role.NormalizedName = model.NormalizedName;

                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddRemoveUsers(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = await roleManager.FindByIdAsync(RoleId);
            var model = new List<UserInRoleVM>();
            foreach (var user in userManager.Users)
            {
                var userInRole = new UserInRoleVM()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                #region the problem is here(Internal server exception) Delete it and run will be ok
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                #endregion

                model.Add(userInRole);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRemoveUsers(List<UserInRoleVM> model,string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user,role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                {
                    continue;
                }
                if (i < model.Count)
                    continue;
            }
            return RedirectToAction("EditRole",new { id = RoleId });
        }

        #endregion
    }
}

