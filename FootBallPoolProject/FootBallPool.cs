using System;
using System.Collections.Generic;

public class FootBallPool
{
    public List<List<FootBallMatch> > roster = new List<List<FootBallMatch> >();
    //2 dimensional list of matches 
    //first index is Home team, second index is Away team.
    //entry is NULL when Home and Away are the same.

    public List<FootBallTeam> teams = new List<FootBallTeam>();
    //All the teams entered in the pool

    //simple datastructure to calculate team ranking
    //This is seperate from the Team datastructure, because it is pool-specific and does not affect play
    public class TeamStats
    {
        public int points = 0;
        public int wins = 0;
        public int goalDifference = 0;
        public int goalsScored = 0;
        public int goalsAgainst = 0;
    }
    public List<TeamStats> teamStats = new List<TeamStats>();//public list for easy stat reading by GUI

    private Random random = new Random();

    public FootBallPool()
	{
	}

    public FootBallPool(int size)
    {
        CreateNewRandomPool(size);
    }


    //This method creates 4 brand new teams and gives the command to fill te roster with them
    //TODO: for future expansion, it's uncooth to keep the creation functionality in the pool, 
    //Later on, replace this with a team database and/or factory class;
    public bool CreateNewRandomPool(int size)
    {
        
        if (size < 2)
        {
            return false;//We do not create a pool without matches
        }
        teams.Clear();//clearing up old teams, if any
        
        for (int i = 0; i < size; i++)
        {
            FootBallTeam newTeam = new FootBallTeam();
            newTeam.randomize(random);
            teams.Add(newTeam);
        }
        FillRoster();
        return true;//Creation was succesfull
    }

    private void FillRoster()
    {
        roster.Clear();
        for (int i = 0; i < teams.Count; i++)//home team index
        {
            var row = new List<FootBallMatch>();//these are all the matches for the home team
            for (int j = 0; j < teams.Count; j++)//away team index
            {
                if (i == j)
                {
                    row.Add(null);//As defined above, a Match is null when Home team plays itself
                }
                else
                {
                    var match = new FootBallMatch();
                    match.Home = teams[i];
                    match.Away = teams[j];
                    row.Add(match);
                }
            }
            roster.Add(row);
        }
    }

    private TeamStats match2TeamStatCalculation(TeamStats thisTeam, int pointsFor, int pointsAgainst)
    {
        if (pointsFor > pointsAgainst) {
            thisTeam.wins++;
        }
        thisTeam.goalDifference += pointsFor - pointsAgainst;
        thisTeam.goalsScored += pointsFor;
        thisTeam.goalsAgainst += pointsAgainst;
        return thisTeam;
    }

    private void calculateTeamStats()
    {
        if (teams == null || roster == null) return;//to prevent read errors
        teamStats.Clear();
        for (int i=0; i<teams.Count; i++)
        {
            teamStats.Add(new TeamStats());
        }
        for (int i = 0; i < roster.Count; i++)
        {
            for (int j = 0; j<roster[i].Count; j++)
            {
                if (roster[i][j] != null)
                {
                    teamStats[i] = match2TeamStatCalculation(teamStats[i], roster[i][j].score.Item1, roster[i][j].score.Item2);
                    teamStats[j] = match2TeamStatCalculation(teamStats[j], roster[i][j].score.Item2, roster[i][j].score.Item1);
                }
            }
        }
    }


    public void ResetPool()
    {
        if (roster == null) return;
        foreach (var HomeRow in roster)
        {
            foreach (var match in HomeRow)
            {
                if (match != null)
                {
                    match.ResetMatch();
                }
            }
        }
        teamStats.Clear();
    }

    public void SimulatePool()
    {
        if (roster == null) return;
        foreach(var HomeRow in roster)
        {
            foreach (var match in HomeRow)
            {
                if (match != null)
                {
                    match.SimulateMatch(random);
                }
            }
        }
        calculateTeamStats();
    }
}
