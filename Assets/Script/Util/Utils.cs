using System.Collections.Generic;
using UnityEngine;
using YARG.Chart;

namespace YARG.Util {
	public static class Utils {
		/// <summary>
		/// Calculates the length of an Info(Note) object in beats.
		/// </summary>
		/// <param name="beatTimes">List of beat times associated with the Info object.</param>
		/// <returns>Length of the Info object in beats.</returns>
		public static float InfoLengthInBeats(Data.AbstractInfo info, List<Beat> beatTimes) {
			int beatIndex = 1;
			// set beatIndex to first relevant beat
			while (beatIndex < beatTimes.Count && beatTimes[beatIndex].Time <= info.time) {
				++beatIndex;
			}

			float beats = 0;
			// add segments of the length wrt tempo
			for (; beatIndex < beatTimes.Count && beatTimes[beatIndex].Time <= info.EndTime; ++beatIndex) {
				var curBPS = 1/(beatTimes[beatIndex].Time - beatTimes[beatIndex - 1].Time);
				// Unit math: s * b/s = pt
				beats += (beatTimes[beatIndex].Time - Mathf.Max(beatTimes[beatIndex - 1].Time, info.time)) * curBPS;
			}

			// segment where EndTime is between two beats (beatIndex-1 and beatIndex)
			if (beatIndex < beatTimes.Count && beatTimes[beatIndex - 1].Time < info.EndTime && info.EndTime < beatTimes[beatIndex].Time) {
				var bps = 1/(beatTimes[beatIndex].Time - beatTimes[beatIndex - 1].Time);
				beats += (info.EndTime - beatTimes[beatIndex - 1].Time) * bps;
			}
			// segment where EndTime is BEYOND the final beat
			else if (info.EndTime > beatTimes[^1].Time) {
				var bps = 1/(beatTimes[^1].Time - beatTimes[^2].Time);
				var toAdd = (info.EndTime - beatTimes[^1].Time) * bps;
				beats += toAdd;
			}

			return beats;
		}
	}
}
