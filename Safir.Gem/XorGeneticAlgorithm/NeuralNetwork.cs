// <copyright file="NeuralNetwork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Gem.XorGeneticAlgorithm
{
    using System;
    using System.Collections.Generic;

    public class NeuralNetwork
    {
        private readonly Random _gen = new Random();

        private int _numInputNodes;
        private int _numHiddenNodes;
        private int _numOutNodes;

        private double[,] _inputToHiddenWeights;
        private double[,] _hiddenToOutputWeights;

        private List<double> _inputs;
        private List<double> _outputs;
        private List<double> _hidden;

        public NeuralNetwork(int numInputNodes, int numHiddenNodes, int numOutNodes) {
            _numInputNodes = numInputNodes;
            _numHiddenNodes = numHiddenNodes;
            _numOutNodes = numOutNodes;

            _inputToHiddenWeights = new double[numInputNodes + 1, numHiddenNodes];
            _hiddenToOutputWeights = new double[numHiddenNodes + 1, numOutNodes];

            _inputs = new List<double>();
            _outputs = new List<double>();
            _hidden = new List<double>();
        }

        internal delegate void ChangeHandler(object sender, NeuralNetworkEventArgs nne);

        public static bool IsInTraining { get; internal set; }

        public double[,] InputToHiddenWeights { get => _inputToHiddenWeights; set => _inputToHiddenWeights = value; }

        public double[,] HiddenToOutputWeights { get => _hiddenToOutputWeights; set => _hiddenToOutputWeights = value; }

        public int NumberOfInputs => _numInputNodes;

        public int NumberOfHidden => _numHiddenNodes;

        public int NumberOfOutputs => _numOutNodes;

        public void InitializeNetwork() {
            _inputs[_numInputNodes] = 1.0;
            _hidden[_numHiddenNodes] = 1.0;
            for (int i = 0; i < _numInputNodes + 1; i++) {
                for (int j = 0; j < _numHiddenNodes; j++) {
                    _inputToHiddenWeights[i, j] = (_gen.NextDouble() * 4) - 2;
                }
            }

            for (int i = 0; i < _numHiddenNodes + 1; i++) {
                for (int j = 0; j < _numOutNodes; j++) {
                    _hiddenToOutputWeights[i, j] = (_gen.NextDouble() * 4) - 2;
                }
            }
        }

        public double GetError(double[] targets) {
            double error = 0.0;

            error = Math.Sqrt(Math.Pow(targets[0] - _outputs[0], 2));

            return error;
        }

        internal void PassForward(double[] trainingSet, double[] v) {
            // Load a set of inputs into our current inputs
            for (int i = 0; i < _numInputNodes; i++) {
                _inputs[i] = trainingSet[i];
            }

            // Forward to hidden nodes, and calculate activations in hidden layer
            for (int i = 0; i < _numHiddenNodes; i++) {
                double sum = 0.0;
                for (int j = 0; j < _numInputNodes + 1; j++) {
                    sum += _inputs[j] * _inputToHiddenWeights[j, i];
                }

                _hidden[i] = SigmoidActivationFunction.ProcessValue(sum);
            }

            // Forward to output nodes, and calculate activations in output layer
            for (int i = 0; i < _numOutNodes; i++) {
                double sum = 0.0;
                for (int j = 0; j < _numHiddenNodes + 1; j++) {
                    sum += _hidden[j] * _hiddenToOutputWeights[j, i];
                }

                _outputs[i] = SigmoidActivationFunction.ProcessValue(sum);
            }
        }
    }
}