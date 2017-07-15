// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem
{
    using Safir.Gem.XorGeneticAlgorithm;

    public class Program
    {
        public static void Main(string[] args) {
            var trainer = new GaTrainerXor();
            var bestNN = trainer.Train(5);
            trainer.DoActualRun();
        }
    }
}
