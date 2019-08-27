using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using retake.Models;

namespace retake.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(RegWLog newUser)
        {
            UserReg submittedUser = newUser.UserReg;
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == submittedUser.Email))
                {
                    ModelState.AddModelError("UserReg.Email", "Email already in use!");
                    return View("Index");
                };

                if(dbContext.Users.Any(u => u.UserName == submittedUser.UserName))
                {
                    ModelState.AddModelError("UserReg.UserName", "User Name already in use!");
                    return View("Index");
                };

                PasswordHasher<UserReg> Hasher = new PasswordHasher<UserReg>();
                submittedUser.Password = Hasher.HashPassword(submittedUser, submittedUser.Password);
                dbContext.Add(submittedUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", submittedUser.UserId);
                return RedirectToAction("Dashboard", new {id = submittedUser.UserId});
            }
            else
            {

                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(RegWLog LogForm)
        {
            UserLog loggedUser = LogForm.UserLog;
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.UserName == loggedUser.UserName);
                if(userInDb == null)
                {
                    ModelState.AddModelError("UserLog.UserName", "Invalid login");
                    return View("Index");
                }
                var hasher = new PasswordHasher<UserLog>();
                var result = hasher.VerifyHashedPassword(loggedUser, userInDb.Password, loggedUser.Password);
                if (result ==0)
                {
                    ModelState.AddModelError("UserLog.Password", "Invalid Login");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard", new {id = userInDb.UserId});

            }
            else{
                return View("Index");
            }
        }

        [HttpGet("home/{id}")]
        public IActionResult Dashboard(int id)
        {
            int? UserSess = HttpContext.Session.GetInt32("UserId");
            if(UserSess == 0 || UserSess == null || UserSess != id)
            {
                return View("Index");
            }
            var hobbyInfo = dbContext.Hobbies
                            .Include(f => f.Faves)
                            .ToList();

            ViewBag.id = UserSess;
            var expert = dbContext.Hobbies.Where(f => f.Difficulty == "Hard").OrderByDescending(f => f.Faves.Count).FirstOrDefault();
            if(expert == null)
            {
                ViewBag.expert = "None";
            }
            else{
                ViewBag.expert = expert.Name;
            }

            var inter = dbContext.Hobbies.Where(f => f.Difficulty == "Normal").OrderByDescending(f => f.Faves.Count).FirstOrDefault();
            if(inter == null)
            {
                ViewBag.inter = "None";
            }
            else
            {
                ViewBag.inter = inter.Name;
            }

            var novice = dbContext.Hobbies.Where(f => f.Difficulty == "Easy").OrderByDescending(f => f.Faves.Count).FirstOrDefault();
            if(novice == null)
            {
                ViewBag.novice = "None";
            }
            else{
                ViewBag.novice = novice.Name;
            }
            
            return View(hobbyInfo);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            int? UserSess = HttpContext.Session.GetInt32("UserId");
            if(UserSess == 0 || UserSess == null)
            {
                return View("Index");
            }
            ViewBag.id = UserSess;
            return View();
        }
        
        [HttpPost("newHobby")]
        public IActionResult newHobby(Hobby newHobby)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Hobbies.Any(u => u.Name == newHobby.Name))
                {
                    ModelState.AddModelError("Name", "Name already exist!");
                    return View("New");
                };
                int? creatorid = HttpContext.Session.GetInt32("UserId");
                newHobby.UserId = (int)creatorid;
                dbContext.Hobbies.Add(newHobby);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", new {id = creatorid});
            }
            else{
                return View("New");
            }
        }

        [HttpGet("hobby/{id}")]
        public IActionResult HobbyInfo(int id)
        {
            int? UserSess = HttpContext.Session.GetInt32("UserId");
            if(UserSess == 0 || UserSess == null)
            {
                return View("Index");
            }
            var hobbyinfo = dbContext.Hobbies
                            .Include(h => h.Faves)
                            .ThenInclude(h => h.Enthusiast)
                            .FirstOrDefault(h => h.HobbyId == id);
            ViewBag.id = UserSess;
            return View(hobbyinfo);
        }

        [HttpGet("add/{id}")]
        public IActionResult FaveHobby(int id)
        {
            int? UserSess = HttpContext.Session.GetInt32("UserId");
            Hobby joining = dbContext.Hobbies
                            .Include(h => h.Faves)
                            .FirstOrDefault(h => h.HobbyId == id);
            if(joining == null || joining.UserId != UserSess)
            {
                Favorite rsvp = new Favorite();
                rsvp.HobbyId = id;
                rsvp.UserId = (int)UserSess;
                dbContext.Faves.Add(rsvp);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", new {id = UserSess});
            }
            return RedirectToAction("Dashboard", new {id = UserSess});

        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
