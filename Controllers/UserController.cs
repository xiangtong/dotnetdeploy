using System;
using System.Collections.Generic;
// using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Network.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Network.Controllers
{
 public class UserController : Controller
    {
        private NTContext _context;
        public UserController(NTContext context)
        {
            _context = context;
        }
        // GET: /Login and Reg page/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
           int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null){
                int userid=(int)Uid;
                return RedirectToAction("Profile");
            }
            else
            {
                return View();
            }            
        }

        // Post : Register
        [HttpPost]
        [Route("/register")]
        public IActionResult Register(RegisterViewModel regmodel)
        {
            if(ModelState.IsValid)
            {
                List<User> users=_context.users.Where(user => user.Email==regmodel.Email).ToList();
                if(users.Count!=0)
                {
                    ViewBag.emailexist="Email has been registered!";
                    return View("Index");
                }
                else
                {
                    User newuser = new User
                    {
                        FirstName = regmodel.FirstName,
                        LastName = regmodel.LastName,
                        Password = regmodel.Password,
                        Email = regmodel.Email,
                        CreatedAt = DateTime.Now
                    };
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                    _context.Add(newuser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", newuser.UserId);
                    return RedirectToAction("Profile");
                }
            }
            return View("Index");
        }

        // Post: Login
        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginViewModel logmodel)
        {
            if(ModelState.IsValid){
                User curuser= _context.users.SingleOrDefault(user => user.Email == logmodel.LEmail);      
                if(curuser == null)
                {
                    ViewBag.loginemailexist="Email do not exist!";
                    return View("Index");
                }
                else
                {
                    var Hasher = new PasswordHasher<User>();
                    // if(curuser.Password != logmodel.LPassword){
                    if(0 == Hasher.VerifyHashedPassword(curuser, curuser.Password, logmodel.LPassword))
                    {
                        ViewBag.loginpassword="Password error";
                        return View("Index");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", curuser.UserId);
                        return RedirectToAction("Profile");
                    }
                }
            }
            return View("Index");
        }

        //Get: Logout
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // GET: /Profile/
        [HttpGet]
        [Route("/profile")]
        public IActionResult Profile()
        {
           int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null){
                int userid=(int)Uid;
                User user=_context.users
                        .SingleOrDefault(u=>u.UserId==userid);
                List<Connection> connections =_context.connections
                        .Include(c=>c.Friend)
                        .Include(c=>c.Self)
                        .Where(c=>c.SelfId==userid || c.FriendId ==userid).ToList();
                List<Invitation> invitations =_context.invitations
                        .Include(i=>i.Sender)
                        .Where(i =>i.ReceiverId==userid)
                        .ToList();
                ModelBundle bundle= new ModelBundle();
                bundle.user=user;
                bundle.connections=connections;
                bundle.invitations=invitations;
                return View("Profile",bundle);
            }
            else
            {
                return View("Index");
            }            
        }

        // GET: /detail user page/
        [HttpGet]
        [Route("/user/{userid}")]
        public IActionResult UserPage(int userid)
        {
           int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                User user=_context.users
                        .SingleOrDefault(u=>u.UserId==userid);
                return View("User",user);
            }
            return View("Index");
        }
        // GET: /all users page/
        [HttpGet]
        [Route("/allusers")]
        public IActionResult AllUsersPage()
        {
           int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                int userid=(int)Uid;
                List<User> allusers=_context.users
                        .Include(u=>u.Connections)
                        .Include(u=>u.FConnections)
                        .Include(u=>u.Invitations)
                        .Include(u=>u.SInvitations).ToList();
                List<User> users =new List<User>();
                foreach(User user in allusers)
                {
                    int i=0;
                    if(user.UserId==userid)
                    {
                        i=1;
                    }
                    if(i==0)
                    {
                        foreach(Connection c in user.Connections)
                        {
                            if(c.SelfId==userid || c.FriendId==userid)
                            {
                                i=1;
                                break;
                            }
                        }
                    }
                    if(i==0)
                    {
                        foreach(Connection c in user.FConnections)
                        {
                            if(c.SelfId==userid || c.FriendId==userid)
                            {
                                i=1;
                                break;
                            }
                        }
                    }
                    if(i==0)
                    {
                        foreach(Invitation invi in user.Invitations)
                        {
                            if(invi.ReceiverId==userid ||invi.SenderId==userid)
                            {
                                i=1;
                                break;
                            }
                        }
                    }
                    if(i==0)
                    {
                        foreach(Invitation invi in user.SInvitations)
                        {
                            if(invi.ReceiverId==userid ||invi.SenderId==userid)
                            {
                                i=1;
                                break;
                            }
                        }
                    }
                    if(i==0)
                    {
                        users.Add(user);
                    }
                }
                ViewBag.users=users;
                if(TempData["success"]!=null)
                {
                    ViewData["success"]=TempData["success"];
                }
                return View("AllUsers");
            }
            return View("Index");
        }
        // Get: /Send Connect Invitation/
        [HttpGet]
        [Route("/connect/new/{ReceiverId}")]
        public IActionResult NewConnection(int ReceiverId)
        {
          int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                int userid=(int)Uid;
                Invitation newinvitation=new Invitation();
                newinvitation.SenderId=userid;
                newinvitation.ReceiverId=ReceiverId;
                newinvitation.CreatedAt=DateTime.Now;
                _context.Add(newinvitation);
                _context.SaveChanges();
                TempData["success"]=$" You have sent invitation successfully";
                return  RedirectToAction("AllUsersPage"); 
            }
            return View("Index");
        }
        // Get: /Accept Connect Invitation/
        [HttpGet]
        [Route("/connect/accept/{invatationid}")]
        public IActionResult Accept(int invatationid)
        {
          int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                int userid=(int)Uid;
                Invitation invitation=_context.invitations
                        .SingleOrDefault(i=>i.InvitationId==invatationid);
                Connection newconnection =new Connection();
                newconnection.SelfId=userid;
                newconnection.FriendId=invitation.SenderId;
                newconnection.CreatedAt=DateTime.Now;     
                _context.Add(newconnection);
                _context.Remove(invitation);
                _context.SaveChanges();
                return  RedirectToAction("Profile"); 
            }
            return View("Index");
        }

        // Get: /Ignore Connect Invitation/
        [HttpGet]
        [Route("/connect/ignore/{invitationid}")]
        public IActionResult Ignore(int invitationid)
        {
          int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                int userid=(int)Uid;
                Invitation invitation=_context.invitations
                        .SingleOrDefault(i=>i.InvitationId==invitationid);
                _context.Remove(invitation);
                _context.SaveChanges();
                return  RedirectToAction("Profile"); 
            }
            return View("Index");
        }
        // Get: /Delete friend/
        [HttpGet]
        [Route("/connect/delete/{connectionid}")]
        public IActionResult Delete(int connectionid)
        {
          int? Uid = HttpContext.Session.GetInt32("UserId");
            if(Uid!=null)
            {
                // int userid=(int)Uid;
                // Connection connection=_context.connections
                //         .SingleOrDefault(c=> c.UserId==userid && c.FriendId=friendid);
                // if(connection==null)
                // {
                //     connection=_context.connections
                //         .SingleOrDefault(c=> c.FriendId==userid && c.UserId=friendid);
                // }
                Connection connection=_context.connections
                        .SingleOrDefault(c=>c.ConnectionId==connectionid);
                _context.Remove(connection);
                _context.SaveChanges();
                return  RedirectToAction("Profile"); 
            }
            return View("Index");
        }
    }
}
