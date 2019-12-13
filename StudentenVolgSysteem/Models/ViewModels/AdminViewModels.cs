using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace StudentenVolgSysteem.Models
{
    [NotMapped]
    public class UserRolesModel
    {
        public UserRolesModel()
        {

        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> UserRolesList { get; set; }
        public string[] RoleNames { get; set; }
        public List<IdentityRole> CurrentRoles { get; set; }
    }

    [NotMapped]
    public class AssignRole
    {
        [Required(ErrorMessage = "Select proper role")]
        public string UserRoleName { get; set; }
        [Required(ErrorMessage = "Select username")]
        public string UserID { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> UserRolesList { get; set; }
    }
}