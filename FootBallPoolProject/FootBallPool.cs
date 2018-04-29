using System;
using System.Collections.Generic;


public class FootBallPool
{
    //Class Footballpool acts as an agent for teams and matches, to assign teams to matches and handle the matches's outcomes.

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
    //TODO: Maybe make this list a return value instead
    public List<TeamStats> teamStats = new List<TeamStats>();//public list for easy stat reading by GUI

    //Since this is the highest agent in the simulation, it gets the agency over the random number generator
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

    //This method assigns points to a team in the pool based on a math's score outcome
    private TeamStats match2TeamStatCalculation(TeamStats thisTeam, int pointsFor, int pointsAgainst)
    {
        if (pointsFor > pointsAgainst) {//A winning team gets 3 points
            thisTeam.wins++;
            thisTeam.points += 3;
        }
        else if (pointsFor == pointsAgainst)//If a team ties, both teams get 1 point
        {
            thisTeam.points += 1;
        }
        thisTeam.goalDifference += pointsFor - pointsAgainst;
        thisTeam.goalsScored += pointsFor;
        thisTeam.goalsAgainst += pointsAgainst;
        return thisTeam;
    }

    private void calculateTeamStats()
    {
        if (teams == null || roster == null) return;//to prevent read errors

        //Making new stats
        teamStats.Clear();
        for (int i=0; i<teams.Count; i++)
        {
            teamStats.Add(new TeamStats());
        }

        //Checking every match
        for (int i = 0; i < roster.Count; i++)
        {
            for (int j = 0; j<roster[i].Count; j++)
            {
                if (roster[i][j] != null && roster[i][j].played)
                {
                    //Assigning points for each team individually
                    //Note: i=home index, j=away index, Item1=home-score, Item2=Away-score
                    teamStats[i] = match2TeamStatCalculation(teamStats[i], roster[i][j].score.Item1, roster[i][j].score.Item2);
                    teamStats[j] = match2TeamStatCalculation(teamStats[j], roster[i][j].score.Item2, roster[i][j].score.Item1);
                }
            }
        }
    }

    //a comparative method for sorting to determine if Team A ranks Higher than Team B
    private bool rankHigher(TeamStats A, TeamStats B)
    {
        if (A.points > B.points) return true;
        if (A.points < B.points) return false;
        if (A.wins > B.wins) return true;
        if (A.wins < B.wins) return false;
        if (A.goalDifference > B.goalDifference) return true;
        if (A.goalDifference < B.goalDifference) return false;
        if (A.goalsScored > B.goalsScored) return true;
        if (A.goalsScored < B.goalsScored) return false;
        if (A.goalsAgainst < B.goalsAgainst) return true;
        if (A.goalsAgainst > B.goalsAgainst) return false;
        return true;
    }

    //The ranking method returns a list of team indices in the order of ranking in the pool.
    public List<int> rankings()
    {
        //creating all stats
        calculateTeamStats();
        if (teamStats == null) return null;
        //creating a return value list with the indexes of the teams in ranked order
        List<int> rankIndex = new List<int>();
        for (int i = 0; i < teamStats.Count; i++)
        {
            rankIndex.Add(i);
        }
        
        //using simple bubble sort to order ranking
        for (int i = 0; i<rankIndex.Count-1; i++)
        {
            for (int j = i+1; j<rankIndex.Count; j++)
            {
                if (!rankHigher(teamStats[rankIndex[i]], teamStats[rankIndex[j]]))
                {
                    int temp = rankIndex[i];
                    rankIndex[i] = rankIndex[j];
                    rankIndex[j] = temp;
                }
            }
        }

        return rankIndex;
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
