using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{/// <summary>
/// Sätter denna som abstrakt till att börja med, eventuellt så kanske vi inte behöver det.
/// Men, enligt GRASP principen Information Expert så känns det som att Game (som har all information 
/// om spelet) får hantera matchresultaten.
/// </summary>
    public abstract class Game
    {
        public int Id { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public DateTime GameStart { get; set; }
        public Player Winner { get; set; }

        public enum GameOutcome
        {
            Win = 1,
            Loss = 0
        }
        // Spelaren med högst rating blir playerone.
        // Detta för att RatingCalculator skall fungera korrekt. :)
        public Game(Player one, Player two)
        {
            if (one.Rating > two.Rating)
            {
                PlayerOne = one;
                PlayerTwo = two;
            }
            else
            {
                PlayerOne = two;
                PlayerTwo = one;
            }
        }

        public void UpdateRating()
        {
            int delta;
            RatingCalculator rc = new RatingCalculator();
            if (Winner.Equals(PlayerOne))
            {
                delta = rc.CalculateELO(PlayerOne.Rating, PlayerTwo.Rating, (RatingCalculator.GameOutcome) GameOutcome.Win);
                PlayerOne.Rating += delta;
                PlayerTwo.Rating -= delta;
            }
            else
            {
                delta = rc.CalculateELO(PlayerOne.Rating, PlayerTwo.Rating, (RatingCalculator.GameOutcome)GameOutcome.Loss);
                PlayerOne.Rating += delta;
                PlayerTwo.Rating -= delta;
            }

        }
    }
}
