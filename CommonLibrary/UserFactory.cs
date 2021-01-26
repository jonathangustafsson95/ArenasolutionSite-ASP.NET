using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    public class UserFactory
    {
        public User CreateUser(String uType)
        {
            switch (uType)
            {
                case "Player":
                    return new Player();
                case "LeagueOwner":
                    LeagueOwner leagueOwner = new LeagueOwner
                    {
                        Leagues = new List<League>()
                    };
                    return leagueOwner;
                case "Operator":
                    return new Operator();
                case "Advertiser":
                    return new Advertiser();
                default:
                    return null;
            }
        }
    }
}
