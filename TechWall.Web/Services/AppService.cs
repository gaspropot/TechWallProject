
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TechWall.Data;
using TechWall.Entities;
using TechWall.ViewModels;
using TechWall.Hubs;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace TechWall.Services
{
    public class AppService
    {
        //nomizw einai kalytera na valeis to context ston contstructor kai na kaneis tis methodous me using
        //public ProductIndexData GetProducts(string category, decimal? maxprice, string name)
        //{
        //    TechWallDbContext _Context = new TechWallDbContext();

        //    var viewModel = new ProductIndexData();

        //    viewModel.Products = _Context.Products.AsQueryable()
        //        .Include(i => i.Pictures)
        //        .Include(i => i.Category)
        //        .Include(i => i.Brand);


        //    if (category != null)
        //    {
        //        viewModel.Products = _Context.Products.Where(p => p.Category.Name == category);
        //    }
        //    if (maxprice > 0)
        //    {
        //        viewModel.Products = _Context.Products.Where(p => p.Price <= maxprice);
        //    }
        //    if (name != null)
        //    {
        //        viewModel.Products = _Context.Products.Where(p => p.Name.Contains(name));
        //    }

        //    _Context.Database.Connection.Close();

        //    return viewModel;
        //}

        public IQueryable<Product> GetProducts()
        {

            TechWallDbContext _Context = new TechWallDbContext();


            IQueryable<Product> ListOfProducts = _Context.Products;

            return ListOfProducts;
        }

        public Product GetOneProduct(int ProductId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var product = _Context.Products.FirstOrDefault(p => p.ID == ProductId);

            _Context.Database.Connection.Close();
            return product;
        }

        internal string AddUserConnection(Guid ConnectionId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var userId = HttpContext.Current.User.Identity.GetUserId();

            _Context.UserConnections.Add(new UserConnection
            {
                ConnectionId = ConnectionId,
                UserId = userId,
            });
            _Context.SaveChanges();
            _Context.Database.Connection.Close();
            return userId;
        }

        internal string RemoveUserConnection(Guid ConnectionId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            string userId = "0";
            var current = _Context.UserConnections.FirstOrDefault(x => x.ConnectionId == ConnectionId);

            if (current != null)
            {
                userId = current.UserId ?? "0";
                _Context.UserConnections.Remove(current);
                _Context.SaveChanges();
            }

            _Context.Database.Connection.Close();
            return userId;
        }

        internal IList<string> GetUserConnections(string UserId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            return _Context.UserConnections.Where(x => x.UserId == UserId).Select(x => x.ConnectionId.ToString()).ToList();
        }

        public List<UserDTO> GetUsersToChat()
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var userId = HttpContext.Current.User.Identity.GetUserId();

            return _Context.Users
                .Include("UserConnections")
                .Where(x => x.Id != userId)
                .OrderByDescending(x => x.LastMessage)
                .Select(x => new UserDTO
                {
                    UserId = x.Id,
                    Email = x.Email,
                    IsOnline = x.UserConnections.Count > 0,
                }).ToList();
        }

        internal ChatBoxModel GetChatBox(string toUserId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var userId = HttpContext.Current.User.Identity.GetUserId();

            var toUser = _Context.Users.FirstOrDefault(x => x.Id == toUserId);
            var messages = _Context.Messages.Where(x => (x.FromUser == userId && x.ToUser == toUserId) || (x.FromUser == toUserId && x.ToUser == userId))
                .Where(x => x.IsArchived == false)
                .OrderByDescending(x => x.Date)
                .Select(x => new MessageDTO
                {
                    ID = x.Id,
                    Message = x.Message1,
                    Class = x.FromUser == userId ? "right" : "left",
                    Date = x.Date,
                })
                .OrderBy(x => x.ID)
                .ToList();

            _Context.Database.Connection.Close();

            return new ChatBoxModel
            {
                ToUser = ToUserDto(toUser),
                Messages = messages,
            };
        }

        internal ChatBoxModel GetManagerChatBox(string toUserId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var userId = HttpContext.Current.User.Identity.GetUserId();

            var toUser = _Context.Users.FirstOrDefault(x => x.Id == toUserId);

            var messages = _Context.Messages.Where(x => (x.FromUser == userId && x.ToUser == toUserId) || (x.FromUser == toUserId && x.ToUser == userId))
            .OrderByDescending(x => x.Date)
            .Select(x => new MessageDTO
            {
                ID = x.Id,
                Message = x.Message1,
                Class = x.FromUser == userId ? "right" : "left",
                isArchived = x.IsArchived,
                Date = x.Date
            })
            .OrderBy(x => x.ID)
            .ToList();


            _Context.Database.Connection.Close();

            return new ChatBoxModel
            {
                ToUser = ToUserDto(toUser),
                Messages = messages,
            };
        }

        internal bool SendMessage(string toUserId, string message)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            try
            {
                string user_id = HttpContext.Current.User.Identity.GetUserId();

                _Context.Messages.Add(new Message
                {
                    FromUser = user_id,
                    ToUser = toUserId,
                    Message1 = message,
                    Date = DateTime.Now
                });

                _Context.Users.FirstOrDefault(x => x.Id == user_id).LastMessage = DateTime.Now;

                _Context.SaveChanges();
                ChatHub.RecieveMessage(user_id, toUserId, message);

                _Context.Database.Connection.Close();
                return true;
            }
            catch
            {
                _Context.Database.Connection.Close();
                return false;
            }
        }

        public UserDTO ToUserDto(ApplicationUser user)
        {
            return new UserDTO
            {
                UserId = user.Id,
                Email = user.Email,
            };
        }

        internal List<MessageDTO> LoadMessages(string toUserId)
        {
            TechWallDbContext _Context = new TechWallDbContext();

            var userId = HttpContext.Current.User.Identity.GetUserId();
            var messages = _Context.Messages.Where(x => (x.FromUser == userId && x.ToUser == toUserId) || (x.FromUser == toUserId && x.ToUser == userId))
                .Where(x => x.IsArchived == false)
                .OrderByDescending(x => x.Date)
                .Select(x => new MessageDTO
                {
                    ID = x.Id,
                    Message = x.Message1,
                    Class = x.FromUser == userId ? "right" : "left",
                })
                .ToList();

            return messages;
        }
    }
}