using System;

namespace Degree_Work.Mathematical_Sources
{
    /// <summary>
    /// Статический класс, предоставляющий методы для решения нелинейных уравнений
    /// </summary>
    public static class Equations
    {
        /// <summary>
        /// Решение уравнения вида f(z)=Z. Производная функции находится по симметричным формулам с использованием правила Рунге. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий комплексное число, представляет собой левую часть уравнения</param>
        /// <param name="Z">Комплексное число, правая часть уравнения</param>
        /// <returns></returns>
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Complex.Complex Z)
        {
            Complex.Complex initial_approximation = Z, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c) - Z; }, dFdz = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if ((f(z) - Z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(z)=Z. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий комплексное число, представляет собой левую часть уравнения</param>
        /// <param name="df">Производная функции левой части</param>
        /// <param name="Z">Комплексное число, правая часть уравнения</param>
        /// <returns></returns>
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Func<Complex.Complex, Complex.Complex> df, Complex.Complex Z)
        {
            //Початкове наближення спочатку є правою частиною
            Complex.Complex initial_approximation = Z, z, tmp;
            //кількість невдач при розрахунках та кількість розрахунків
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c) - Z; }, dFdz = (c) => { return df(c); };
            //Екземпляр класу, призначений для работи с псеводовипадковими величинами
            Random rnd = new Random(DateTime.Now.Millisecond);

            //Точка початку ітераційного процесу при невдачі
            fail_mark:

            //До початкового наближення додаємо псевдовипадкове комплексне число
            initial_approximation += (new Complex.Complex(rnd.NextDouble(), rnd.NextDouble()));
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                //Процес метода Ньютона
                z = z - F(z) / dFdz(z);
                //Якщо досягнута точність
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    //Перевіряємо, чи буде знайдена точка, при дії на неё вказаної
                    //функції конформного відображення правою частиною
                    if ((f(z) - Z).Abs <= Settings.Eps) { return z; }
                    //Якщо ні, то інкрементуємо кількість невдач та переходимо 
                    //в початок ітераційного процесу
                    else { fails++; goto fail_mark; }
                }
                //Якщо результат НЕ-число, нескінченно віддалена точка або кількість розрахунків 
                //стала більною за 100, то інкрементуємо кількість невдач та переходимо 
                //в початок ітераційного процесу
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            //Якщо кількість невдач стала большою за 49, то повертаємо НЕ-число
            return Complex.Complex.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(z)=0. Производная функции находится по симметричным формулам с использованием правила Рунге. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий комплексное число, представляет собой левую часть уравнения</param>
        /// <returns></returns>
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f)
        {
            Complex.Complex initial_approximation = 0, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c); }, dFdz = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if (f(z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(z)=0. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий комплексное число, представляет собой левую часть уравнения</param>
        /// <param name="df">Производная функции левой части</param>
        /// <returns></returns>
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Func<Complex.Complex, Complex.Complex> df)
        {
            Complex.Complex initial_approximation = 0, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c); }, dFdz = (c) => { return df(c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:

            initial_approximation += (new Complex.Complex(rnd.NextDouble(), rnd.NextDouble()));
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if (f(z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0. Производная функции находится по симметричным формулам с использованием правила Рунге. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <returns></returns>
        public static double Solve(Func<double, double> f)
        {
            double initial_approximation = 0, x, tmp;
            int fails = 0, iters;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);
            fail_mark:
            initial_approximation += rnd.NextDouble();
            x = initial_approximation;
            tmp = x;
            iters = 0;
            while (fails < 100)
            {
                iters++;
                x = x - f(x) / dfdx(x);
                if (Math.Abs(x - tmp) <= Settings.Eps)
                {
                    if (Math.Abs(x) <= Settings.Eps) { return x; }
                    else { fails++; goto fail_mark; }
                }
                if (double.IsNaN(x) || double.IsInfinity(x) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = x;
            }
            return double.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0. Начальные приближение задается случайным образом.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <param name="dfdx">Производная функции левой части</param>
        /// <returns></returns>
        public static double Solve(Func<double, double> f, Func<double, double> dfdx)
        {
            double initial_approximation = 0, x, tmp;
            int fails = 0, iters;
            Random rnd = new Random(DateTime.Now.Millisecond);
            fail_mark:
            initial_approximation += rnd.NextDouble();
            x = initial_approximation;
            tmp = x;
            iters = 0;
            while (fails < 100)
            {
                iters++;
                x = x - f(x) / dfdx(x);
                if (Math.Abs(x - tmp) <= Settings.Eps)
                {
                    if (Math.Abs(x) <= Settings.Eps) { return x; }
                    else { fails++; goto fail_mark; }
                }
                if (double.IsNaN(x) || double.IsInfinity(x) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = x;
            }
            return double.NaN;
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0. Производная функции находится по симметричным формулам с использованием правила Рунге.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <param name="initial_approximation">Начальное приближение</param>
        /// <returns></returns>
        public static double NSolve(Func<double, double> f, double initial_approximation)
        {
            double x = initial_approximation, tmp = x;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { if (Math.Abs(f(x)) < Settings.Eps) { return x; } else { throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation."); } }
                tmp = x;
            }
            throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation.");
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <param name="dfdx">Производная функции левой части</param>
        /// <param name="initial_approximation">Начальное приближение</param>
        /// <returns></returns>
        public static double NSolve(Func<double, double> f, Func<double, double> dfdx, double initial_approximation)
        {
            double x = initial_approximation, tmp = x;
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { if (Math.Abs(f(x)) < Settings.Eps) { return x; } else { throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation."); } }
                tmp = x;
            }
            throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation.");
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0. Производная функции находится по симметричным формулам с использованием правила Рунге.
        /// Не генерирует исключения.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <param name="initial_approximation">Начальное приближение</param>
        /// <param name="x">Корень уравнение, передается из метода по ссылке</param>
        /// <returns>Успешно ли найден корень</returns>
        public static bool TryNSolve(Func<double, double> f, double initial_approximation, out double x)
        {
            x = initial_approximation; double tmp = x;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { return Math.Abs(f(x)) < Settings.Eps ? true : false; }
                tmp = x;
            }
            return false;
        }

        /// <summary>
        /// Решение уравнения вида f(х)=0.
        /// Не генерирует исключения.
        /// </summary>
        /// <param name="f">Обобщенный делегат Func, принимающий и возвращающий число с плавающей точкой, представляет собой левую часть уравнения</param>
        /// <param name="dfdx">Производная функции левой части</param>
        /// <param name="initial_approximation">Начальное приближение</param>
        /// <param name="x">Корень уравнение, передается из метода по ссылке</param>
        /// <returns>Успешно ли найден корень</returns>
        public static bool TryNSolve(Func<double, double> f, Func<double, double> dfdx, double initial_approximation, out double x)
        {
            x = initial_approximation; double tmp = x;
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { return Math.Abs(f(x)) < Settings.Eps ? true : false; }
                tmp = x;
            }
            return false;
        }

        /// <summary>
        /// Первая производная функции комплексного аргумента
        /// </summary>
        /// <param name="f">Функция комплексного аргумента</param>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        private static Complex.Complex FirstDerivative(Func<Complex.Complex, Complex.Complex> f, Complex.Complex z)
        {
            return (f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * Settings.Eps) + ((f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * Settings.Eps) - (f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }

        /// <summary>
        /// Первая производная функции действительного аргумента
        /// </summary>
        /// <param name="f">Функция действительного аргумента</param>
        /// <param name="x">Действительный аргумент</param>
        /// <returns></returns>
        private static double FirstDerivative(Func<double, double> f, double x)
        {
            return (f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * Settings.Eps) + ((f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * Settings.Eps) - (f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }
    }
}
