using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaDigi
{
    class ReadInput
    {

        //Read input data from .txt file
        //Default location {repo location}\QuaDigi\bin\Debug\net7.0
        public List<string> ReadFile(string fileName)
        {
            List<string> read = new List<string>();
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        read.Add(line);
                    }
                }
            }

            return read;
        }

        //Convert input into List of Class Objects
        //Expected input is a List of String values
        //Expected value format example: "{2017-01-03T10:01:18, SPO2, 98.78}"
        public List<Measurement> ParseMeasurements(List<string> measurements)
        {
            List<Measurement> mList = new();
            foreach (var measurement in measurements)
            {
                string[] measurementValues = measurement.Trim('{').Trim('}').Replace(" ","").Split(",");
                try
                {
                    mList.Add(new Measurement(DateTime.Parse(measurementValues[0]), Double.Parse(measurementValues[2]), 
                        (Measurement.MeasurementType)(int)(Measurement.MeasurementType)Enum.Parse(typeof(Measurement.MeasurementType), measurementValues[1])));
                }
                catch 
                {
                    Console.WriteLine("Failed to parse row: " + measurement);
                }
            }
            return mList;
        }


    }
}
