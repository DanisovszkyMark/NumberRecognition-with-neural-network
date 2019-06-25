using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberRecognition.Neural_Network
{
    class NetworkProcessor
    {
        private static List<float[]> inputs;
        private static List<float[]> outputs;

        public static void Learning(NeuralNetwork net, string fileName)
        {
            try
            {
                LoadInputOutput(fileName);
            }
            catch { throw new Exception("Nem lehetett beolvasni a fájlból"); }

            for (int i = 0; i < inputs.Count; i++)
            {
                net.FeedForward(inputs[i]);
                net.BackProp(outputs[i]);
            }
        }

        private static void LoadInputOutput(string fileName)
        {
            if (inputs == null || outputs == null)
            {
                inputs = new List<float[]>();
                outputs = new List<float[]>();

                StreamReader sr = new StreamReader(fileName);

                while (!sr.EndOfStream)
                {
                    string[] atm = sr.ReadLine().Split('-');
                    float[] atmInput = new float[atm[0].Length];
                    float[] atmOutput = new float[atm[1].Length];

                    for (int i = 0; i < atm[0].Length; i++)
                    {
                        atmInput[i] = (float)Convert.ToDouble(atm[0][i].ToString());
                    }

                    for (int i = 0; i < atm[1].Length; i++)
                    {
                        atmOutput[i] = (float)Convert.ToDouble(atm[1][i].ToString());
                    }

                    inputs.Add(atmInput);
                    outputs.Add(atmOutput);
                }
                sr.Close();
            }
        }

        public static float Try(NeuralNetwork net, float[] input, int OutputNumber)
        {
            return net.FeedForward(input)[OutputNumber];
        }

        public static void LoadNetFromFile(ref NeuralNetwork net, string fileName)
        {
            StreamReader sr = new StreamReader(fileName);

            string[] first = sr.ReadLine().Split(':');
            int[] atmfloat = new int[first.Length - 1];

            for (int i = 0; i < atmfloat.Length; i++)
            {
                atmfloat[i] = Convert.ToInt32(first[i].ToString());
            }

            net = new NeuralNetwork(atmfloat, first[first.Length - 1]);

            List<Layer> atmLayers = new List<Layer>();
            while (!sr.EndOfStream)
            {
                string[] atm = sr.ReadLine().Split(':');
                int atmNumberOfInputs = Convert.ToInt32(atm[0].ToString());
                int atmNumberOfOutputs = Convert.ToInt32(atm[1].ToString());

                Layer atmLayer = new Layer(atmNumberOfInputs, atmNumberOfOutputs, net.acti);

                float[] atmOutputs = new float[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmOutputs[i] = (float)Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.outputs = atmOutputs;

                float[] atmInputs = new float[atmNumberOfInputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmInputs.Length; i++)
                {
                    atmInputs[i] = (float)Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.inputs = atmInputs;

                float[,] atmWeights = new float[atmNumberOfOutputs, atmNumberOfInputs];

                for (int i = 0; i < atmNumberOfOutputs; i++)
                {
                    atm = sr.ReadLine().Split(':');
                    for (int k = 0; k < atmNumberOfInputs; k++)
                    {
                        atmWeights[i, k] = (float)Convert.ToDouble(atm[k].ToString());
                    }
                }
                atmLayer.weights = atmWeights;

                float[,] atmWeightsDelta = new float[atmNumberOfOutputs, atmNumberOfInputs];

                for (int i = 0; i < atmNumberOfOutputs; i++)
                {
                    atm = sr.ReadLine().Split(':');
                    for (int k = 0; k < atmNumberOfInputs; k++)
                    {
                        atmWeightsDelta[i, k] = (float)Convert.ToDouble(atm[k].ToString());
                    }
                }
                atmLayer.weightsDelta = atmWeightsDelta;

                float[] atmgamma = new float[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmgamma[i] = (float)Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.gamma = atmgamma;

                float[] atmerror = new float[atmNumberOfOutputs];
                atm = sr.ReadLine().Split(':');
                for (int i = 0; i < atmOutputs.Length; i++)
                {
                    atmerror[i] = (float)Convert.ToDouble(atm[i].ToString());
                }
                atmLayer.error = atmerror;

                atmLayers.Add(atmLayer);
            }
            net.Layers = atmLayers.ToArray();
            sr.Close();
        }

        public static void SaveNetToFile(NeuralNetwork net, string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);

            for (int i = 0; i < net.Layer.Length; i++)
            {
                sw.Write(net.Layer[i].ToString());
                if (i < net.Layer.Length - 1) sw.Write(":");
            }
            sw.WriteLine(":" + net.acti);

            foreach (Layer y in net.Layers)
            {
                sw.WriteLine(y.numberOfInputs.ToString() + ":" + y.numberOfOutputs.ToString());

                for (int i = 0; i < y.outputs.Length; i++)
                {
                    sw.Write(y.outputs[i].ToString());
                    if (i < y.outputs.Length - 1) sw.Write(":");
                }
                sw.WriteLine();

                for (int i = 0; i < y.inputs.Length; i++)
                {
                    sw.Write(y.inputs[i].ToString());
                    if (i < y.inputs.Length - 1) sw.Write(":");
                }
                sw.WriteLine();

                for (int i = 0; i < y.numberOfOutputs; i++)
                {
                    for (int j = 0; j < y.numberOfInputs; j++)
                    {
                        sw.Write(y.weights[i, j].ToString());
                        if (j < y.numberOfInputs - 1) sw.Write(":");
                    }
                    sw.WriteLine();
                }

                for (int i = 0; i < y.numberOfOutputs; i++)
                {
                    for (int j = 0; j < y.numberOfInputs; j++)
                    {
                        sw.Write(y.weightsDelta[i, j].ToString());
                        if (j < y.numberOfInputs - 1) sw.Write(":");
                    }
                    sw.WriteLine();
                }

                for (int i = 0; i < y.gamma.Length; i++)
                {
                    sw.Write(y.gamma[i].ToString());
                    if (i < y.gamma.Length - 1) sw.Write(":");
                }
                sw.WriteLine();

                for (int i = 0; i < y.error.Length; i++)
                {
                    sw.Write(y.error[i].ToString());
                    if (i < y.error.Length - 1) sw.Write(":");
                }
                sw.WriteLine();
            }

            sw.Close();
        }
    }
}
