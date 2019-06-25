using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberRecognition.Neural_Network
{
    class NeuralNetwork
    {
        public int[] Layer;
        public Layer[] Layers;
        public string acti;

        public float[] FeedForward(float[] inputs)
        {
            Layers[0].FeedForward(inputs);
            for (int i = 1; i < Layers.Length; i++)
            {
                Layers[i].FeedForward(Layers[i - 1].outputs);
            }
            return Layers[Layers.Length - 1].outputs;
        }

        public void BackProp(float[] expected)
        {
            for (int i = Layers.Length - 1; i >= 0; i--)
            {
                if (i == Layers.Length - 1) Layers[i].BackPropOutput(expected);
                else Layers[i].BackPropHidden(Layers[i + 1].gamma, Layers[i + 1].weights);
            }

            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i].UpdateWeights();
            }
        }

        public NeuralNetwork(int[] Layer, string acti)
        {
            this.Layer = new int[Layer.Length];
            for (int i = 0; i < Layer.Length; i++)
            {
                this.Layer[i] = Layer[i];
            }

            Layers = new Layer[Layer.Length - 1];
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(Layer[i], Layer[i + 1], acti);
            }

            this.acti = acti;
        }
    }
}
