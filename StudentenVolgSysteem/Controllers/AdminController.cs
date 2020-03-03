using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.DAL;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;


namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        SVSContext db = new SVSContext();

        #region Actions

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Users
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

        // GET: Admin/Roles
        public ActionResult Roles()
        {
            return View(RoleManager.Roles.ToList());
        }

        #region Unused code

        // GET: Admin/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Admin/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Admin/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Admin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        #endregion

        // GET: Admin/AssignRolesToUsersAsync
        [HttpGet]
        public ActionResult AssignRolesToUsersAsync()
        {
            AssignRole assignRoles = new AssignRole();
            assignRoles.UserList = getUsers();
            assignRoles.UserRolesList = getRoles();
            return View(assignRoles);
        }

        // POST: Admin/AssignRolesToUserAsync
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                
            }
            
            assignRole.UserRolesList = getRoles();
            assignRole.UserList = getUsers();
            
            return View(assignRole);
        }

        // TODO ???
        public void ResetPasswordForUser(int id)
        {
            throw new NotImplementedException();
        }

        // GET: Admin/EditRolesForUser
        
        [HttpGet]
        public ActionResult EditRolesForUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRolesModel urm = new UserRolesModel();
            var user = UserManager.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            urm.UserId = id;
            urm.UserName = user.UserName;
            urm.UserRolesList = RoleManager.Roles.ToList();
            IList<string> userRoleNames = UserManager.GetRoles(urm.UserId);
            urm.CurrentRoles = new List<IdentityRole>();
            // Add identity roles to a new list and return view
            foreach (var role in RoleManager.Roles)
            {
                foreach (string userRoleName in userRoleNames)
                {
                    if (role.Name == userRoleName)
                    {
                        urm.CurrentRoles.Add(role);
                    }
                }
            }
            return View(urm);
        }

        /// <summary>
        /// Post for editting user roles
        /// </summary>
        /// <param name="urm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRolesForUser([Bind(Include = "UserId,UserName,UserRolesList,RoleNames")] UserRolesModel urm)
        {
            if (ModelState.IsValid)
            {
                List<IdentityRole> roles = RoleManager.Roles.ToList();
                foreach (var role in roles)
                {
                    //If the user already has the role
                    if (UserManager.IsInRole(urm.UserId, role.Name))
                    {
                        //but it is not in the list of selected roles
                        try
                        {
                            if (!urm.RoleNames.Contains(role.Name))
                            {
                                //remove the user from role
                                UserManager.RemoveFromRole(urm.UserId, role.Name);
                            }
                        }
                        catch (ArgumentNullException e)
                        {
                            UserManager.RemoveFromRole(urm.UserId, role.Name);
                        }
                        //if it is in the list of selected roles, do nothing
                    }
                    //If the user doesn't already have the role
                    else
                    {
                        //But the role is in the list of selected roles
                        try
                        {
                            if (urm.RoleNames.Contains(role.Name))
                            {
                                //Add the user to role
                                UserManager.AddToRole(urm.UserId, role.Name);
                            }
                        }
                        catch (ArgumentNullException e)
                        {

                        }
                        //If it's not in the ilst of selected roles, do nothing
                    }
                }
                return RedirectToAction("Users");
            }

            urm.UserRolesList = RoleManager.Roles.ToList();
            IList<string> userRoleNames = UserManager.GetRoles(urm.UserId);
            foreach (var role in RoleManager.Roles)
            {
                foreach (string userRoleName in userRoleNames)
                {
                    if (role.Name == userRoleName)
                    {
                        urm.CurrentRoles.Add(role);
                    }
                }
            }
            return View(urm);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns list of users
        /// </summary>
        /// <returns>User List</returns>
        [NonAction]
        public List<SelectListItem> getUsers()
        {
            List<SelectListItem> listUsers = new List<SelectListItem>();
            listUsers.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in UserManager.Users)
            {
                listUsers.Add(new SelectListItem { Text = item.UserName, Value = item.Id });
            }

            return listUsers;
        } 

        /// <summary>
        /// Returns a list of roles
        /// </summary>
        /// <returns>Role List</returns>
        [NonAction]
        public List<SelectListItem> getRoles()
        {
            List<SelectListItem> listRoles = new List<SelectListItem>();
            listRoles.Add(new SelectListItem { Text = "Select", Value = "0" });

            var roles = RoleManager.Roles;

            foreach (var item in roles)
            {
                listRoles.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }

            return listRoles;
        }


        /// <summary>
        /// Check if user has role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns>bool</returns>
        [NonAction]
        public bool UserHasRole(string userId, string roleName)
        {
            var roleNames = UserManager.GetRoles(userId);
            foreach (var name in roleNames)
            {
                if (roleName == name)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }



}
