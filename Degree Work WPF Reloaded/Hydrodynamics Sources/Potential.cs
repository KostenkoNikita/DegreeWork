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
    /// <summary>
    /// Клас, що інкапсулює функціонал комплексного потенціалу течії
    /// Спадкує інтерфейс, що має подію, яка повідомлює про зміну 
    /// параметрів комплексного потенціалу
    /// </summary>
    class Potential : INotifyPropertyChanged
    {
        /// <summary>
        /// Індексатор, що повертає значення комплексного потенціалу в точці
        /// </summary>
        /// <param name="dzeta">Комплексне число, що являє собою точку, в якій шукається значення потенціалу</param>
        /// <returns></returns>
        public Complex this[Complex dzeta] { get { return W(dzeta); } }

        /// <summary>
        /// Швидкість потоку в нескінченності
        /// </summary>
        double _V_inf;

        /// <summary>
        /// Кут атаки
        /// </summary>
        double _alpha;

        /// <summary>
        /// Радіус циліндра, що обтікається
        /// </summary>
        double _R;

        /// <summary>
        /// Циркуляція
        /// </summary>
        double _G;

        /// <summary>
        /// Інтерфейсне посилання, що являє собою функцію конформного відображення.
        /// Функція являє собою тип даних, який реалізує інтерфейс IConformalMapFunction.
        /// Такий підхід дозволяє використовувати поліморфізм в коді програми, маючи один клас,
        /// що представляє собою комплексний потенціал, і поліморфне посилання, до якого будуть
        /// неявно зводитися будь-які типи, які реалізують потрібний інтерфейс
        /// </summary>
        IConformalMapFunction _f;

        /// <summary>
        /// Швидкість на нескінченності
        /// </summary>
        public double V_inf
        {
            get
            {
                return _V_inf;
            }
            set
            {
                //Не може бути нульовою ао від'ємною
                _V_inf = value == 0 ? throw new ArgumentException() : Abs(value);
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
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
                //Не може бути нульовим ао від'ємним (якщо не задається в конструкторі)
                _R = value == 0 ? throw new ArgumentException() : Abs(value);
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Циркуляція потоку
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
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Кут атаки в радіанах
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
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Кут атаки в градусах
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
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Інтерфейсне посилання, що являє собою функцію конформного відображення.
        /// Функція являє собою тип даних, який реалізує інтерфейс IConformalMapFunction.
        /// Такий підхід дозволяє використовувати поліморфізм в коді програми, маючи один клас,
        /// що представляє собою комплексний потенціал, і поліморфне посилання, до якого будуть
        /// неявно зводитися будь-які типи, які реалізують потрібний інтерфейс
        /// </summary>
        public IConformalMapFunction f
        {
            get { return _f; }
            set
            {
                _f = value;
                //Виконання метода з подією повідомлення (не використовується)
                //в данній реалізації
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Термінова змінна
        /// </summary>
        Complex tmp;

        /// <summary>
        /// Подія, яка виконується при зміні якогост параметра
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Конструктор екземпляра класа, що інкапсулює функціонал комплексного потенціалу течії
        /// </summary>
        /// <param name = "V_inf"> Швидкість течії у нескінченності </param>
        /// <param name = "alpha"> Кут атаки в радіанах. Повинен дорівнювати нулю в разі обтікання смуги або півплощини </param>
        /// <param name = "R"> Радіус циліндра, що обтікається. Повинен дорівнювати нулю в разі обтікання смуги або півплощини </param>
        /// <param name = "G"> Циркуляція. Повинна дорівнювати нулю в разі обтікання смуги або півплощини </param>
        /// <param name = "f"> Функція конформного відображення (реалізує інтерфейс IConformalMapFunction) </param>
        public Potential(double V_inf, double alpha, double R, double G, IConformalMapFunction f)
        {
            this.f = f;
            this._V_inf = V_inf;
            this._R = R;
            this._alpha = R == 0 ? 0 : alpha;
            this._G = R == 0 ? 0 : G;
        }

        /// <summary>
        /// Потенціальна функція
        /// </summary>
        /// <param name="z">Точка, в якій шукається значення потенціальної функції</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double phi(Complex z)
        {
            return this[z].Re;
        }

        /// <summary>
        /// Функція струму
        /// </summary>
        /// <param name="z">Точка, в якій шукається значення функції струму</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double psi(Complex z)
        {
            return this[z].Im;
        }

        /// <summary>
        /// Швидкість у допоміжній площині у вигляді комплексного числа
        /// </summary>
        /// <param name="dzeta">Точка, в якій шукається значення швидкості</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex V(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate;
        }

        /// <summary>
        /// Швидкість у фізичній площині у вигляді комплексного числа
        /// </summary>
        /// <param name="z">Точка, в якій шукається значення швидкості</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex V_physical_plane(Complex z)
        {
            try
            {
                //Знаходимо значення оберної функції конформного відображення в точці.
                //У випадку будь-якої помилки повертаємо НЕ-число (NaN).
                //В іншому випадку знаходимо швижкість (за правилом дифереціювання
                //складної функції)
                tmp = f.dzeta(z);
                return IsNaN(tmp) ? NaN : (dW_ddzeta(z) / f.dz_ddzeta(tmp)).Conjugate;
            }
            catch { return NaN; }
        }

        /// <summary>
        /// Горизонтальна компонента швидкості у допоміжній площині
        /// </summary>
        /// <param name="dzeta">Точка, в якій шукається значення горизонтальної компоненти швидкості</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double V_ksi(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Re;
        }

        /// <summary>
        /// Вертикальна компонента швидкості у допоміжній площині
        /// </summary>
        /// <param name="dzeta">Точка, в якій шукається значення вертикальної компоненти швидкості</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double V_eta(Complex dzeta)
        {
            return dW_ddzeta(dzeta).Conjugate.Im;
        }

        /// <summary>
        /// Значення комплексного потенціала в точці
        /// </summary>
        /// <param name="dzeta">Комплексное число, представляющее собой точку, в которой ищется значение комплексного потенциала</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex W(Complex dzeta)
        {
            return _V_inf * Exp(-I * this._alpha) * dzeta + (_R * _R * _V_inf * Exp(I * _alpha)) / dzeta + this._G * Ln(dzeta) / (2 * Math.PI * I);
        }

        /// <summary>
        /// Значення похідної від функції комплексного потенціала в точці
        /// </summary>
        /// <param name="dzeta">Точка, в якій шукається значення комплексного потенцалу</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex dW_ddzeta(Complex dzeta)
        {
            //Комплексний потенціал заданий у загальному вигляді для 
            return _V_inf * Exp(-I * this._alpha) - (_R * _R * _V_inf * Exp(I * this._alpha)) / (dzeta * dzeta) + this._G / (2 * Math.PI * I * dzeta);
        }

        /// <summary>
        /// Строкове представлення комплексного потенціала
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"V={_V_inf}, alpha={_alpha}, R={_R}, G={_G}";
        }

        /// <summary>
        /// Метод, що визивається при виконанні події PropertyChanged
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
