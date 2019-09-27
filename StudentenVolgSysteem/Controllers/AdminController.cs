using Microsoft.AspNet.Identity;
using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            List<FrontEndApplicationUser> frontEndApplicationUsers = new List<FrontEndApplicationUser>();
            foreach (var user in UserManager.Users.ToList())
            {
                FrontEndApplicationUser a = new FrontEndApplicationUser(user);

                foreach (var role in user.Roles)
                {
                    a.RoleNames.Add(RoleManager.FindById(role.RoleId).Name);
                }

                frontEndApplicationUsers.Add(a);
            }

            return View(frontEndApplicationUsers);

            //TODO: make front-end-safe user model without pwdHash and securityStamp
        }

        public ActionResult Roles()
        {
            return View(RoleManager.Roles.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public List<System.Web.WebPages.Html.SelectListItem> getUsers()
        {
            List<System.Web.WebPages.Html.SelectListItem> listUsers = new List<System.Web.WebPages.Html.SelectListItem>();
            listUsers.Add(new System.Web.WebPages.Html.SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in UserManager.Users)
            {
                listUsers.Add(new System.Web.WebPages.Html.SelectListItem { Text = item.UserName, Value = item.Id });
            }

            return listUsers;
        }

        [NonAction]
        public List<System.Web.WebPages.Html.SelectListItem> getRoles()
        {
            List<System.Web.WebPages.Html.SelectListItem> listRoles = new List<System.Web.WebPages.Html.SelectListItem>();
            listRoles.Add(new System.Web.WebPages.Html.SelectListItem { Text = "Select", Value = "0" });

            var roles = RoleManager.Roles;

            foreach (var item in roles)
            {
                listRoles.Add(new System.Web.WebPages.Html.SelectListItem { Text = item.Name, Value = item.Id });
            }

            return listRoles;
        }

        [HttpGet]
        public ActionResult AssignRolesToUsersAsync()
        {
            AssignRole assignRoles = new AssignRole();
            assignRoles.UserList = getUsers();
            assignRoles.UserRolesList = getRoles();
            return View(assignRoles);
        }

        public bool UserHasRole(string userId, string roleName)
        {
            var roleNames = UserManager.GetRolesAsync(userId).Result;
            foreach (var name in roleNames)
            {
                if (roleName == name)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> AssignRolesToUsersAsync(AssignRole assignRole)
        {
            if (assignRole.UserRoleName == "0")
            {
                ModelState.AddModelError("RoleName", " select UserRoleName");
            }
            if (assignRole.UserID == "0")
            {
                ModelState.AddModelError("UserName", " select Username");
            }

            if (ModelState.IsValid)
            {
                if (UserHasRole(assignRole.UserID, assignRole.UserRoleName))
                {
                    ViewBag.ResultMessage = "Current user already has that role";
                }
                else
                {
                    var role = RoleManager.FindById(assignRole.UserRoleName);
                    IdentityResult result = await UserManager.AddToRoleAsync(assignRole.UserID, role.Name);
                    if (result.Succeeded)
                    {
                        ViewBag.ResultMessage = "User added to role successfully!";
                    }
                    else
                    {
                        ViewBag.ResultMessage = "User was not added to role!";
                    }
                }
                assignRole.UserRolesList = getRoles();
                assignRole.UserList = getUsers();
            }
            else
            {
                assignRole.UserRolesList = getRoles();
                assignRole.UserList = getUsers();
            }
            return View(assignRole);
        }

        public void ResetPasswordForUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
