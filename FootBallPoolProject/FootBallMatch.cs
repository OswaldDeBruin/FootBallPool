using System;

public class FootBallMatch
{
    //Class footballmatch acts as a data structure to hold match outcome statistics AND as an agent for letting teams compete with eachother
    //The current structure is used with the FootBallPool class, but can also be used in a knockout structure without any alteration.
    //TODO: when expanding, the simulation functionality should be delegated to a seperate agaent class at some point.

    
    public FootBallTeam Home;//Home team skill statistics
    public FootBallTeam Away;//Away team skill statistics
    //In conjunction with FootBallPool or knockout, Team variables are only to be used as pointers, do not create new teams for a single match

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

    //This method is run for each team seperately in the simulation
    //It simulates the amount of goals the team scores in a match, see class footbalteam for further reference
    private int SimulateScore (FootBallTeam player, FootBallTeam opponent, Random random)
    {
        //dice rolls:
        float attack = player.attack * player.strategyModulation(random);//simulating attack influenced by team's strategy
        float defence = opponent.defence * opponent.strategyModulation(random);//simulating opponent's defence by opponent's strategy

        //creating a believable score out of the dice rolls
        float scorePart =  attack / (attack+defence);
        return (int)Math.Floor( (float)ScoreConstant * scorePart);
    }

    //This method simulates the full match 
    public void SimulateMatch(Random random)
    {
        if (played) return;// We do not play a match more than once
        int HomeScore = SimulateScore(Home, Away, random);//Simulating Home team score
        int AwayScore = SimulateScore(Away, Home, random);//Simulating Away team score

        score = new Tuple<int, int>(HomeScore, AwayScore);//Registering simulated score
        played = true;//Taking note the match has played
    }
}
