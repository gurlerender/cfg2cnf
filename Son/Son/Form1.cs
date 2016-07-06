using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFG2CNF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> state = new List<string>();
        Dictionary<string, string> states = new Dictionary<string, string>();
        List<string> stateRules = new List<string>();
        int rulesNumber;
        bool startState = false;
        private void button2_Click(object sender, EventArgs e)
        {

            string allString = richTextBox2.Text;

            string[] rules = allString.Split('\n');

            rulesNumber = rules.Count();

            List<string> stateRules = new List<string>();

            for (int i = 0; i < rulesNumber; i++)
            {
                state.AddRange(rules[i].Split(new string[] { "->" }, StringSplitOptions.None));

            }
            //son ekleme
            if(state[1].Contains("€"))
            {
                startState = true; 
            }

            List<string> tempList = new List<string>();

            for (int i = 0; i < rulesNumber*2;i=i+2 )
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;
                for (int j = 0; j < tempList.Count; j++) 
                {
                    for (int x = 0; x < rulesNumber * 2; x=x+2)
                    { 
                        if (tempList[j].Length == 1 && tempList[j] == state[x])
                        {
                            tempList[j] = state[x + 1];
                            if(tempList[j].Contains("€"))
                            {
                            //    tempList[j]=tempList[j].Replace("€", "");
                            }

                        }
                        
                    }
                    
                    state[i + 1] =state[i+1]+'|'+tempList[j];
                }

                for (int k = 0; k < tempList.Count; )
                {
                    tempList.RemoveAt(k);
                }
            }
            
            //son ek bitiş
            

                ControlEpsilon(rulesNumber);
        }

        private void ControlEpsilon(int rulesNumber)
        {
            int epsilonIndex;

            for (int i = 1; i < rulesNumber * 2; i = i + 2)
            {
                
                if (state[i].Contains('€'))
                {
                    epsilonIndex = i - 1;
                    ReplaceEpsilon(epsilonIndex);
                }

            }

            EliminateState();
        }

        private void ReplaceEpsilon(int epsilonIndex)
        {
            string epsilonState = state[epsilonIndex];

           // string temp;

            List<string> tempList = new List<string>();

            List<string> tempList2 = new List<string>();

         
           // List<string> addList = new List<string>();

            for (int i = 1; i < rulesNumber; i = i + 2)
            {
                if (state[i].Contains(epsilonState))
                {
                    //temp = state[i].Replace(epsilonState, "");

                    tempList.AddRange(state[i].Split('|'));

                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (tempList[j].Contains(epsilonState))
                        {
                            if (tempList[j].StartsWith(epsilonState) && tempList[j].EndsWith(epsilonState) && tempList[j].Length > 2)
                            {
                                tempList2.Add(epsilonState + tempList[j].Replace(epsilonState, ""));
                                tempList2.Add(tempList[j].Replace(epsilonState, "") + epsilonState);
                                tempList2.Add(tempList[j].Replace(epsilonState, ""));
                                state[i] = state[i] + "|" + tempList2[0] + "|" + tempList2[1] + "|" + tempList2[2];
                            }
                            else
                            {
                                tempList[j] = tempList[j].Replace(epsilonState, "");
                                state[i] = state[i] + "|" + tempList[j];
                            }

                        }
                    }

                   
                }
            }
            //     EliminateState();
        }

        private void EliminateState()
        {
            List<string> tempList = new List<string>();

            

            for (int i = 0; i < rulesNumber * 2; i = i + 2)
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;

                for (int j = 0; j < tempList.Count ; j++)
                {
                    if (tempList[j] == state[i] || tempList[j] == "€" || tempList[j] == "")
                    {
                        tempList.RemoveAt(j);
                        j--;
                        
                    }
                    else state[i + 1] = state[i + 1] + "|" + tempList[j];


                }
                for (int k = 0; k < tempList.Count; )
                {
                    tempList.RemoveAt(k);
                }

            }

            ChangeState();
        }

        private void ChangeState()
        {
          
            List<string> tempList = new List<string>();
            
            for (int i = 0; i < rulesNumber * 2; i = i + 2)
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;
                for (int j = 0; j < tempList.Count ; j++)
                {
                   for (int k = 0; k < rulesNumber * 2; k = k + 2)
                   {
                       if (tempList[j] == state[k])
                       {
                           tempList[j] = state[k + 1];
                           
                       }
                    
                   }
                   state[i + 1] = state[i + 1] + "|" + tempList[j];
                }
                

                    for (int k = 0; k < tempList.Count; )
                    {
                        tempList.RemoveAt(k);
                    }
            }
            AddStartState();
        }

        private void AddStartState()
        {
            List<string> state2 = new List<string>();
            state2.Add("S");
            state2.Add(state[1]);
            for (int i = 0; i < state.Count; i++)
            { 
                state2.Add(state[i]);
            }
            state = state2;
            EliminateMoreSymbols();
        }
        

        private void EliminateMoreSymbols()
        {
            List<string> tempList = new List<string>();
            List<string> newState = new List<string>();
            string[] controlLetter = { "A₁", "", "A₂", "", "A₃", "", "A₄", "", "A₅", "", "A₆", "", "A₇" };
            //1234567
            string tempString;

            for (int i = 0; i < (rulesNumber+1)  * 2; i = i + 2)
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;
                for (int j = 0; j < tempList.Count ; j++)
                {
                    if(tempList[j].Length >2)
                {

                    tempString = tempList[j].ToString();
                    tempString = tempString.First() + controlLetter[i];
                    
                    newState.Add(controlLetter[i]);
                    newState.Add(tempList[j].Substring(1,2));
                    tempList[j] = tempString;
                    
                }
                   state[i + 1] = state[i + 1] + "|" + tempList[j];
                }
                

                    for (int k = 0; k < tempList.Count; )
                    {
                        tempList.RemoveAt(k);
                    }

            }
            EliminateTerminals(newState);
            
            }

        private void EliminateTerminals(List<string> newState)
        {
            List<string> tempList = new List<string>();

            string tempString;

            int letterNumber=0;

            string[] controlLetter = { "B₁", "B₂", "B₃", "B₄", "B₅", "B₆", "B₇", "B₈", "B₉", "B₁₀", "B₁₁", "B₁₂" };

            for (int i = 0; i < (rulesNumber + 1) * 2; i = i + 2)
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;
                char tempChar;
                for (int j = 0; j < tempList.Count; j++)
                {
                   tempString = tempList[j];
                   
                   for (int k = 0; k < tempString.Length;k++ )
                   {
                       tempChar = tempString[k];
                       if (char.IsLower(tempChar) || char.IsDigit(tempChar))
                       {
                           
                           tempString = tempString.Replace(tempChar.ToString(), controlLetter[letterNumber]);
                           //tempString = tempList[j];
                          // tempString = controlLetter[i];
                           
                           newState.Add(controlLetter[letterNumber]);
                           newState.Add(tempChar.ToString());
                           tempList[j] =tempString;
                           letterNumber++;
                       }
                       
                   }
                   state[i + 1] = state[i + 1] + "|" + tempList[j];
                 
                }


                for (int k = 0; k < tempList.Count; )
                {
                    tempList.RemoveAt(k);
                }

            }

            ArrangeState(newState);
        }

        private void ArrangeState(List<string> newState)
        {
            List<string> tempList = new List<string>();


            for (int i = 0; i < (rulesNumber+1) * 2; i = i + 2)
            {
                tempList.AddRange(state[i + 1].Split('|'));
                state[i + 1] = null;
                for (int j = 0; j < tempList.Count; j++)
                {
                    if (tempList[j] == "")
                    {
                        tempList.RemoveAt(j);
                        j--;
                    }
                    else state[i + 1] = state[i + 1] + "|" + tempList[j];
                }

              
                for (int k = 0; k < tempList.Count; )
                {
                    tempList.RemoveAt(k);
                }
            }

            if (startState == true)
            {
                state[1] = '€' + state[1];
            }
            DisplayScreen(newState);
        }

        private void DisplayScreen(List<string> newState)
        {
            int location = 150 ;
            
            for (int i = 0; i < state.Count; i=i+2)
            {
                Label label = new Label();

                label.Top = 150+i*15;

                location = location + i * 15;

                label.Left = 50;

                label.Font = new Font("", 17);

                label.Width = 1500;

                label.Text += state[i]+"->" + state[i+1];

                this.Controls.Add(label);

            }
            for (int i = 0; i < newState.Count; i = i + 2)
            {
                Label label = new Label();

                label.Top = location + i * 15;

                label.Left = 50;

                label.Font = new Font("", 17);

                label.Width = 1500;

                label.Text += newState[i] + "->" + newState[i + 1];

                this.Controls.Add(label);
            }
            
            
        }

       
    }
       
}

