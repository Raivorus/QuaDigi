using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaDigi
{
    class SortSamples
    {
        public Dictionary<Measurement.MeasurementType, List<Measurement>> sample(DateTime startOfSampling, List<Measurement> unsampledMeasurements)
        {
            Dictionary<Measurement.MeasurementType, List<Measurement>> sampledMeasurements = new();
            List<int> toRemove = new();


            foreach (var type in Enum.GetNames(typeof(Measurement.MeasurementType)))
            {
                sampledMeasurements.Add((Measurement.MeasurementType)(int)(Measurement.MeasurementType)Enum.Parse(typeof(Measurement.MeasurementType), type),
                    new List<Measurement>());
            }

            //Remove entries older than startOfSampling
            foreach(var measurement in unsampledMeasurements)
            {
                if (measurement.GetMeasurementTime() < startOfSampling)
                {
                    toRemove.Add(unsampledMeasurements.IndexOf(measurement));
                }
            }
            foreach (int i in toRemove.OrderDescending())
            {
                unsampledMeasurements.RemoveAt(i);
            }
            toRemove.Clear();



            int cycleCount = 0;
            while (unsampledMeasurements.Any()) {
                foreach (Measurement measurement in unsampledMeasurements)
                {
                    if (sampledMeasurements.ContainsKey(measurement.GetMeasurementType()))
                    {
                        //Confirm correct timeframe
                        if (startOfSampling.AddMinutes(5 * (cycleCount)) < measurement.GetMeasurementTime()
                            & measurement.GetMeasurementTime() <= startOfSampling.AddMinutes(5 * (cycleCount + 1)))
                        {
                            //Add indexes of entries to be removed after current timeframe
                            toRemove.Add(unsampledMeasurements.IndexOf(measurement));


                            //If no existing entries
                            if (!sampledMeasurements[measurement.GetMeasurementType()].Any())
                            {
                                //Add new entry
                                sampledMeasurements[measurement.GetMeasurementType()].Add(new Measurement(
                                    measurement.GetMeasurementTime(), measurement.GetMeasurementValue(), measurement.GetMeasurementType()));
                            }
                            else
                            {
                                //If new entry outside previous timeframe
                                if (sampledMeasurements[measurement.GetMeasurementType()][sampledMeasurements[measurement.GetMeasurementType()].Count - 1].GetMeasurementTime()
                                    == startOfSampling.AddMinutes(5 * (cycleCount)))
                                {
                                    //Add new entry
                                    sampledMeasurements[measurement.GetMeasurementType()].Add(new Measurement(
                                        measurement.GetMeasurementTime(), measurement.GetMeasurementValue(), measurement.GetMeasurementType()));
                                }
                                else
                                {
                                    //If new entry has later timestamp than recorded entry
                                    if (sampledMeasurements[measurement.GetMeasurementType()][sampledMeasurements[measurement.GetMeasurementType()].Count - 1].GetMeasurementTime()
                                        < measurement.GetMeasurementTime())
                                    {
                                        //Update entry
                                        sampledMeasurements[measurement.GetMeasurementType()][sampledMeasurements[measurement.GetMeasurementType()].Count - 1]
                                            = new Measurement(measurement.GetMeasurementTime(), measurement.GetMeasurementValue(), measurement.GetMeasurementType());
                                    }
                                }
                            }


                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid MeasurementType: {0}", measurement.GetMeasurementType());
                    }
                }

                //Update newest entries' display time
                foreach (var type in Enum.GetNames(typeof(Measurement.MeasurementType)))
                {
                    var index = (Measurement.MeasurementType)(int)(Measurement.MeasurementType)Enum.Parse(typeof(Measurement.MeasurementType), type);
                    if (sampledMeasurements[index].Any())
                    {
                        sampledMeasurements[index][sampledMeasurements[index].Count - 1] =
                            new Measurement(startOfSampling.AddMinutes(5 * (cycleCount + 1)),
                                sampledMeasurements[index][sampledMeasurements[index].Count - 1].GetMeasurementValue(),
                                sampledMeasurements[index][sampledMeasurements[index].Count - 1].GetMeasurementType());
                    }
                }

                //Increment timeframe
                cycleCount++;

                //Remove entries from latest timeframe
                foreach (int i in toRemove.OrderDescending())
                {
                    unsampledMeasurements.RemoveAt(i);
                }
                toRemove.Clear();
            }

            return sampledMeasurements;
        }
    }
}
