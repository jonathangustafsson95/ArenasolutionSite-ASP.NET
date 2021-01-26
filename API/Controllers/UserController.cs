using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Controller for Users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork unitOfWork;

        public UserController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        /// <summary>
        /// This method takes a user and compare username and password to all users in database. If the user exists,
        /// the user from the database is returned. If the user dosen't exist or there is no connection to the database
        /// an error is returned.
        /// </summary>
        /// <param name="user">A user to be Authenticated (baseclass)</param>
        /// <returns>Authenticated user (subclass)</returns>
        [HttpPost]
        [Route("Authenticate")]
        public  ActionResult<User> AuthenticateUser(User user)
        {
            try
            {
                var DBusers = unitOfWork.UserRepository.Get(null, null, "LeagueMemberShips,Leagues");

                foreach (var u in DBusers)
                {
                    if (u.UserName.ToLower().Equals(user.UserName.ToLower()) && u.Password.Equals(user.Password))
                    {
                        return u;
                    }
                }
                return BadRequest("Wrong username/password");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }   
        }
        /// <summary>
        /// This method takes a user of subclass Player and attemts to register it in the database.
        /// If the user already exists or there is no connection to the database an errormessage 
        /// is returned else a message is returned to confirm registration.
        /// </summary>
        /// <param name="player">A user to be registered (subclass: Player)</param>
        /// <returns>Objectresult</returns>
        [HttpPost]
        [Route("Register")]
        public ActionResult<User> RegisterUser(Player player)
        {
            try
            {
                var DBusers = unitOfWork.UserRepository.Get();

                foreach (var u in DBusers)
                {
                    if (u.UserName.ToLower().Equals(player.UserName.ToLower()))
                    {
                        return BadRequest("Username already taken!");
                    }
                }
                unitOfWork.UserRepository.Insert(player);
                unitOfWork.Save();
                return Ok("You have been registered!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        /// <summary>
        /// This method takes a arbitrary searchstring and attempts to match it to the users in the database.
        /// if no users match or the database is unavailable an errormessage is returned. Else a list of
        /// matching users is returned.
        /// </summary>
        /// <param name="searchString">Arbitrary searchstring</param>
        /// <returns>A list of users (baseclass)</returns>
        [HttpGet]
        [Route("SearchUser/{searchString}")]
        public ActionResult<List<User>> SearchUser(string searchString)
        {
            var userList = new List<User>();
            try
            {
                if (searchString == "getallusers")
                {
                    return unitOfWork.UserRepository.Get().ToList();
                }
                userList = unitOfWork.UserRepository.Get().ToList();
                userList = userList.FindAll(x => x.UserName.ToLower().StartsWith(searchString.ToLower()));
                if (userList != null && userList.Count() > 0)
                {
                    return userList;
                }
                return BadRequest("No user with that name!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        /// <summary>
        /// Takes a userId and finds matching user in database. This is used to retrive a spesific user
        /// when clicking profilepage from searchview. If no user is found or there is no connection to
        /// database an error message is returned. Else the matching user is returned.
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>A matching User</returns>
        [HttpGet]
        [Route("ById/{id}")]
        public ActionResult<User> SearchUserById(string id)
        {
            Int32.TryParse(id, out int Id);
            try
            {
                var DBusers = unitOfWork.UserRepository.Get(null, null, "LeagueMemberShips,Leagues");

                foreach (var u in DBusers)
                {
                    if (u.UserId == Id)
                    {
                        return u;
                    }
                }
                return BadRequest("No user with that name!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        /// <summary>
        /// This method takes a dictionary containing userId and userType. It retreives matching
        /// user from database and creates an new user with the new specified subtype but the 
        /// same username/password using Userfactory. The old user is deleted from the database 
        /// and the new user is added. If everything worked as intended the new user is returned.
        /// </summary>
        /// <param name="dict">Dictionary with UserId and UserType</param>
        /// <returns>User(Subclass)</returns>
        [HttpPost]
        [Route("ChangeUserType")]
        public ActionResult<User> ChangeUserType([FromBody]Dictionary<string, string> dict)
        {
            try
            {
                int userId;
                Int32.TryParse(dict["userId"], out userId);

                User userSearched = unitOfWork.UserRepository.GetByID(userId);

                if (userSearched == null)
                {
                    return BadRequest("How did u get here?!");
                }
                else
                {
                    UserFactory userFactory = new UserFactory();
                    var changedUser = userFactory.CreateUser(dict["userTypeString"]);
                    changedUser.Password = userSearched.Password;
                    changedUser.UserName = userSearched.UserName;

                    unitOfWork.UserRepository.Delete(userSearched.UserId);
                    unitOfWork.UserRepository.Insert(changedUser);
                    unitOfWork.Save();

                    changedUser = unitOfWork.UserRepository.Get(x => x.UserName == changedUser.UserName).First();
                    return changedUser;
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        /// <summary>
        /// This method takes a userId and deletes matching user from database.
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>ObjectResult</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult<string> DeleteUser(string id)
        {
            Int32.TryParse(id, out int Id);
            try
            {
                foreach (var leagueMember in unitOfWork.LeagueMemberRepository.Get())
                {
                    if (leagueMember.UserId == Id)
                    {
                        unitOfWork.LeagueMemberRepository.Delete(leagueMember);
                    }
                }
                foreach (var tournamentPlayer in unitOfWork.TournamentPlayerRepository.Get())
                {
                    if (tournamentPlayer.UserId == Id)
                    {
                        unitOfWork.TournamentPlayerRepository.Delete(tournamentPlayer);
                    }
                }
                unitOfWork.UserRepository.Delete(Id);
                unitOfWork.Save();
                return StatusCode(200, "OK");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        /// <summary>
        /// This method takes a userId and updates changed userinformation in Database.
        /// (This has to be fixed)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User(currently baseclass but it should be subclass)</returns>
        [HttpPost]
        [Route("Edit")]
        public ActionResult<User> Edit(User user)
        {
            foreach (var item in unitOfWork.UserRepository.Get())
            {
                if (user.UserName == item.UserName && user.UserId != item.UserId)
                {
                    return BadRequest("That name is already taken");
                }
            }
            User DBuser = unitOfWork.UserRepository.Get(u => u.UserId == user.UserId, null, "LeagueMemberShips,Leagues").First();
            DBuser.UserName = user.UserName;
            DBuser.Password = user.Password;
            unitOfWork.UserRepository.Update(DBuser);
            unitOfWork.Save();
            return DBuser;
        }
    }
}