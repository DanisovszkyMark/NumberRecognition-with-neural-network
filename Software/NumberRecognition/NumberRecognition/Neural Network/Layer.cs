using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberRecognition.Neural_Network
{
    class Layer
    {
        private static Random rnd = new Random();
        public static float learningRate = 0.033f;
        public int numberOfInputs;
        public int numberOfOutputs;

        public float[] outputs;
        public float[] inputs;
        public float[,] weights;

        public float[,] weightsDelta;
        public float[] gamma;
        public float[] error;

        public Layer(int numberOfInputs, int numberOfOutputs, string acti)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            inputs = new float[numberOfInputs];
            outputs = new float[numberOfOutputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            weightsDelta = new float[numberOfOutputs, numberOfInputs];
            gamma = new float[numberOfOutputs];
            error = new float[numberOfOutputs];

            Activation = new Acti(TanH);
            DActivation = new DActi(DTanH);

            InitilizeWeights();
        }

        public float[] FeedForward(float[] inputs)
        {
            this.inputs = inputs;

            for (int i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;
                for (int j = 0; j < numberOfInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }
                outputs[i] = Activation(outputs[i]);
            }
            return outputs;
        }

        public void BackPropOutput(float[] expected)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                error[i] = outputs[i] - expected[i];
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = error[i] * DActivation(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                {
                    gamma[i] += gammaForward[j] * weightsForward[j, i];
                }
                gamma[i] *= DActivation(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void InitilizeWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] = (float)rnd.NextDouble() - 0.5f;
                }
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] -= weightsDelta[i, j] * learningRate;
                }
            }
        }

        //Kiegészíthetőség miatt (jobb lenne rá inkább egy stratégia minta)
        private delegate float Acti(float value);
        private Acti Activation;

        private delegate float DActi(float value);
        private DActi DActivation;

        private float TanH(float value)
        {
            return (float)Math.Tanh(value);
        }

        private float DTanH(float value)
        {
            return 1 - (value * value);
        }
    }
}
