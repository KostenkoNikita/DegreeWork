using Degree_Work.Mathematical_Sources.Complex;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    /// <summary>
    /// Інтерфейс, що має в собі загальні методи і індексатор для функції конформного відображення.
    /// Використання інтерфейсного посилання дозволяє використовувати поліморфізм в коді, визначивши
    /// в класі комплексного потенціалу змінну типу інтерфейсу і використовувати її для безлічі 
    /// конформних відображень,
    /// що визначені в програмі.
    /// </summary>
    interface IConformalMapFunction
    {
        /// <summary>
        /// Індексатор з доступом тільки для читання, що приймає об'єкт точки і
        /// повертає значення функції конформного відображення (також у вигляді точки)
        /// </summary>
        /// <param name="dzeta"></param>
        /// <returns></returns>
        DataPoint this[DataPoint dzeta] { get; }

        /// <summary>
        /// Значення функції в точці (у вигляді комплексного числа)
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение функции</param>
        /// <returns></returns>
        Complex z(Complex dzeta);

        /// <summary>
        /// Значення похідної функції в точці (у вигляді комплексного числа)
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение производной функции</param>
        /// <returns></returns>
        Complex dz_ddzeta(Complex dzeta);

        /// <summary>
        /// Значення функції, оберненої до функції конформного відображення
        /// (у вигляді комплексного числа)
        /// </summary>
        /// <param name="Z">Точка, в которой ищется значение функции, обратной к функции конформного отображения</param>
        /// <returns></returns>
        Complex dzeta(Complex Z);

        /// <summary>
        /// Значення функції в точці (у вигляді точки)
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение функции</param>
        /// <returns></returns>
        DataPoint z(DataPoint dzeta);

        /// <summary>
        /// Значення похідної функції в точці (у вигляді точки)
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение производной функции</param>
        /// <returns></returns>
        DataPoint dz_ddzeta(DataPoint dzeta);

        /// <summary>
        /// Значення функції, оберненої до функції конформного відображення
        /// (у вигляді точки)
        /// </summary>
        /// <param name="Z">Точка, в которой ищется значение функции, обратной к функции конформного отображения</param>
        /// <returns></returns>
        DataPoint dzeta(DataPoint Z);
    }
}
