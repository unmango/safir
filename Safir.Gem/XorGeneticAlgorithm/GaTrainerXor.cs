// <copyright file="GaTrainerXor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem.XorGeneticAlgorithm
{
    using System;
    using System.Collections.Generic;

    public class GaTrainerXor
    {
        private const double ACCEPTABLE_NN_ERROR = 0.1;
        private const int POPULATION = 15;
        private const double MUTATION = 0.5;
        private const double RECOMBINE = 0.4;

        private Random _gen = new Random(5);

        private double[,] _trainSet = {
             { 0, 0 },
             { 0, 1 },
             { 1, 0 },
             { 1, 1 }
        };

        private List<NeuralNetwork> _networks;
        private bool _foundGoodANN = false;
        private int _bestConfiguration = -1;

        public GaTrainerXor() {
            _networks = new List<NeuralNetwork>();
            for (int i = 0; i <= _networks.Capacity; i++) {
                _networks[i] = new NeuralNetwork(2, 2, 1);
            }
        }

        public object GaTrainerNnChange { get; private set; }

        public NeuralNetwork Train(int trainingTimes) {
            int a, b, winner, loser;
            a = b = winner = loser = 0;

            for (int i = 0; i < trainingTimes; i++) {
                NeuralNetwork.IsInTraining = true;

                if (_foundGoodANN) {
                    break;
                }

                // pick 2 ANN's at random, GA - SELECTION
                a = (int)(_gen.NextDouble() * POPULATION);
                b = (int)(_gen.NextDouble() * POPULATION);

                if (Evaluate(a) < Evaluate(b)) {
                    winner = a;
                    loser = b;
                } else {
                    winner = b;
                    loser = a;
                }

                double[,] winnerInputToHiddenWeights = _networks[winner].InputToHiddenWeights;
                double[,] loserInputToHiddenWeights = _networks[loser].InputToHiddenWeights;
                double[,] winnerHiddenToOutputWeights = _networks[winner].HiddenToOutputWeights;
                double[,] loserHiddenToOutputWeights = _networks[winner].HiddenToOutputWeights;

                // i_to_h_wts RECOMBINATION LOOP
                for (int j = 0; j < _networks[winner].NumberOfInputs + 1; j++) {
                    for (int k = 0; k < _networks[winner].NumberOfHidden; k++) {
                        // get genes from winner randomly for i_to_h_wts wieghts
                        if (_gen.NextDouble() < RECOMBINE) {
                            // set the weights to be that of the input weights from GA
                            loserInputToHiddenWeights[j, k] = winnerInputToHiddenWeights[j, k];
                        }
                    }
                }

                // h_to_o_wts RECOMBINATION LOOP
                for (int j = 0; j < _networks[winner].NumberOfHidden + 1; j++) {
                    for (int k = 0; k < _networks[winner].NumberOfOutputs; k++) {
                        // get genes from winner randomly for i_to_h_wts wieghts
                        if (_gen.NextDouble() < RECOMBINE) {
                            // set the weights to be that of the input weights from GA
                            loserHiddenToOutputWeights[j, k] = winnerHiddenToOutputWeights[j, k];
                        }
                    }
                }

                // i_to_h_wts MUTATION LOOP
                for (int j = 0; j < _networks[winner].NumberOfInputs + 1; j++) {
                    for (int k = 0; k < _networks[winner].NumberOfHidden; k++) {
                        // add some mutation randomly
                        if (_gen.NextDouble() < MUTATION) {
                            loserInputToHiddenWeights[j, k] += (_gen.NextDouble() * 0.2) - 0.1;
                        }
                    }
                }

                // h_to_o_wts MUTATION LOOP
                for (int j = 0; j < _networks[winner].NumberOfHidden + 1; j++) {
                    for (int k = 0; k < _networks[winner].NumberOfOutputs; k++) {
                        // add some mutation randomly
                        if (_gen.NextDouble() < MUTATION) {
                            loserHiddenToOutputWeights[j, k] += (_gen.NextDouble() * 0.2) - 0.1;
                        }
                    }
                }

                _networks[loser].InputToHiddenWeights = loserInputToHiddenWeights;
                _networks[loser].HiddenToOutputWeights = loserHiddenToOutputWeights;
            }

            // AT THIS POINT ITS EITHER THE END OF TRAINING OR WE HAVE
            // FOUND AN ACCEPTABLE ANN, WHICH IS BELOW THE VALUE
            NeuralNetwork.IsInTraining = false;

            if (_bestConfiguration == -1) {
                _bestConfiguration = winner;
            }

            return _networks[_bestConfiguration];
        }

        public void DoActualRun() {
            for (int i = 0; i <= _trainSet.GetUpperBound(0); i++) {
                ForwardWeights(_bestConfiguration, GetTrainSet(i));
                double[] targetValues = GetTargetValues(GetTrainSet(i));
            }
        }

        private double Evaluate(int popMember) {
            double error = 0.0;

            for (int i = 0; i <= _trainSet.GetUpperBound(0); i++) {
                ForwardWeights(popMember, GetTrainSet(i));
                double[] targetValues = GetTargetValues(GetTrainSet(i));
                error += _networks[popMember].GetError(targetValues);
            }

            if (error < ACCEPTABLE_NN_ERROR) {
                _bestConfiguration = popMember;
                _foundGoodANN = true;
            }

            return error;
        }

        private double[] GetTargetValues(double[] currSet) {
            double valOfSet = 0;
            double[] targetArgs = new double[1];
            for (int i = 0; i < currSet.Length; i++) {
                valOfSet += currSet[i];
            }

            targetArgs[0] = valOfSet == 1 ? 1.0 : 0.0;
            return targetArgs;
        }

        private double[] GetTrainSet(int i) {
            double[] trainValues = {
                _trainSet[i, 0],
                _trainSet[i, 1]
            };
            return trainValues;
        }

        private void ForwardWeights(int popMember, double[] trainingSet) {
            _networks[popMember].PassForward(trainingSet, GetTargetValues(trainingSet));
        }
    }
}
