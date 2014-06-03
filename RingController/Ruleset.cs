using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace PoomsaeBoard
{
    [DelimitedRecord(",")]
    public class Ruleset
    {
        public static FileHelperEngine engine = null;

        public String technical,
                      technicalMin,
                      technicalMax,
                      technicalMinor,
                      technicalMajor,
                      presentation,
                      poomsae,
                      final;

        [FieldConverter(typeof(PresentationRuleConverter))] 
        public PresentationRule[] presentations;

        public Ruleset() {
            this.technical = "";
            this.technicalMin = "";
            this.technicalMax = "";
            this.technicalMinor = "";
            this.technicalMajor = "";
            this.presentation = "";
            this.poomsae = "";
            this.final = "";
            this.presentations = new PresentationRule[0];
        }

        private static void checkEngine()
        {
            if (engine == null) engine = new FileHelperEngine(typeof(Ruleset));
        }

        public static Ruleset[] read(String file)
        {
            checkEngine();
            try
            {
                return engine.ReadFile(file) as Ruleset[];
            }
            catch
            {
                return null;
            }
        }

        public static void write(String file, params Ruleset[] records)
        {
            checkEngine();
            engine.WriteFile(file, records);
        }

        public static void append(String file, params Ruleset[] records)
        {
            checkEngine();
            engine.AppendToFile(file, records);
        }

        [DelimitedRecord(":")]
        public class PresentationRule
        {
            public String name,
                          min,
                          max,
                          step;

            public PresentationRule()
            {
                this.name = "";
                this.min = "0";
                this.max = "0";
                this.step = "0";
            }

            public PresentationRule(String name, String min, String max, String step)
            {
                this.name = name;
                this.min = min;
                this.max = max;
                this.step = step;
            }

            public override string ToString()
            {
                return this.name + ":" + this.min + ":" + this.max + ":" + this.step;
            }
        }

        public class PresentationRuleConverter : ConverterBase
        {
            public override object StringToField(string str)
            {
                String[] split = str.Split(':');
                PresentationRule p = new PresentationRule();
                if (split.Length > 0) p.name = split[0];
                if (split.Length > 1) p.min = split[1];
                if (split.Length > 2) p.max = split[2];
                if (split.Length > 3) p.step = split[3];
                return p;
            }
        }
    }
}
