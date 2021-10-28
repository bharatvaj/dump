package ami.hacker.one;

import android.util.Log;

public class audioConverter {
	private static String TAG="audioConverter";
	private boolean audioSequencer()
	{
		boolean bool = false;
		try
		{
		//Convertion
		bool = true;
		}
		catch(Exception e)
		{
	    Log.e(TAG, e.getMessage());
		bool = false;
		}
		
		return bool;
	}

}
