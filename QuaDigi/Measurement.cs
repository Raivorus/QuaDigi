using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuaDigi
{
    class Measurement
    {
        public enum MeasurementType
        {
            TEMP,
            HRATE,
            SPO2
        }


        private DateTime measurementTime;
        private Double measurementValue;
        private MeasurementType type;

        public Measurement(DateTime measurementTime, double measurementValue, MeasurementType type) 
        {
            this.measurementTime = measurementTime;
            this.measurementValue = measurementValue;
            this.type = type;
        }

        public DateTime GetMeasurementTime()
        {
            return measurementTime;
        }
        public Double GetMeasurementValue() 
        { 
            return measurementValue;
        }
        public MeasurementType GetMeasurementType()
        {
            return type;
        }

    }

}
