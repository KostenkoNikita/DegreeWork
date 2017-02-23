using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    interface IConformalMapFunction
    {
        DataPoint this[DataPoint dzeta] { get; }
        complex z(complex dzeta);
        complex dz_ddzeta(complex dzeta);
        complex dzeta(complex Z);
        DataPoint z(DataPoint dzeta);
        DataPoint dz_ddzeta(DataPoint dzeta);
        DataPoint dzeta(DataPoint Z);
    }
}
