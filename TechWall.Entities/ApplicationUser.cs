﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TechWall.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public virtual ProfilePicture Picture { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
        public virtual ICollection<UserConnection> UserConnections { get; set; }
        public Nullable<System.DateTime> LastMessage { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1},{2} {3}", Address, City,State, ZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Picture", this.Picture != null ? this.Picture.URL : string.Empty));
            return userIdentity;
        }


      
    }


   


}
