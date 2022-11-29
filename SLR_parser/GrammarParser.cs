using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLR_parser {
    public class GrammarParser {

        public List<String> ParsingStack = new List<string>();
        public List<String> InputTape = new List<string>();
        public List<List<String>> Table = new List<List<string>>();
        public List<List<String>> StackTable = new List<List<string>>();

        public IDictionary<int, List<List<String>>> numbered_rules = new Dictionary<int, List<List<String>>>();

        public GrammarParser(List<List<String>> Table, IDictionary<int, List<List<String>>> numbered_rules) {
            
            int count = 0;
            foreach (var x in Table) {
                x.RemoveAt(0);
                count++;
            }

            this.Table = Table;
            this.numbered_rules = numbered_rules;
        }

        public void parse(String Input, List<String> cols) {

            foreach (var a in Input.ToCharArray()) {
                InputTape.Add(a.ToString());
            }
// dollar is added to parsing stack
            InputTape.Add("$");
            ParsingStack.Add("$0");
            

            String Action = "";

            while(InputTape.Count >= 0) {

                Console.WriteLine("STACK: " + String.Join(" , ", ParsingStack));
                Console.WriteLine("INPUT: " + String.Join(" , ", InputTape));
                Console.WriteLine("Action: " + Action);
                Console.WriteLine();

                String stackTop = ParsingStack[ParsingStack.Count - 1];
                String inputTop = InputTape[0];
                String TableLookup = Table[int.Parse(stackTop[1].ToString())][cols.IndexOf(inputTop)];
// table lookup has parsing table
                if (TableLookup.Contains("S")) {
                    InputTape.RemoveAt(0);
                    ParsingStack.Add(inputTop+TableLookup[1]);
                    // shift the state
                    Action = "SHIFT-"+ TableLookup[1];
                }
// if reduce then rule is poped
                else if (TableLookup.Contains("R")) {

                    int pops = numbered_rules[int.Parse(TableLookup[1].ToString())][1].Count;

                    for(int pop = 0; pop < pops; pop++) {
                        ParsingStack.RemoveAt(ParsingStack.Count - 1);
                    }

                    String num = Table[int.Parse(ParsingStack[ParsingStack.Count - 1][1].ToString())][cols.IndexOf(numbered_rules[int.Parse(TableLookup[1].ToString())][0][0])];
                    
                    ParsingStack.Add((numbered_rules[int.Parse(TableLookup[1].ToString())][0][0]).ToString()+num);
                    Action = "REDUCE " + numbered_rules[int.Parse(TableLookup[1].ToString())][0][0] + " -> "+String.Join(" ", numbered_rules[int.Parse(TableLookup[1].ToString())][1]);
                }

                else if (TableLookup.Contains("a")) {
                    Action = "Accept";
                    break;
                }

                else if(TableLookup.Contains("")) {
                    Action = "Error";
                    break;
                }

            }
            Console.WriteLine("STACK: " + String.Join(" , ", ParsingStack));
            Console.WriteLine("INPUT: " + String.Join(" , ", InputTape));
            Console.WriteLine("Action: " + Action);
            Console.WriteLine();

        }
    }
}
