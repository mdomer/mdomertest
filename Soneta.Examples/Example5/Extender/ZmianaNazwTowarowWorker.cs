﻿#define EXAMPLE5

using System.Linq;
using Soneta.Business;
using Soneta.Examples.Example5.Extender;
using Soneta.Tools;
using Soneta.Towary;

#if EXAMPLE5
[assembly: Worker(typeof(ZmianaNazwTowarowWorker), typeof(Towary))]
#endif

namespace Soneta.Examples.Example5.Extender {

    public class ZmianaNazwTowarowWorker : ContextBase {

        public ZmianaNazwTowarowWorker(Context context) : base(context) {
        }

        // Potrzebne dla akcji parametry
        [Context]
        public ZmianaNazwTowarowParams @params {
            get;
            set;
        }

        // Potrzebne dane na których zostanie wykonana akcja
        [Context]
        public Towar[] Towary {
            get; set;
        }

        // Akcja jaka zostanie wykonana na danych w oparciu o ustawione parametry
        [Action("Soneta Examples/Zmiana postfix-prefix", Mode = ActionMode.SingleSession | ActionMode.ConfirmSave | ActionMode.Progress)]
        public void ZmianaNazw() {
            using (var t = @params.Session.Logout(true)) {
                foreach (var towar in Towary.Where(towar => @params.TypTowaru == towar.Typ)) {

                    if (!@params.DodajPrefix.IsNullOrEmpty() && !towar.Nazwa.StartsWith(@params.DodajPrefix)) {
                        towar.Nazwa = @params.DodajPrefix + towar.Nazwa;
                    }

                    if (!@params.DodajPostfix.IsNullOrEmpty() && !towar.Nazwa.StartsWith(@params.DodajPostfix)) {
                        towar.Nazwa = towar.Nazwa + @params.DodajPostfix;
                    }

                    if (!@params.UsunPrefix.IsNullOrEmpty() && towar.Nazwa.StartsWith(@params.UsunPrefix)) {
                        towar.Nazwa = towar.Nazwa.Substring(@params.UsunPrefix.Length);
                    }

                    if (!@params.UsunPostfix.IsNullOrEmpty() && towar.Nazwa.EndsWith(@params.UsunPostfix)) {
                        towar.Nazwa = towar.Nazwa.Substring(0, towar.Nazwa.Length - @params.UsunPostfix.Length);
                    }

                }
                t.Commit();
            }
        }
    }

    public class ZmianaNazwTowarowParams : ContextBase {

        public ZmianaNazwTowarowParams(Context context) : base(context) {
            TypTowaru = TypTowaru.Towar;
        }

        public TypTowaru TypTowaru { get; set; }

        public string DodajPrefix { get; set; }

        public string DodajPostfix { get; set; }

        public string UsunPrefix { get; set; }

        public string UsunPostfix { get; set; }
    }
}
