using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechWall.Services;
using TechWall.ViewModels;
using TechWall.Data;
using TechWall.Entities;
using PagedList;
using System.Net;
using PayPal.Api;
using Microsoft.AspNet.Identity.Owin;
using Order = TechWall.Entities.Order;

namespace TechWall.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string sortOrder, string currentFilter, string searchString,string categoryFilter, string MinPrice, string MaxPrice, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceHighToLowSortParm = (String.IsNullOrEmpty(sortOrder) || sortOrder == "PriceLowtoHigh") ? "PriceHightoLow" : "";
            ViewBag.PriceLowToHighSortParm = (String.IsNullOrEmpty(sortOrder) || sortOrder == "PriceHightoLow") ? "PriceLowtoHigh" : "";
            ViewBag.CategoryFilterParm = categoryFilter;

           

            ProductIndexData viewmodel = new ProductIndexData();
            viewmodel.Products = new AppService().GetProducts();
            viewmodel.Categories = new CategoryService().GetCategories();

            if (!String.IsNullOrEmpty(categoryFilter))
            {
                var category = viewmodel.Categories.FirstOrDefault(c => c.Name.Contains(categoryFilter));
                if (category.ParentCategoryID != null)
                {
                    viewmodel.Products = category.Products;
                }
                else
                {
                    var subcategories = new CategoryService().GetSubCategories(category.ID);

                    List<Product> productssub = new List<Product>();
                    foreach (var item in subcategories)
                    {
                        productssub.AddRange(item.Products);
                    }

                    viewmodel.Products = productssub;
                }
            }




            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {

                viewmodel.Products = viewmodel.Products.Where(s => s.Name.ToLower().Contains(searchString.ToLower())
                                       || s.Description.ToLower().Contains(searchString.ToLower())
                                       || s.Category.Name.ToLower().Contains(searchString.ToLower()));
            }

            ViewBag.PriceRangeMinPrice = MinPrice;
            ViewBag.PriceRangeMaxPrice = MaxPrice;

            if (!String.IsNullOrEmpty(MinPrice) && !String.IsNullOrEmpty(MaxPrice))
            {

                viewmodel.Products = viewmodel.Products.Where(s => s.Price >= Convert.ToDecimal(MinPrice) && s.Price <= Convert.ToDecimal(MaxPrice));
            }
            else if (!String.IsNullOrEmpty(MinPrice) && String.IsNullOrEmpty(MaxPrice))
            {

                viewmodel.Products = viewmodel.Products.Where(s => s.Price >= Convert.ToDecimal(MinPrice));
            }
            else if (String.IsNullOrEmpty(MinPrice) && !String.IsNullOrEmpty(MaxPrice))
            {

                viewmodel.Products = viewmodel.Products.Where(s => s.Price <= Convert.ToDecimal(MaxPrice));
            }
            else
            {

                viewmodel.Products = viewmodel.Products;
            }

            switch (sortOrder)
            {
                case "PriceHightoLow":

                    viewmodel.Products = viewmodel.Products.OrderByDescending(s => s.Price);
                    break;
                case "PriceLowtoHigh":

                    viewmodel.Products = viewmodel.Products.OrderBy(s => s.Price);
                    break;
                default:

                    viewmodel.Products = viewmodel.Products.OrderBy(s => s.ID);
                    break;
            }

          

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            viewmodel.Products = viewmodel.Products.ToPagedList(pageNumber, pageSize);
            return View(viewmodel);
        }

        public ActionResult MyAccount()
        {
            TechWallDbContext _Context = new TechWallDbContext();
            ApplicationUser user = new ApplicationUser();
            user = _Context.Users.First(u => u.UserName == User.Identity.Name);
            return View(user);
        }

        public ActionResult WishList()
        {

            return View();
        }

        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TechWallDbContext _Context = new TechWallDbContext();
            Product product = new Product();
            product = _Context.Products.First(u => u.ID == id);


            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Cart()
        {
            var cart = CreateOrGetCart();
            return View(cart);
        }

        public ActionResult Add(int ProductId)
        {
            var product = new AppService().GetOneProduct(ProductId);
            var picture = product.Pictures.FirstOrDefault();
            
            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == product.ID);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem()
                {
                    ProductId = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    pictureUrl=picture.URL
                });
            }

            SaveCart(cart);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int ProductId)
        {
            var product = new AppService().GetOneProduct(ProductId);

            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == product.ID);

            if (existingItem != null)
            {
                cart.CartItems.Remove(existingItem);
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Clear()
        {
            ClearCart();

            return RedirectToAction("Index", "Home");
        }

        private Cart CreateOrGetCart()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                SaveCart(cart);
            }

            return cart;
        }

        private void SaveCart(Cart cart)
        {
            Session["Cart"] = cart;
        }

        private void ClearCart()
        {
            var cart = new Cart();
            SaveCart(cart);
        }

        //PAYPAL

        private TechWallDbContext _dbContext => HttpContext.GetOwinContext().Get<TechWallDbContext>();

        public ActionResult Checkout()
        {
            var cart = CreateOrGetCart();

            if (cart.CartItems.Any())
            {
                // Flat rate shipping
                int shipping = 5;

                var taxRate = 0M;

                var subtotal = cart.CartItems.Sum(x => x.Price * x.Quantity);
                var tax = Convert.ToInt32((subtotal + shipping) * taxRate);
                var total = subtotal + shipping + tax;

                var userId = HttpContext.User.Identity.GetUserId();

                // Create an Order object to store info about the shopping cart
                var order = new Order()
                {
                    OrderDate = DateTime.UtcNow,
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    User = _dbContext.Users.FirstOrDefault(x => x.Id == userId),
                    OrderItems = cart.CartItems.Select(x => new OrderItem()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Quantity = x.Quantity
                    }).ToList()
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                // Get PayPal API Context using configuration from web.config
                var apiContext = GetApiContext();

                // Create a new payment object
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = $"TechWall Shopping Cart Purchase",
                            amount = new Amount
                            {
                                currency = "EUR",
                                total = order.Total.ToString(), // PayPal expects string amounts, eg. "20.00",
                                details = new Details()
                                {
                                    subtotal = order.Subtotal.ToString(),
                                    shipping = order.Shipping.ToString(),
                                    tax = order.Tax.ToString()
                                }
                            },
                            item_list = new ItemList()
                            {
                                items =
                                    order.OrderItems.Select(x => new Item()
                                    {
                                        description = x.Name,
                                        currency = "EUR",
                                        quantity = x.Quantity.ToString(),
                                        price = x.Price.ToString(), // PayPal expects string amounts, eg. "20.00"
                                    }).ToList()
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "Home", null, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "Home", null, Request.Url.Scheme)
                    }
                };

                // Send the payment to PayPal
                var createdPayment = payment.Create(apiContext);

                // Save a reference to the paypal payment
                order.PayPalReference = createdPayment.id;
                _dbContext.SaveChanges();

                // Find the Approval URL to send our user to
                var approvalUrl =
                    createdPayment.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.href);
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            // Fetch the existing order
            var order = _dbContext.Orders.FirstOrDefault(x => x.PayPalReference == paymentId);

            // Get PayPal API Context using configuration from web.config
            var apiContext = GetApiContext();

            // Set the payer for the payment
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            // Identify the payment to execute
            var payment = new Payment()
            {
                id = paymentId
            };

            // Execute the Payment
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            ClearCart();

            return RedirectToAction("Thankyou");
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}