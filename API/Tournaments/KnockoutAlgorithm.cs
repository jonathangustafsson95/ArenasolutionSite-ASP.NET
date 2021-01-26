using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;

namespace API.Tournaments
{
    public class KnockoutAlgorithm : TournamentAlgorithm
    {
        public KnockoutAlgorithm(UnitOfWork unitOfWork)
            :base (unitOfWork)
        {
        }
        public override void StartTournament(int tournamentId)
        {
            Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);
            Knockout knockout = BuildTree(tournament.TournamentPlayers);
            tournament.Knockout = knockout;
            unitOfWork.TournamentRepository.Update(tournament);
            unitOfWork.Save();
        }

        private Knockout BuildTree(ICollection<TournamentPlayer> playerList)
        {
            Knockout knockout = new Knockout();

            if (playerList.Count() > 2)
            {
                ICollection<TournamentPlayer> playerListLeft = playerList.Take(playerList.Count / 2).ToList();
                ICollection<TournamentPlayer> playerListRight = playerList.Skip(playerList.Count / 2).ToList();

                knockout.LeftNode = BuildTree(playerListLeft);
                knockout.RightNode = BuildTree(playerListRight);
                knockout.player1 = null;
                knockout.player2 = null;
                if(playerList.Count == 3)
                {
                    knockout.RightNode = null;
                    knockout.player2 = playerListRight.First();
                }
                unitOfWork.KnockoutRepository.Insert(knockout);
                unitOfWork.Save();
                return knockout;
            }
            else
            {
                knockout.player1 = playerList.First();
                knockout.player2 = playerList.Last();
                knockout.LeftNode = null;
                knockout.RightNode = null;
                unitOfWork.KnockoutRepository.Insert(knockout);
                unitOfWork.Save();
                return knockout;
            }
        }

        public override void RecordWinLoss(int tournamentId, int winnerUserId)
        {
            Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);
            Knockout knockout = tournament.Knockout;
            SearchTree_AndAddWinner(knockout, winnerUserId);
            return;
        }

        private void SearchTree_AndAddWinner(Knockout knockout, int winnerUserId)
        {
            if (knockout.player1.UserId == winnerUserId || knockout.player2.UserId == winnerUserId)
                return;
            else if (knockout.LeftNode.player1.UserId == winnerUserId)
                knockout.player1 = knockout.LeftNode.player1;
            else if (knockout.LeftNode.player2.UserId == winnerUserId)
                knockout.player1 = knockout.LeftNode.player2;
            else if (knockout.RightNode.player1.UserId == winnerUserId)
                knockout.player2 = knockout.RightNode.player1;
            else if (knockout.RightNode.player2.UserId == winnerUserId)
                knockout.player2 = knockout.RightNode.player2;
            else
            {
                SearchTree_AndAddWinner(knockout.LeftNode, winnerUserId);
                SearchTree_AndAddWinner(knockout.RightNode, winnerUserId);
            }

            unitOfWork.KnockoutRepository.Update(knockout);
            unitOfWork.Save();
        }

        public override List<Knockout> GetMatches(int tournamentId)
        {
            List<Knockout> matchesList = new List<Knockout>();
            Tournament tournament = unitOfWork.TournamentRepository.GetByID(tournamentId);
            //Fungerar inte att ladda in knockout, inte hunnit fixa
            Knockout knockout = tournament.Knockout;
            Queue<Knockout> queue = new Queue<Knockout>();
            queue.Enqueue(knockout);

            while (queue.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine(queue.Count());
                Knockout current = queue.Dequeue();
                matchesList.Add(current);
                System.Diagnostics.Debug.WriteLine(queue.Count());

                if (current.LeftNode != null)
                {
                    queue.Enqueue(current.LeftNode);
                }
                if (current.RightNode != null)
                {
                    queue.Enqueue(current.RightNode);
                }
            }

            return matchesList;
        }
    }
}
