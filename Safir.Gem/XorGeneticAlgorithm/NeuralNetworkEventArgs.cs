// <copyright file="NeuralNetworkEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem.XorGeneticAlgorithm
{
    using System.Collections.Generic;

    internal class NeuralNetworkEventArgs
    {
        private List<double> _targetOutputs;
        private List<double> _outputs;

        public NeuralNetworkEventArgs(List<double> outputs, List<double> targetOutputs) {
            _outputs = outputs;
            _targetOutputs = targetOutputs;
        }

        public List<double> TargetOutputs => _targetOutputs;

        public List<double> Outputs => _outputs;
    }
}