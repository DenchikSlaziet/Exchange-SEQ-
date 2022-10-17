using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


namespace Exchange
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var Kurs = 69.76m;
            var CommissionDefolt = 8.0m;
            var CommissionException = 0.37m;
            var Sum = 0.0m;
            var log  = new LoggerConfiguration().MinimumLevel.Information().
            Enrich.WithProperty("Kurs",Kurs).
            WriteTo.Seq("http://localhost:5341", apiKey: "kkFLkDCP7ZQsYAJMCYDK").
            CreateLogger();

            Console.WriteLine("Курс валют\nUSD: " + Kurs + "\nКомиссия при обмене до 500$ = " + CommissionDefolt + "%\t Комиссия при обмене более чем 500$ = " + CommissionException + "%\n");
            var Dollar = 0.0m;
            var Proverka = false;
            while (!Proverka)
            {
                log.Information("Пользователь вводит данные");
                Console.Write("Введите сумму в долларах : ");
                Proverka = decimal.TryParse(Console.ReadLine(), out Dollar);
                if(!Proverka)
                {
                    log.Error("Не парсится");
                    log.Error("Обмен не прошел!");
                }
            }

            log.Information($"Ввод прошел успешно. Пользователь ввел {Dollar} Долларов");
            var WriteKomissia = "Ваша сумма с учетом комиссии = ";
            var WriteNoKomissia = "Ваша сумма без комиссии комиссии = ";
            log.Information(Dollar > 500 ? $"Комиссия(%) {CommissionException}" : $"Комиссия(%) {CommissionDefolt}");
            if (Dollar > 500)          
            { 
                Console.WriteLine(WriteNoKomissia + Math.Round(Dollar * Kurs, 2) + " Руб.\n" + WriteKomissia + Math.Round(Dollar * Kurs / 100 * (100 - CommissionException), 2) + " Руб.");
                Sum = Math.Round(Dollar * Kurs / 100 * (100 - CommissionException), 2);
            }
            else
            {
                Console.WriteLine(WriteNoKomissia + Math.Round(Dollar * Kurs, 2) + " Руб.\n" + WriteKomissia + Math.Round(Dollar * Kurs - CommissionDefolt, 2) + " Руб.");
                Sum = Math.Round(Dollar * Kurs - CommissionDefolt, 2);
            }
            log.Information($"Обмен прошел успешно. Пользователь получил {Sum} Руб.  ") ;
            Console.ReadKey();
        }
    }
}
