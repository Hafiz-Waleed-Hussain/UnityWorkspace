using UnityEngine;
using System.Collections;

enum BounceDirection
{
	BounceDirectionGoingUp = 1,
	BounceDirectionStayingStill = 0,
	BounceDirectionGoingDown = -1,
	BounceDirectionGoingLeft = 2,
	BounceDirectionGoingRight = 3
};

public enum BWNumberSequence {
    BWNumberSequenceInOrder,
    BWNumberSequenceRandom,
    BWNumberSequenceReverse,
    BWNumberSequenceNumberAsked,
    BWNumberSequenceStartingPoint
};

public enum BWGameState {
	BWGameStateUnknown = -1,
	BWGameStateGuessed = 0,
	BWGameStateResetting
}
/*
public enum BWAudioState {
	BWAudioStateMute = -1,
	BWAudioStatePlayingIntro = 0,
	BWAudioStatePlayingIntro,
	BWAudioStatePlaying
}*/

public class BWConstants {
	
	public static float diagonalAngle = 14.0f * 0.0174532925f;
	public static float flowersLayerMaxX = 640;
	public static float flowersLayerMinX = -640;
	public static float screenWidth = 1200;
	public static int numbersOnScreen = 7;
	public static float idleTime = 10.0f;
}
