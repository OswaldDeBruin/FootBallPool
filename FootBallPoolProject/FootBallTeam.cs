using System;

public class FootBallTeam
{
    //The FootBallTeam class is a statistics holder with a couple of functions for easy handling
    //Statistics are values between 0 and 1;
    public float attack;//Determines the amount of attempts on a goal a team can succeed in
    public float defence;//Determines the amount of attacks the team can thwart.
    public float strategy;
    //Strategy is the wildcard value that determines how much of the first 2 skills are randomized
    //A high strategy means a consistent team with very little deviation in team performance
    //A low strategy makes the team fluctual and perform at max at one game, while they flunk at the other

    public void randomize(Random random)
    {
        attack = (float)random.NextDouble();
        defence = (float)random.NextDouble();
        strategy = (float)random.NextDouble();
    }

	public FootBallTeam()
	{
	}

    //This method is a dice roll controlled by the strategy statistic
    public float strategyModulation(Random random)
    {
        return 1 - ((1 - strategy) * (float)random.NextDouble());
    }
}
