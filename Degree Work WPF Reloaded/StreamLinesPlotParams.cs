using System;
using OxyPlot;

namespace Degree_Work
{
    public class StreamLinesPlotGeomParams
    {
        internal double XMin, XMax, YMin, YMax, MRKh, hVertical;

        public StreamLinesPlotVisualParams Clone()
        {
            return (StreamLinesPlotVisualParams)MemberwiseClone();
        }
    }
    public class StreamLinesPlotVisualParams
    {
        internal OxyColor LineColor, ArrowColor, BorderFillColor, BorderStrokeColor;
        internal double BorderStrokeThickness, LineStrokeThickness, ArrowStokeThickness;

        public StreamLinesPlotVisualParams Clone()
        {
            return (StreamLinesPlotVisualParams)MemberwiseClone();
        }
    }
}
