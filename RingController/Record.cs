using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers; 

namespace PoomsaeBoard
{
    [DelimitedRecord(",")] 
    public class Record
    {
        public static FileHelperEngine engine = null;

        public String name;
        public double score1t = 0.0;
        public double score1p = 0.0;
        public double score1f = 0.0;
        public double score2t = 0.0;
        public double score2p = 0.0;
        public double score2f = 0.0;
        public double scoref = 0.0;
        public double time1 = 0.0;
        public double time2 = 0.0;

        public int judgeCount;

        [FieldConverter(typeof(QuadDoubleConverter))] 
        public QuadDouble[] judgeScores;

        private static void checkEngine()
        {
            if (engine == null) engine = new FileHelperEngine(typeof(Record));
        }

        public static Record[] read(String file)
        {
            checkEngine();
            try
            {
                return engine.ReadFile(file) as Record[];
            }
            catch
            {
                return null;
            }
        }

        public static void write(String file, params Record[] records)
        {
            checkEngine();
            engine.WriteFile(file, records);
        }

        public static void append(String file, params Record[] records)
        {
            checkEngine();
            engine.AppendToFile(file, records);
        }
    }

    [DelimitedRecord(":")]
    public class QuadDouble
    {
        public double score1t;
        public double score1p;
        public double score2t;
        public double score2p;

        public QuadDouble()
        {
            this.score1t = this.score1p = this.score2t = this.score2p = 0.0;
        }

        public QuadDouble(double score1t = 0.0, double score1p = 0.0, double score2t = 0.0, double score2p = 0.0)
        {
            this.score1t = score1t;
            this.score1p = score1p;
            this.score2t = score2t;
            this.score2p = score2p;
        }

        public override string ToString()
        {
            return score1t.ToString() + ":" + score1p.ToString() + ":" + score2t.ToString() + ":" + score2p.ToString();
        }
    }

    public class QuadDoubleConverter : ConverterBase
    {
        public override object StringToField(string str)
        {
            String[] split = str.Split(':');
            QuadDouble qd = new QuadDouble();

            if (split.Length > 0) Double.TryParse(split[0], out qd.score1t);
            if (split.Length > 1) Double.TryParse(split[1], out qd.score1p);
            if (split.Length > 2) Double.TryParse(split[2], out qd.score2t);
            if (split.Length > 3) Double.TryParse(split[3], out qd.score2p);

            return qd;
        }
    }
}
