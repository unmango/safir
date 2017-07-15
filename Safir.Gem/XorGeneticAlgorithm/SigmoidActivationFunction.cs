// <copyright file="SigmoidActivationFunction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem.XorGeneticAlgorithm
{
    using System;

    internal class SigmoidActivationFunction
    {
        public static double ProcessValue(double x) {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }
    }
}
