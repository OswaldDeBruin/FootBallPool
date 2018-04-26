using System;

public class FootBallMatch
{
    //In conjunction with FootBallPool, Team variables are only to be used as pointers, do not create new teams for a single match
    public FootBallTeam Home;
    public FootBallTeam Away;
        
    public Tuple<int, int> score;
    //First int is home goals, second is away goals

    public bool played = false;
    //Set to true when match is over

    const int ScoreConstant = 6;
    //This constant is used to make a believable score in the simulation

    public FootBallMatch()
	{
        ResetMatch();
	}

    public void ResetMatch()
    {
        score = new Tuple<int, int>(0, 0);
        played = false;
    }

    
    private int SimulateScore (FootBallTeam player, FootBallTeam opponent, Random random)
    {
        float attack = player.attack * player.strategyModulation(random);
        float defence = opponent.defence * opponent.strategyModulation(random);
        float scorePart =  attack / (attack+defence);
        return (int)Math.Floor( (float)ScoreConstant * scorePart);
    }

    public void SimulateMatch(Random random)
    {
        if (played) return;// We do not play a match again
        int HomeScore = SimulateScore(Home, Away, random);
        int AwayScore = SimulateScore(Away, Home, random);
        score = new Tuple<int, int>(HomeScore, AwayScore);
        played = true;
    }
}
