using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary
{
    public class RatingCalculator
    {

        public enum GameOutcome
        {
            Win = 1,
            Loss = 0
        }

        /// <summary>
        /// returnerar en decimal som är sannolikheten att playerOne skall vinna matchen.
        /// p1: 1500, p2: 1500 = 0.5 (50%) chans att p1 vinner.
        /// </summary>
        /// <param name="playerOneRating"></param>
        /// <param name="playerTwoRating"></param>
        /// <returns></returns>
        private double ExpectationToWin(int playerOneRating, int playerTwoRating)
        {
            return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
        }

        public int CalculateELO(int playerOneRating, int playerTwoRating, GameOutcome outcome)
        {
            //viktning
            int eloK = 32;

            return (int)(eloK * ((int)outcome - ExpectationToWin(playerOneRating, playerTwoRating)));

            
        }

    }
}
