

using System.Runtime.CompilerServices;

namespace QuaDigi 
{
    class Program
    {



        static void Main(string[] args)
        {

            SortSamples sortSamples = new();
            ReadInput reader = new();
            List<Measurement> measurementList = new();
            Dictionary<Measurement.MeasurementType, List<Measurement>> sampledMeasurements = new();
            List<string> input = new();


            //Read input from file
            try
            {
                 input = reader.ReadFile("Input.txt");
            }
            catch 
            {
                Console.WriteLine("Failed to read input file");
                goto FailedToRead;
            }

            //Covert input to List of Class Objects
            try
            {
                measurementList = reader.ParseMeasurements(input);
            }
            catch
            {
                Console.WriteLine("Failed to parse input file");
                goto FailedToRead;
            }

            //Process List
            try
            {
                sampledMeasurements = sortSamples.sample(DateTime.Parse("2017-01-03T10:00:00"), measurementList);
            }
            catch
            {
                Console.WriteLine("Error while sorting");
            }
        FailedToRead:


            Console.WriteLine();
            foreach (var pair in sampledMeasurements)
            {
                foreach (var measurement in pair.Value)
                {
                    Console.WriteLine("{" + measurement.GetMeasurementTime().ToString() + ", " + measurement.GetMeasurementType().ToString() + ", " + measurement.GetMeasurementValue().ToString() + "}");
                }
            }


            


            Console.ReadLine();
        }
    }
}