using BenfordSet.Common;
using System;
using System.Collections.Generic;

namespace BenfordSet.Model
{
    internal class Calculation : FileAttributes
    {
        public event EventHandler? CheckFileRequired;
        public event EventHandler? NoCheckFileRequired;

        //private ProgrammEvents _events;
        //public int NumberInFiles { get; set; }
        private int _CountDeviations;
        public Dictionary<int, int> CountedNumbers { get; protected set; }
        internal double _Threshold;
        internal readonly double[] _BenfordNumbers = { 30.1, 17.6, 12.5, 9.7, 7.9, 6.7, 5.8, 5.1, 4.6 };
        internal double[] Digits = new double[9];//{ 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal double[] Difference = new double[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public string CalculationResult { get; set; } = String.Empty;

        public CountNumbers CounterObject { get; private set; }

        public Calculation(CountNumbers countObj, double threshold)
        {
            CounterObject = countObj;
            _Threshold = threshold;
        }

        public void StartCalculation()
        {
            CalculateDistribution();
            Deviation();
            ClassifyResults();
            // GetOutput();
        }


        private void CalculateDistribution()
        {
            for (var k = 0; k <= Digits.Length - 1; k++)
                Digits[k] = Math.Round(ConvertTypes(CountedNumbers[k]) / CounterObject.NumbersInFile * 100, 1);
        }

        private double ConvertTypes(int numbers)
            => (double)numbers;

        private void Deviation()
        {
            for (var k = 0; k < _BenfordNumbers.Length; k++)
                Difference[k] = Math.Round(Math.Abs(_BenfordNumbers[k] - Digits[k]), 1);
        }

        private void ClassifyResults()
        {
            for (int i = 0; i < _BenfordNumbers.Length; i++)
                if (Difference[i] > _Threshold)
                    _CountDeviations += 1;

            if (_CountDeviations > 3)
                this.CheckFileRequired?.Invoke(this, EventArgs.Empty);
            else
                this.NoCheckFileRequired?.Invoke(this, EventArgs.Empty);
        }

        //private void GetOutput()
        //{
        //    Output output = new(CountedNumbers, Digits, Difference, _Threshold);
        //    CalculationResult = output.BuildAnalyseResult();
        //}

    }
}
