// <copyright file="TrainerEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem.XorGeneticAlgorithm
{
    using System;

    internal class TrainerEventArgs : EventArgs
    {
        private readonly int _trainLoop = 0;

        public TrainerEventArgs(int trainLoop) {
            _trainLoop = trainLoop;
        }

        public int TrainLoop => _trainLoop;
    }
}
