using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootBallPoolProject
{
    public partial class Form1 : Form
    {
        private FootBallPool pool = new FootBallPool();
        public Form1()
        {
            InitializeComponent();
            CreateNewPool();
            UpdateTeamViewer();
            UpdatePoolViewer();
        }

        private void CreateNewPool()
        {
            pool.CreateNewRandomPool(4);//creating a new pool with 4 teams
        }

        private void UpdateTeamViewer()
        {
            teamViewLabel.Text = "";
            if (pool == null || pool.teams == null) return;
            string s = "";//short string name for clear code building
            for (int i = 0; i < pool.teams.Count; i++)
            {
                if (pool.teams[i] == null) continue;//This check is not necessary, but handy when expanding the code
                s += "Team " + (i + 1) + ":\n";
                s += "  Attack: " + pool.teams[i].attack + "\n";
                s += " Defence: " + pool.teams[i].defence + "\n";
                s += "Strategy: " + pool.teams[i].strategy + "\n";
                if (pool.teamStats!=null && pool.teamStats.Count > i)
                {
                    s += "Points:" + pool.teamStats[i].points + "\n";
                    s += "Wins:" + pool.teamStats[i].wins+"\n";
                    s += "Total Goals Scored/Against/Difference:" + pool.teamStats[i].goalsScored + "/" + pool.teamStats[i].goalsAgainst + "/" + pool.teamStats[i].goalDifference + "\n";
                }
                s += "\n";
            }
            teamViewLabel.Text = s;
        }

        private String intStringFormatting(int i)
        {
            string s = "";
            if (i < 10) s += " ";
            return s + i.ToString();
        }


        //Big method to parse the pool results into simple, readable text
        private void UpdatePoolViewer()
        {
            poolResultsLabel.Text = "";
            if (pool == null || pool.roster == null) return;
            string s = "Match results:\n"; //short string name for clear code building
            for (int i = 0; i < pool.roster.Count; i++)
            {
                if (pool.roster[i]!=null)
                {
                    s += intStringFormatting(i+1) + ":";
                    for (int j = 0; j < pool.roster[i].Count; j++)
                    {
                        if (j != 0)//Nice text divider
                        {
                            s += " | ";
                        }
                        //All matches are 5 chars wide
                        if (pool.roster[i][j] == null)
                        {
                            s += "  X  ";
                        }
                        else
                        {
                            if (pool.roster[i][j].played)
                            {
                                s += intStringFormatting(pool.roster[i][j].score.Item1);
                                s += "/";
                                s += intStringFormatting(pool.roster[i][j].score.Item2);
                            }
                            else
                            {
                                s += " -/- ";
                            }
                        }

                    }
                    s += "\n";
                }
            }
            if (pool.teamStats != null)
            {
                var ranking = pool.rankings();
                if (ranking != null)
                {
                    s += "Rankings: \n";
                    for( int i = 0; i<ranking.Count; i++)
                    {
                        s += (i+1) +":  Team " +(ranking[i]+1) + "\n";
                    }
                }
            }

            poolResultsLabel.Text = s;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            CreateNewPool();
            UpdateTeamViewer();
            UpdatePoolViewer();
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            pool.ResetPool();//TODO: This functionality might deserve its own button when expanding
            pool.SimulatePool();
            UpdateTeamViewer();
            UpdatePoolViewer();
        }
    }
}
