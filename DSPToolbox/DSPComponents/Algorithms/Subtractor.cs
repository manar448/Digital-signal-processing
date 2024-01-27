using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            //throw new NotImplementedException();

            MultiplySignalByConstant multi = new MultiplySignalByConstant();
            multi.InputSignal = InputSignal2;
            multi.InputConstant = -1;
            multi.Run();
            InputSignal2 = multi.OutputMultipliedSignal;

            Adder add = new Adder();
            add.InputSignals = new List<Signal>();
            add.InputSignals.Add(InputSignal1);
            add.InputSignals.Add(InputSignal2);
            add.Run();

            OutputSignal = add.OutputSignal;
        }
    }
}