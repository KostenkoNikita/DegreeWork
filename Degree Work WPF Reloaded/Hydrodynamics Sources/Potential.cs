#pragma warning disable 169
#pragma warning disable 168
//#define HELP_FOR_GROUP_LEADER

using System;

using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;
using static Degree_Work.Mathematical_Sources.Complex.Complex;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Degree_Work.Mathematical_Sources.Complex;

namespace Degree_Work.Hydrodynamics_Sources
{
    class Potential : INotifyPropertyChanged
    {
        /// <summary>
        /// Индексатор, возвращающий значение потенциала в точке
        /// </summary>
        /// <param name="dzeta">Комплексное число, представляющее собой точку, значение потенциала в которой нужно определить</param>
        /// <returns></returns>
        public Complex this[Complex dzeta] { get { return W(dzeta); } }

        /// <summary>
        /// Скорость потока на бесконечности
        /// </summary>
        double _V_inf;

        /// <summary>
        /// Угол атаки
        /// </summary>
        double _alpha;

        /// <summary>
        /// Радиус обтекаемого цилиндра
        /// </summary>
        double _R;

        /// <summary>
        /// Циркуляция
        /// </summary>
        double _G;

        /// <summary>
        /// Интерфейсная ссылка, представляющая собой функцию конформного отображения.
        /// Функция представляет собой тип данных, реализующий интерфейс IConformalMapFunction.
        /// Такой подход позволяет использовать полиморфизм в коде программы, имея один класс,
        /// представляющий собой комплексный потенциал, и полиморфную ссылку, к которой будут
        /// неявно приводиться любые типы, реализующие нужный интерфейс
        /// </summary>
        IConformalMapFunction _f;

        /// <summary>
        /// Скорость на бесконечности
        /// </summary>
        public double V_inf
        {
            get
            {
                return _V_inf;
            }
            set
            {
                if (value > 0) { _V_inf = value; }
                else if (value < 0) { _V_inf = -value; }
                else { throw new ArgumentException(); }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Радиус обтекаемого цилиндра
        /// </summary>
        public double R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value == 0 ? throw new ArgumentException() : Abs(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Циркуляция потока
        /// </summary>
        public double G
        {
            get
            {
                return _G;
            }
            set
            {
                _G = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Угол атаки в радианах
        /// </summary>
        public double AlphaRadians
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Угол атаки в градусах
        /// </summary>
        public double AlphaDegrees
        {
            get
            {
                return _alpha * 180.0 / Math.PI; ;
            }
            set
            {
                _alpha = value * Math.PI / 180.0;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Интерфейсная ссылка, представляющая собой функцию конформного отображения.
        /// Функция представляет собой тип данных, реализующий интерфейс IConformalMapFunction.
        /// Такой подход позволяет использовать полиморфизм в коде программы, имея один класс,
        /// представляющий собой комплексный потенциал, и полиморфную ссылку, к которой будут
        /// неявно приводиться любые типы, реализующие нужный интерфейс
        /// </summary>
        public IConformalMapFunction f
        {
            get { return _f; }
            set
            {
                _f = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Временная переменная, используется для вичисления скорости в физической плоскости
        /// </summary>
        Complex tmp;

        /// <summary>
        /// Событие, которое выполняется при изменении какого-то свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Конструктор экземпляра класса, инкапсулирующего функционал комплексного потенциала течения
        /// </summary>
        /// <param name="V_inf">Скорость течения на бесконечности</param>
        /// <param name="alpha">Угол атаки в радианах. Должен быть равен нулю в случае обтекания полосы или полуплоскости</param>
        /// <param name="R">Радиус обтекаемого цилинда. Должен быть равен нулю в случае обтекания полосы или полуплоскости</param>
        /// <param name="G">Циркуляция. Должна быть равна нулю в случае обтекания полосы или полуплоскости</param>
        /// <param name="f">Функция конформного отображения (реализует интерфейс IConformalMapFunction)</param>
        public Potential(double V_inf, double alpha, double R, double G, IConformalMapFunction f)
        {
            this.f = f;
            this._V_inf = V_inf;
            this._R = R;
            this._alpha = R == 0 ? 0 : alpha;
            this._G = R == 0 ? 0 : G;
        }

        /// <summary>
        /// Потенциальная функция
        /// </summary>
        /// <param name="z">Точка, в которой ищется значение потенциальной функции</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double phi(Complex z)
        {
            return this[z].Re;
        }

        /// <summary>
        /// Функция тока
        /// </summary>
        /// <param name="z">Точка, в которой ищется значение функции тока</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double psi(Complex z)
        {
            return this[z].Im;
        }

        /// <summary>
        /// Скорость во вспомогательной плоскости в виде комплексного числа
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение скорости</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex V(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate;
        }

        /// <summary>
        /// Скорость в физической плоскости в виде комплексного числа
        /// </summary>
        /// <param name="z">Точка, в которой ищется значение скорости</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex V_physical_plane(Complex z)
        {
            try
            {
                tmp = f.dzeta(z);
                return IsNaN(tmp) ? NaN : (dW_ddzeta(z) / f.dz_ddzeta(tmp)).Conjugate;
            }
            catch { return NaN; }
        }

        /// <summary>
        /// Горизонтальная компонента скорости во вспомогательной плоскости
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение горозонтальной компоненты скорости</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double V_ksi(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Re;
        }

        /// <summary>
        /// Вертикальная компонента скорости во вспомогательной плоскости
        /// </summary>
        /// <param name="dzeta">Точка, в которой ищется значение горозонтальной компоненты скорости</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double V_eta(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Im;
        }

        /// <summary>
        /// Значение комплексного потенциала в точке
        /// </summary>
        /// <param name="dzeta">Комплексное число, представляющее собой точку, в которой ищется значение комплексного потенциала</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex W(Complex dzeta)
        {
            return _V_inf * Exp(-I * this._alpha) * dzeta + (_R * _R * _V_inf * Exp(I * _alpha)) / dzeta + this._G * Ln(dzeta) / (2 * Math.PI * I);
        }

        /// <summary>
        /// Значение производной комплексного потенциала по комплексной координате в точке
        /// </summary>
        /// <param name="dzeta">Комплексное число, представляющее собой точку, в которой ищется значение производной комплексного потенциала по комплексной координате</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex dW_ddzeta(Complex dzeta)
        {
            return _V_inf * Exp(-I * this._alpha) - (_R * _R * _V_inf * Exp(I * this._alpha)) / (dzeta * dzeta) + this._G / (2 * Math.PI * I * dzeta);
        }

        /// <summary>
        /// Переопределенный метод класс Object, возвращающий строковое представление экземпляра класса комплексного потенциала
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"V={_V_inf}, alpha={_alpha}, R={_R}, G={_G}";
        }

        /// <summary>
        /// Метод, в аргументы которого передается имя вызывающего компонента в рамках выполнения события PropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



#if HELP_FOR_GROUP_LEADER
    class PotentialHelp
    {
        public Complex this[Complex dzeta] { get { return W(dzeta); } }

        public double Sh1, a,b,R,Sh2;
        IConformalMapFunction _f;

        Complex tmp;
        public PotentialHelp(double Sh1, double Sh2)
        {
            this.a = 1;
            this.b = 1;
            this.R = 2.5;
            this.Sh1 = Sh1;
            this.Sh2 = Sh2;
            _f = new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform();
        }
        public double phi(Complex z)
        {
            return this[z].Re;
        }
        public double psi(Complex z)
        {
            return this[z].Im;
        }
        public Complex V(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate;
        }
        public Complex V_physical_plane(Complex z)
        {
            return new Complex();
        }
        public double V_ksi(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Re;
        }
        public double V_eta(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Im;
        }
        public Complex W(Complex dzeta)
        {
            //return -(1.0 / pi) * (V0 * ((dzeta - 1) * ln(dzeta - a) - (dzeta + a) * ln(dzeta + a) + 2 * a)
            //    + (Sh / 4.0) * (2 * (dzeta - a) * (dzeta + a) * (ln(dzeta - a) - ln(dzeta + a)) + 3 * a * a - 4 * a * dzeta)
            //    + 2 * Sh * a * dzeta);

   //         return (-1 / pi) * ((dzeta - 1) * ln(dzeta - 1) - (dzeta + 1) * ln(dzeta + 1) +
   //2 + 0.25 *
   // Sh1 * (4 * dzeta + 3 +
   //   2 * (dzeta - 1) * (dzeta + 1) * ln((dzeta - 1) / (dzeta + 1))));

            return -((dzeta / a - 1) * ln((dzeta / a - 1)) - (dzeta / a + 1) * ln((dzeta / a + 1)) + 0.4e1 + 0.25e0 * Sh1 * (0.2e1 * (dzeta / a - 1) * (dzeta / a + 1) * (ln((dzeta / a - 1)) - ln((dzeta / a + 1))) + 0.3e1 + (4 * dzeta / a)) + ((dzeta - R) / b - 1) * ln(((dzeta - R) / b - 1)) - ((dzeta - R) / b + 1) * ln(((dzeta - R) / b + 1)) + 0.25e0 * Sh2 * (0.2e1 * ((dzeta - R) / b - 1) * ((dzeta - R) / b + 1) * (ln(((dzeta - R) / b - 1)) - ln(((dzeta - R) / b + 1))) + 0.3e1 + (4 * (dzeta - R) / b))) / Math.PI;
        }
        public Complex dW_ddzeta(Complex dzeta)
        {
            //return -(V0 * (ln(dzeta - a) - ln(dzeta + a)) + Sh * (0.2e1 * (dzeta + a) * (ln(dzeta - a) - ln(dzeta + a)) + 0.2e1 * (dzeta - a) * (ln(dzeta - a) - ln(dzeta + a)) + 0.2e1 * (dzeta - a) * (dzeta + a) * (0.1e1 / (dzeta - a) - 0.1e1 / (dzeta + a))) / 0.4e1) / Math.PI;
            //return 0.31831 * (-2 * Sh1 - ln(-1 + dzeta) - dzeta * Sh1 * ln((-1 + dzeta) / (1 + dzeta)) + ln(1 + dzeta));
            return -(0.1e1 / a * ln((dzeta / a - 1)) - 0.1e1 / a * ln((dzeta / a + 1)) + 0.25e0 * Sh1 * (0.2e1 / a * (dzeta / a + 1) * (ln((dzeta / a - 1)) - ln((dzeta / a + 1))) + 0.2e1 * (dzeta / a - 1) / a * (ln((dzeta / a - 1)) - ln((dzeta / a + 1))) + (2 * (dzeta / a - 1) * (dzeta / a + 1) * (1 / a / (dzeta / a - 1) - 1 / a / (dzeta / a + 1))) + (4 / a)) + 0.1e1 / b * ln(((dzeta - R) / b - 1)) - 0.1e1 / b * ln(((dzeta - R) / b + 1)) + 0.25e0 * Sh2 * (0.2e1 / b * ((dzeta - R) / b + 1) * (ln(((dzeta - R) / b - 1)) - ln(((dzeta - R) / b + 1))) + 0.2e1 * ((dzeta - R) / b - 1) / b * (ln((dzeta - R) / b - 1) - ln((dzeta - R) / b + 1)) + (2 * ((dzeta - R) / b - 1) * ((dzeta - R) / b + 1) * (1 / b / ((dzeta - R) / b - 1) - 1 / b / ((dzeta - R) / b + 1))) + (4 / b))) / Math.PI;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
#endif
}
