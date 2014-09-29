
//#line define PG_LAND 600;

public class PG_Constants {
	
	// Land height and width in BG_Mountain
	public static float PG_LAND_WIDTH=600F;
	public static float PG_LAND_HEIGHT=200F;
	public static float PG_SCREEN_WIDTH=1280F;
	
	
	
	
	  //----------- PATH
	// ---- Referent shapes
	public static string _referentsPath 	=   "JWPondGame/Sprites/Referent/";
	
	// ---- Sound Clips
	public static string _soundclipPath 	=   "JWPondGame/SoundClips/";
	public static string _correctAnswer 	=   "correctAnswer/";
	public static string _wrongAnswer 		=     "wrongAnswer/";
	public static string _differentMode 	=   "differentMode/";
	public static string _basicShapes 		=   "basicShapes/";
	public static string _find 				=   "find/";
	public static string _counting 			=   "counting/";
	public static string _celebration 		=   "celebration/";
	public static string _tickle			=	"tickle/";
	public static string _quickSandHurry	=    "quickSand/";
	public static string _crying			=    "crying/";
	public static string _jump				=    "jump";
	public static string _rollingShapes		=    "rollingStones/rollingStones";
	public static string _introClips		=		"introClips/";
	
	
	// ----- Backgrounds
	public static string _background 	=   "JWPondGame/Sprites/Backgrounds/";
	public static string _shapes	 	=   "JWPondGame/Sprites/Shapes/";
	public static string _sprites	 	=   "JWPondGame/Sprites/";
	
	
	// ----- Numbers
	public static string _numbers	= "Numbers/";
	
	
	// ----- Animation Sprite Sheets
	// ---- Olly Animations
	public static string _animations			=   "JWPondGame/Animations/";
	public static string _ollyIdle				=   "ollyIdleAnim";
	public static string _ollyJumpLandToWater	=   "ollyJumpLandToWater";
	public static string _ollyJumpWaterToWater	=   "ollyJumpWaterToWater";
	public static string _ollyHop				=   "ollyHopAnim";
	public static string _ollyWaterTOLand		=   "ollyWaterToLandAnim";
	public static string _ollyCelebrationAnim	=   "ollyCelebrationAnim";
	public static string _ollyCelebrationAnim2	=   "ollyCelebrationAnim2";
	public static string _ollyTickle			=   "ollyTickleAnim";
	public static string _ollyWrongAnswer		=   "ollyWrongAnswer";
	public static string _ollyStill				=   "ollyStill";
	public static string _ollyWeap				=   "ollyWeapAnim";
	
	
	// ------ Icky Animations
	public static string _ickyCrossPond			=   "ickyCrossPondAnim";
	public static string _ickyIdle				=   "ickyIdleAnim";
	public static string _ickyCome				=   "ickyComeAnim";
	public static string _ickyCelebration		=   "ickyCelebrationAnim";
	public static string _ickyTickle			=   "ickyTickleAnim";
	public static string _ickyWeap				=   "ickyWeapAnim";
	
	
	
	// ------ correct/wrong tap sprites
	public static string _wrongTick					=   "JWPondGame/Sprites/Referent/CP_Cross-hd";
	public static string _correctTick				=   "JWPondGame/Sprites/Referent/CP_tick-hd";
	public static string _questionMark				=   "JWPondGame/Sprites/Referent/NumberQuestion-hd";
	
	// ----- Hard Mode Quick Sand
	public static string _quickSand					=    "JWPondGame/Sprites/Sands/";
	
	// ------------ Formula to calculate the Olly position on island
	// center of olly = center of island + (islandwidht/4)
	// center of icky = center of island - (islandwidht/4)
	
	public enum ISLAND_ONE_SHAPE
	{
		// islandLeft Center  = -426 , scale = 554 so ollyPosition = -426 + 554/4 = -287
		// islandRight Center =  396, scale  = 541 so ickyPosition =  396 - 541/4 =  261
		
		ollyPositionX = -200,
		ollyPositionY =  136,
		ollyPositionZ = -137,
		
		ickyPositionX = 197,
		ickyPositionY = 149,
		ickyPositionZ = -40
	}
	
	public enum ISLAND_TWO_SHAPES
	{
		// islandLeft Center  = -495 , scale = 417 so ollyPosition = -495 + 417/4 = -390
		// islandRight Center =  464, scale  = 394 so ickyPosition =  464 - 394/4 =  365
		
		ollyPositionX = -347,
		ollyPositionY =  153,
		ollyPositionZ = -40,
		
		ickyPositionX = 340,
		ickyPositionY = 166,
		ickyPositionZ = -40
		
	}	
	
	
	public enum ISLAND_THREE_SHAPES
	{
		// islandLeft Center  = -532 , scale = 344 so ollyPosition = -532 + 344/4 = -446
		// islandRight Center =  494, scale  = 331 so ickyPosition =  494 - 331/4 =  412
		
		ollyPositionX = -431,
		ollyPositionY =  141,
		ollyPositionZ = -40,
		
		ickyPositionX = 402,
		ickyPositionY = 158,
		ickyPositionZ = -40
	}
	
	public enum ISLAND_FOUR_SHAPES
	{
		// islandLeft Center  = -562 , scale = 282 so ollyPosition = -562 + 282/4 = -491
		// islandRight Center =  530, scale  = 263 so ickyPosition =  530 - 263/4 =  465
		
		ollyPositionX = -493,
		ollyPositionY =  141,
		ollyPositionZ = -40,
		
		ickyPositionX = 470,
		ickyPositionY = 157,
		ickyPositionZ = -40
	}
	
	
	
}
