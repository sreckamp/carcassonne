﻿namespace Carcassonne.Model.Expansions
{
    /// <summary>
    /// Claim Grass regions 3 points per closed city that is touched
    /// </summary>
    public class FarmerExpansion:ExpansionPack
    {
        public static readonly ExpansionPack Instance = new FarmerExpansion();

        private FarmerExpansion()
        {
        }
    }
}
