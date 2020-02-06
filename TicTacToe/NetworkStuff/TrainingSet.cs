using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class TrainingSet
    {
        public double[][] inputs;
        /*
         {
            {1,1,0,0,1}, --each number will be mapped unto an inputneuron
            {1,1,0,0,1}, --each array of inputs corresponds to a resultset
            {1,1,0,0,1},
            
             */

        public double[][] results;

        public void toFile(String path)
        {
            if (File.Exists(path))
            {
                path = path + "_new";

            }
            var json1 = JsonConvert.SerializeObject(this);
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(json1.ToString());
                tw.Close();
            }
        }

        public static TrainingSet getFromFile(String path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);
            TrainingSet results = JsonConvert.DeserializeObject<TrainingSet>(json);
            var inputs = results.inputs;
            var resultss = results.results;
            TrainingSet t = new TrainingSet();
            t.inputs = inputs;
            t.results = resultss;
            return t;
        }
    }
}
