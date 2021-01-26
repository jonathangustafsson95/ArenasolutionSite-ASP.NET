using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private UnitOfWork unitOfWork;

        public LeagueController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("GetLeagues/{id}")]
        public ActionResult<List<League>> GetLeagues(int id)
        {
            List<League> LeagueList = new List<League>();
            try
            {
                foreach (var item in unitOfWork.LeagueRepository.Get(null, null, "LeagueOwner"))
                {
                    if (item.LeagueOwnerId == id)
                    {
                        LeagueList.Add(item);
                    }
                    else if (id == 0)
                    {
                        LeagueList.Add(item);
                    }
                }
                return LeagueList;
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("CreateLeague")]
        public ActionResult<User> CreateLeague(League league)
        {
            league.LeagueId = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    league.LeagueOwner = unitOfWork.UserRepository.GetByID(league.LeagueOwnerId) as LeagueOwner;
                    unitOfWork.LeagueRepository.Insert(league);
                    unitOfWork.Save();
                    return unitOfWork.UserRepository.Get(u => u.UserId == league.LeagueOwnerId, null, "Leagues").First();
                }
                else
                {
                    return StatusCode(500, "Something went wrong!");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("ApplyForMembership")]
        public ActionResult<User> ApplyForMembership([FromBody]Dictionary<string, string> dict)
        {
            try
            {
                Int32.TryParse(dict["userId"], out int userId);
                string leagueId = dict["leagueId"];
                List<LeagueMember> leagueMemberList = new List<LeagueMember>();

                foreach (var item in unitOfWork.LeagueMemberRepository.Get())
                {
                    if (item.LeagueId == leagueId && item.UserId == userId)
                    {
                        return StatusCode(500, "You already applied!");
                    }
                }
                LeagueMember applicant = new LeagueMember()
                {
                    LeagueId = leagueId,
                    UserId = userId,
                    Applicant = true,
                    League = unitOfWork.LeagueRepository.GetByID(leagueId),
                    Player = (unitOfWork.UserRepository.GetByID(userId) as Player)
                };
                unitOfWork.LeagueMemberRepository.Insert(applicant);
                unitOfWork.Save();
                var Dbusers = unitOfWork.UserRepository.Get(null, null, "LeagueMemberShips,Leagues");
                foreach (var item in Dbusers)
                {
                    if (item.UserId == userId)
                    {
                        return item;
                    }
                }
                return StatusCode(500, "Something went wrong");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpGet]
        [Route("GetLeagueMembers/{id}")]
        public ActionResult<List<LeagueMember>> GetLeagueMembers(string id)
        {
            List<LeagueMember> leagueMembers = new List<LeagueMember>();
            try
            {
                foreach (var item in unitOfWork.LeagueMemberRepository.Get(null, null, "Player"))
                {
                    if (item.LeagueId == id)
                    {
                        leagueMembers.Add(item);
                    }
                }
                return leagueMembers;
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpGet]
        [Route("GetLeague/{id}")]
        public ActionResult<League> GetLeague(string id)
        {
            League league = null;
            try
            {
                league = unitOfWork.LeagueRepository.Get(l => l.LeagueId == id, null, "LeagueOwner").First();
                league.LeagueMembers = unitOfWork.LeagueMemberRepository.Get(lm => lm.LeagueId == id, null, "Player").ToList();
                return league;
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("AcceptMember")]
        public ActionResult<string> AcceptMember([FromBody]Dictionary<string, string> dict)
        {
            Int32.TryParse(dict["userId"], out int userId);
            string leagueId = dict["leagueId"];
            LeagueMember leagueMember = null;

            try
            {
                foreach (var item in unitOfWork.LeagueMemberRepository.Get())
                {
                    if (item.LeagueId == leagueId && item.UserId == userId)
                    {
                        leagueMember = item;
                        leagueMember.Applicant = false;
                        unitOfWork.LeagueMemberRepository.Update(leagueMember);
                        unitOfWork.Save();
                        return StatusCode(200, "OK");
                    }
                }
                return StatusCode(500, "Something went wrong");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("DeleteMember")]
        public ActionResult<User> DeleteMember([FromBody]Dictionary<string, string> dict)
        {
            Int32.TryParse(dict["userId"], out int userId);
            string leagueId = dict["leagueId"];
            LeagueMember leagueMember = null;

            try
            {
                leagueMember = unitOfWork.LeagueMemberRepository.Get(a => a.LeagueId == leagueId && a.UserId == userId).First();
                unitOfWork.LeagueMemberRepository.Delete(leagueMember);
                unitOfWork.Save();
                var Dbusers = unitOfWork.UserRepository.Get(null, null, "LeagueMemberShips,Leagues");
                foreach (var item in Dbusers)
                {
                    if (item.UserId == userId)
                    {
                        return item;
                    }
                }
                return StatusCode(500, "Something went wrong!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("EditLeague")]
        public ActionResult<User> EditLeague(League league)
        {
            try
            {
                unitOfWork.LeagueRepository.Update(league);
                unitOfWork.Save();
                return unitOfWork.UserRepository.Get(u => u.UserId == league.LeagueOwnerId, null, "Leagues").First();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpDelete]
        [Route("DeleteLeague/{id}")]
        public ActionResult<User> DeleteLeague(string id)
        {
            try
            {
                foreach (var item in unitOfWork.LeagueMemberRepository.Get())
                {
                    if (item.LeagueId == id)
                    {
                        unitOfWork.LeagueMemberRepository.Delete(item);
                    }
                }
                League league = unitOfWork.LeagueRepository.GetByID(id);
                unitOfWork.LeagueRepository.Delete(league);
                unitOfWork.Save();
                return unitOfWork.UserRepository.Get(u => u.UserId == league.LeagueOwnerId, null, "Leagues").First();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}