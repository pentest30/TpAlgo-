using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ClosedXML.Excel;
namespace ConsoleApplication5
{
    public class ALgoAvc
    {
        //une classe pour enregistré les resultas de chaque fonction par rapport les valeurs données.
        public string TypeFunction { get; set; }
        public double Value { get; set; }
        public decimal TimeSpan1 { get; set; }

        public double Result { get; set; }


    }
    class Program
    {
        readonly static Stopwatch Stopwatch = new Stopwatch();
        static void Main()
        {
            //un tavleau avec les valuer a testé
            double[] ar = {10, 20, 50,70, 100, 150, 200, 500, 700,1000};
          //cette liste est pour enregistré les resultat aprés chaque execution de cgaque fonction
            var list = new List<ALgoAvc>();
          //Itération sur la table des valeurs avec l'execution des 3 fonctions .
            foreach (var v in ar)
            {
                list.Add(EvaluateFactEtir(v));
                list.Add(EvaluateFactRec(v));
                list.Add(EvaluateFactRecTerminale(v));
            }
           // cette classe aide a enregistrer la liste des resultats sous forme d'une table dans un fichier excel.
            var wb = new XLWorkbook();
            List<ALgoAvc> final = list.OrderBy(x => x.TypeFunction).ToList();
            var t = ToDataTable(final);
            t.TableName = "Evaluation";
            wb.Worksheets.Add(t);
            wb.SaveAs("resultat.xlsx");
            Console.WriteLine("Les résultats sont enregistrés sous le repertoire suivant: " +Environment.NewLine + Environment.CurrentDirectory + @"\resultat.xlsx");
            Console.WriteLine("pour quitter tapez  n'importe quelle touche." );
            Console.ReadLine();

        }
        //cette fonction execute la fonction Recursive et calcul le temps d'execution en Nano seconde.
        static ALgoAvc EvaluateFactRec(double value)
        {
            var  item = new ALgoAvc();
            Stopwatch.Start();
            item.Result = FactRecur(value);
            Stopwatch.Stop();
            item.Value = value;
            item.TypeFunction = "Fonction Recursive";
            item.TimeSpan1 = Stopwatch.ElapsedTicks*1000000000/Stopwatch.Frequency;
            return item;
        }
        //cette fonction execute la fonction Itérative et calcul le temps d'execution.
        static ALgoAvc EvaluateFactEtir(double value)
        {
            var item = new ALgoAvc();
            Stopwatch.Start();
            item.Result = FactEtitaif(value);
            Stopwatch.Stop();
            item.Value = value;
            item.TypeFunction = "Fonction Itérative";
            item.TimeSpan1 = Stopwatch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
            return item;
        }
        //cette fonction execute la fonction Recursive terminale et calcul le temps d'execution.
        static ALgoAvc EvaluateFactRecTerminale(double value)
        {
            var item = new ALgoAvc();
            Stopwatch.Start();
            item.Result = FactRcTer(value,1);
            Stopwatch.Stop();
            item.Value = value;
            item.TypeFunction = "Fonction Recursive terminale";
            item.TimeSpan1 = Stopwatch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
            return item;
        }

        static double FactRecur(double n)
        {
            if (n == 0) return 1;
            return n*FactRecur(n - 1);
        }

        static double FactEtitaif(double n)
        {
            double a = n-1;
            double r = n;
            while (a>0)
            {
                r = r*a;
                a --;

            }
            return r;
        }

        static double FactRcTer(double n , double a  )
        {
            if (n == 0) return a*1;
            return FactRcTer(n - 1, a*n);
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
