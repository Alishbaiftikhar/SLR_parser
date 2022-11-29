using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SLR_parser {
    public partial class Form1 : Form {

        public List<List<String>> Table = new List<List<string>>();
        public GenerateDFA dfa;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            String grammar = InputBox.Text;

            Preprocessor preprocessor1 = new Preprocessor();
            Tuple<int,String> output = preprocessor1.InitGrammar(grammar);

            preprocessor1.Find_FirstSet();
            preprocessor1.Find_FollowSet();

            Console.WriteLine("------- FIRST SET ---------");
            foreach (var item in preprocessor1.FIRST_SET) {
                String val = item.Key + " : { " + String.Join(", ", item.Value) + " } \n";
                Console.WriteLine(val);

            }

            Console.WriteLine("------- FOLLOW SET ---------");
            foreach (var item in preprocessor1.FOLLOW_SET) {
                String val = item.Key + " : { " + String.Join(", ", item.Value) + " } \n";
                Console.WriteLine(val);
            }

            dfa = new GenerateDFA(preprocessor1.Rules, preprocessor1.nonterminals, preprocessor1.Start_symbol);

            dfa.augmentGrammar(preprocessor1.Rules, preprocessor1.nonterminals, preprocessor1.Start_symbol);
            dfa.statesDict[0] = new List<List<List<String>>>( dfa.findClosure(dfa.AugRules, dfa.AugRules[0][0][0]));
            dfa.generateStates(dfa.statesDict);
            Table = dfa.createParseTable(dfa.statesDict, dfa.stateMap, preprocessor1.terminals, preprocessor1.nonterminals, preprocessor1.FOLLOW_SET);

            Console.WriteLine("GOTO states");
            foreach (var item in dfa.stateMap) {
                String val = "( " + item.Key.Item1 + " , " + item.Key.Item2 + " ) = " + item.Value + "\n";
                Console.WriteLine(val);
            }

            foreach (var st in dfa.statesDict) {
                Console.WriteLine("STATE : "+st.Key);
                foreach (var item in dfa.statesDict[st.Key]) {
                    Console.WriteLine("  "+ String.Join(" ", item[0]) + " -> "+String.Join(" ",item[1]));
                }
                Console.WriteLine("\n--------------------\n");
            }

            Console.WriteLine("     "+String.Join("  |   ", dfa.colss));

            int count = 0;
            foreach (var x in Table) {
                x.Insert(0, "I" + count);
                Console.WriteLine(String.Join("  |  ", x));
                count++;
            }

            GrammarParser parser = new GrammarParser(Table, dfa.numbered_rules);
//gives input for stack
            String inputString = "(a)";
            parser.parse(inputString, dfa.colss);

        }

        private void groupBox3_Enter(object sender, EventArgs e) {

        }

        private void ClearAll_Click(object sender, EventArgs e) {
        }

        private void Ptable_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void ParseButton_Click(object sender, EventArgs e) {

            
        }
    }
}
