using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Degree_Work
{
    /// <summary>
    /// Интерфейс, который реализуют окна, в которых выполняется построение линий тока.
    /// Определяет методы, требуемые для настройки результата; интерфейсная ссылка используется 
    /// в окне SettingsWindow
    /// </summary>
    public interface IStreamLinesPlotWindow
    {
        /// <summary>
        /// Метод, который вызывается из класса окна, реализующего данный интерфейс, при изменении границ изображения линий тока, расстояния между ними и расстояния между их точками
        /// </summary>
        void OnPlotGeomParamsChanged();

        /// <summary>
        /// Метод, который вызывается из класса окна, реализующего данный интерфейс, при изменении параметров цвета и толщины линий
        /// </summary>
        void OnPlotVisualParamsChanged();
    }
}
