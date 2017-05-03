using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    interface IConformalMapFunction
    {
        /// <summary>
        /// Индексатор только для чтения, которому передается точка DataPoint для получения значения функции конформного отображения в этой точке
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение функции</param>
        /// <returns></returns>
        DataPoint this[DataPoint dzeta] { get; }

        /// <summary>
        /// Значение функции в точке
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение функции</param>
        /// <returns></returns>
        complex z(complex dzeta);

        /// <summary>
        /// Значение производной функции в точке
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение производной функции</param>
        /// <returns></returns>
        complex dz_ddzeta(complex dzeta);

        /// <summary>
        /// Значение функции, обратной к функции конформного отображения
        /// </summary>
        /// <param name="Z">Точка, в которой ищется значение функции, обратной к функции конформного отображения</param>
        /// <returns></returns>
        complex dzeta(complex Z);

        /// <summary>
        /// Значение функции в точке
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение функции</param>
        /// <returns></returns>
        DataPoint z(DataPoint dzeta);

        /// <summary>
        /// Значение производной функции в точке
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение производной функции</param>
        /// <returns></returns>
        DataPoint dz_ddzeta(DataPoint dzeta);

        /// <summary>
        /// Значение функции, обратной к функции конформного отображения
        /// </summary>
        /// <param name="Z">Точка, в которой ищется значение функции, обратной к функции конформного отображения</param>
        /// <returns></returns>
        DataPoint dzeta(DataPoint Z);
    }
}
