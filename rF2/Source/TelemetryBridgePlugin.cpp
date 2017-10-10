#include <WinSock2.h>			// socket WinSock2.h must be included before <windows.h>
#include "TelemetryBridgePlugin.hpp"          // corresponding header file
#include <math.h>               // for atan2, sqrt
#include <stdio.h>              // for sample output
#include <limits>
#include <string>         // std::string
// socket
#include <ws2tcpip.h>
// thread
#include <process.h>
#pragma comment(lib, "Ws2_32.lib")


// plugin information

extern "C" __declspec(dllexport)
const char * __cdecl GetPluginName() { return("TelemetryBridge.rF2"); }

extern "C" __declspec(dllexport)
PluginObjectType __cdecl GetPluginType() { return(PO_INTERNALS); }

extern "C" __declspec(dllexport)
int __cdecl GetPluginVersion() { return(7); }

extern "C" __declspec(dllexport)
PluginObject * __cdecl CreatePluginObject() { return((PluginObject *) new TelemetryBridgePlugin); }

extern "C" __declspec(dllexport)
void __cdecl DestroyPluginObject(PluginObject *obj) { delete((TelemetryBridgePlugin *)obj); }


// TelemetryBridgePlugin class

void TelemetryBridgePlugin::WriteToAllExampleOutputFiles(const char * const openStr, const char * const msg)
{
	FILE *fo;

	fo = fopen("ExampleInternalsTelemetryOutput.txt", openStr);
	if (fo != NULL)
	{
		fprintf(fo, "%s\n", msg);
		fclose(fo);
	}

	fo = fopen("ExampleInternalsScoringOutput.txt", openStr);
	if (fo != NULL)
	{
		fprintf(fo, "%s\n", msg);
		fclose(fo);
	}
}

void TelemetryBridgePlugin::SetEnvironment(const EnvironmentInfoV01 &info) {

	// as the SetEnvironment can be called several times at launch                  
	if (environmentAlreadySet) {
		return;
	}

	serverPort = 666;
	serverHost = "127.0.0.1";

	senderSocket = socket(PF_INET, SOCK_DGRAM, 0);
	if (senderSocket  < 0) {
		return;
	}

	sadSender.sin_family = AF_INET;
	sadSender.sin_port = htons(serverPort);
	sadSender.sin_addr.S_un.S_addr = inet_addr(serverHost);

	environmentAlreadySet = true;
}


void TelemetryBridgePlugin::Startup(long version)
{
	char temp[80];
	sprintf(temp, "-STARTUP- (version %.3f)", (float)version / 1000.0f);

	// Open ports, read configs, whatever you need to do.  For now, I'll just clear out the
	// example output data files.
	WriteToAllExampleOutputFiles("w", temp);

	// default HW control enabled to true
	mEnabled = true;
}


void TelemetryBridgePlugin::Shutdown()
{
	WriteToAllExampleOutputFiles("a", "-SHUTDOWN-");
}


void TelemetryBridgePlugin::StartSession()
{
	WriteToAllExampleOutputFiles("a", "--STARTSESSION--");
}


void TelemetryBridgePlugin::EndSession()
{
	WriteToAllExampleOutputFiles("a", "--ENDSESSION--");
}


void TelemetryBridgePlugin::EnterRealtime()
{
	// start up timer every time we enter realtime
	mET = 0.0f;
	WriteToAllExampleOutputFiles("a", "---ENTERREALTIME---");
}


void TelemetryBridgePlugin::ExitRealtime()
{
	WriteToAllExampleOutputFiles("a", "---EXITREALTIME---");
}


void TelemetryBridgePlugin::UpdateTelemetry(const TelemInfoV01 &info)
{
	// Use the incoming data, for now I'll just write some of it to a file to a) make sure it
	// is working, and b) explain the coordinate system a little bit (see header for more info)
	FILE *fo = fopen("ExampleInternalsTelemetryOutput.txt", "a");
	if (fo != NULL)
	{
		// Delta time is variable, as we send out the info once per frame
		fprintf(fo, "DT=%.4f  ET=%.4f\n", info.mDeltaTime, info.mElapsedTime);
		fprintf(fo, "Lap=%d StartET=%.3f\n", info.mLapNumber, info.mLapStartET);
		fprintf(fo, "Vehicle=%s\n", info.mVehicleName);
		fprintf(fo, "Track=%s\n", info.mTrackName);
		fprintf(fo, "Pos=(%.3f,%.3f,%.3f)\n", info.mPos.x, info.mPos.y, info.mPos.z);

		// Forward is roughly in the -z direction (although current pitch of car may cause some y-direction velocity)
		fprintf(fo, "LocalVel=(%.2f,%.2f,%.2f)\n", info.mLocalVel.x, info.mLocalVel.y, info.mLocalVel.z);
		fprintf(fo, "LocalAccel=(%.1f,%.1f,%.1f)\n", info.mLocalAccel.x, info.mLocalAccel.y, info.mLocalAccel.z);

		// Orientation matrix is left-handed
		fprintf(fo, "[%6.3f,%6.3f,%6.3f]\n", info.mOri[0].x, info.mOri[0].y, info.mOri[0].z);
		fprintf(fo, "[%6.3f,%6.3f,%6.3f]\n", info.mOri[1].x, info.mOri[1].y, info.mOri[1].z);
		fprintf(fo, "[%6.3f,%6.3f,%6.3f]\n", info.mOri[2].x, info.mOri[2].y, info.mOri[2].z);
		fprintf(fo, "LocalRot=(%.3f,%.3f,%.3f)\n", info.mLocalRot.x, info.mLocalRot.y, info.mLocalRot.z);
		fprintf(fo, "LocalRotAccel=(%.2f,%.2f,%.2f)\n", info.mLocalRotAccel.x, info.mLocalRotAccel.y, info.mLocalRotAccel.z);

		// Vehicle status
		fprintf(fo, "Gear=%d RPM=%.1f RevLimit=%.1f\n", info.mGear, info.mEngineRPM, info.mEngineMaxRPM);
		fprintf(fo, "Water=%.1f Oil=%.1f\n", info.mEngineWaterTemp, info.mEngineOilTemp);
		fprintf(fo, "ClutchRPM=%.1f\n", info.mClutchRPM);

		// Driver input
		fprintf(fo, "UnfilteredThrottle=%.1f%%\n", 100.0f * info.mUnfilteredThrottle);
		fprintf(fo, "UnfilteredBrake=%.1f%%\n", 100.0f * info.mUnfilteredBrake);
		fprintf(fo, "UnfilteredSteering=%.1f%%\n", 100.0f * info.mUnfilteredSteering);
		fprintf(fo, "UnfilteredClutch=%.1f%%\n", 100.0f * info.mUnfilteredClutch);

		// Filtered input
		fprintf(fo, "FilteredThrottle=%.1f%%\n", 100.0f * info.mFilteredThrottle);
		fprintf(fo, "FilteredBrake=%.1f%%\n", 100.0f * info.mFilteredBrake);
		fprintf(fo, "FilteredSteering=%.1f%%\n", 100.0f * info.mFilteredSteering);
		fprintf(fo, "FilteredClutch=%.1f%%\n", 100.0f * info.mFilteredClutch);

		// Misc
		fprintf(fo, "SteeringShaftTorque=%.1f\n", info.mSteeringShaftTorque);
		fprintf(fo, "Front3rdDeflection=%.3f Rear3rdDeflection=%.3f\n", info.mFront3rdDeflection, info.mRear3rdDeflection);

		// Aerodynamics
		fprintf(fo, "FrontWingHeight=%.3f FrontRideHeight=%.3f RearRideHeight=%.3f\n", info.mFrontWingHeight, info.mFrontRideHeight, info.mRearRideHeight);
		fprintf(fo, "Drag=%.1f FrontDownforce=%.1f RearDownforce=%.1f\n", info.mDrag, info.mFrontDownforce, info.mRearDownforce);

		// Other
		fprintf(fo, "Fuel=%.1f ScheduledStops=%d Overheating=%d Detached=%d\n", info.mFuel, info.mScheduledStops, info.mOverheating, info.mDetached);
		fprintf(fo, "Dents=(%d,%d,%d,%d,%d,%d,%d,%d)\n", info.mDentSeverity[0], info.mDentSeverity[1], info.mDentSeverity[2], info.mDentSeverity[3],
			info.mDentSeverity[4], info.mDentSeverity[5], info.mDentSeverity[6], info.mDentSeverity[7]);
		fprintf(fo, "LastImpactET=%.1f Mag=%.1f, Pos=(%.1f,%.1f,%.1f)\n", info.mLastImpactET, info.mLastImpactMagnitude,
			info.mLastImpactPos.x, info.mLastImpactPos.y, info.mLastImpactPos.z);

		// Wheels
		for (long i = 0; i < 4; ++i)
		{
			const TelemWheelV01 &wheel = info.mWheel[i];
			fprintf(fo, "Wheel=%s\n", (i == 0) ? "FrontLeft" : (i == 1) ? "FrontRight" : (i == 2) ? "RearLeft" : "RearRight");
			fprintf(fo, " SuspensionDeflection=%.3f RideHeight=%.3f\n", wheel.mSuspensionDeflection, wheel.mRideHeight);
			fprintf(fo, " SuspForce=%.1f BrakeTemp=%.1f BrakePressure=%.3f\n", wheel.mSuspForce, wheel.mBrakeTemp, wheel.mBrakePressure);
			fprintf(fo, " ForwardRotation=%.1f Camber=%.3f\n", -wheel.mRotation, wheel.mCamber);
			fprintf(fo, " LateralPatchVel=%.2f LongitudinalPatchVel=%.2f\n", wheel.mLateralPatchVel, wheel.mLongitudinalPatchVel);
			fprintf(fo, " LateralGroundVel=%.2f LongitudinalGroundVel=%.2f\n", wheel.mLateralGroundVel, wheel.mLongitudinalGroundVel);
			fprintf(fo, " LateralForce=%.1f LongitudinalForce=%.1f\n", wheel.mLateralForce, wheel.mLongitudinalForce);
			fprintf(fo, " TireLoad=%.1f GripFract=%.3f TirePressure=%.1f\n", wheel.mTireLoad, wheel.mGripFract, wheel.mPressure);
			fprintf(fo, " TireTemp(l/c/r)=%.1f/%.1f/%.1f\n", wheel.mTemperature[0], wheel.mTemperature[1], wheel.mTemperature[2]);
			fprintf(fo, " Wear=%.3f TerrainName=%s SurfaceType=%d\n", wheel.mWear, wheel.mTerrainName, wheel.mSurfaceType);
			fprintf(fo, " Flat=%d Detached=%d\n", wheel.mFlat, wheel.mDetached);
		}

		// Compute some auxiliary info based on the above
		TelemVect3 forwardVector = { -info.mOri[0].z, -info.mOri[1].z, -info.mOri[2].z };
		TelemVect3    leftVector = { info.mOri[0].x,  info.mOri[1].x,  info.mOri[2].x };

		// These are normalized vectors, and remember that our world Y coordinate is up.  So you can
		// determine the current pitch and roll (w.r.t. the world x-z plane) as follows:
		const double pitch = atan2(forwardVector.y, sqrt((forwardVector.x * forwardVector.x) + (forwardVector.z * forwardVector.z)));
		const double  roll = atan2(leftVector.y, sqrt((leftVector.x *    leftVector.x) + (leftVector.z *    leftVector.z)));
		const double radsToDeg = 57.296;
		fprintf(fo, "Pitch = %.1f deg, Roll = %.1f deg\n", pitch * radsToDeg, roll * radsToDeg);

		const double metersPerSec = sqrt((info.mLocalVel.x * info.mLocalVel.x) +
			(info.mLocalVel.y * info.mLocalVel.y) +
			(info.mLocalVel.z * info.mLocalVel.z));
		fprintf(fo, "Speed = %.1f KPH, %.1f MPH\n\n", metersPerSec * 3.6f, metersPerSec * 2.237f);

		// Close file
		fclose(fo);
	}
}


void TelemetryBridgePlugin::UpdateScoring(const ScoringInfoV01 &info)
{
	// Do my own stuff here
	memcpy(&message[0], &info.mTrackName, sizeof(char));
	sendto(senderSocket, message, sizeof(message), 0, (struct sockaddr *) &sadSender, sizeof(struct sockaddr));



	// Note: function is called twice per second now (instead of once per second in previous versions)
	FILE *fo = fopen("ExampleInternalsScoringOutput.txt", "a");
	if (fo != NULL)
	{
		// Print general scoring info
		fprintf(fo, "TrackName=%s\n", info.mTrackName);
		fprintf(fo, "Session=%d NumVehicles=%d CurET=%.3f\n", info.mSession, info.mNumVehicles, info.mCurrentET);
		fprintf(fo, "EndET=%.3f MaxLaps=%d LapDist=%.1f\n", info.mEndET, info.mMaxLaps, info.mLapDist);

		// Note that only one plugin can use the stream (by enabling scoring updates) ... sorry if any clashes result
		fprintf(fo, "START STREAM\n");
		const char *ptr = info.mResultsStream;
		while (*ptr != NULL)
			fputc(*ptr++, fo);
		fprintf(fo, "END STREAM\n");

		// New version 2 stuff
		fprintf(fo, "GamePhase=%d YellowFlagState=%d SectorFlags=(%d,%d,%d)\n", info.mGamePhase, info.mYellowFlagState,
			info.mSectorFlag[0], info.mSectorFlag[1], info.mSectorFlag[2]);
		fprintf(fo, "InRealtime=%d StartLight=%d NumRedLights=%d\n", info.mInRealtime, info.mStartLight, info.mNumRedLights);
		fprintf(fo, "PlayerName=%s PlrFileName=%s\n", info.mPlayerName, info.mPlrFileName);
		fprintf(fo, "DarkCloud=%.2f Raining=%.2f AmbientTemp=%.1f TrackTemp=%.1f\n", info.mDarkCloud, info.mRaining, info.mAmbientTemp, info.mTrackTemp);
		fprintf( fo, "Wind=(%.1f,%.1f,%.1f) mMinPathWetness=%.2f mMaxPathWetness=%.2f\n", info.mWind.x, info.mWind.y, info.mWind.z, info.mMinPathWetness, info.mMaxPathWetness );
		
		// Print vehicle info
		for (long i = 0; i < info.mNumVehicles; ++i)
		{
			VehicleScoringInfoV01 &vinfo = info.mVehicle[i];
			fprintf(fo, "Driver %d: %s\n", i, vinfo.mDriverName);
			fprintf(fo, " ID=%d Vehicle=%s\n", vinfo.mID, vinfo.mVehicleName);
			fprintf(fo, " Laps=%d Sector=%d FinishStatus=%d\n", vinfo.mTotalLaps, vinfo.mSector, vinfo.mFinishStatus);
			fprintf(fo, " LapDist=%.1f PathLat=%.2f RelevantTrackEdge=%.2f\n", vinfo.mLapDist, vinfo.mPathLateral, vinfo.mTrackEdge);
			fprintf(fo, " Best=(%.3f, %.3f, %.3f)\n", vinfo.mBestSector1, vinfo.mBestSector2, vinfo.mBestLapTime);
			fprintf(fo, " Last=(%.3f, %.3f, %.3f)\n", vinfo.mLastSector1, vinfo.mLastSector2, vinfo.mLastLapTime);
			fprintf(fo, " Current Sector 1 = %.3f, Current Sector 2 = %.3f\n", vinfo.mCurSector1, vinfo.mCurSector2);
			fprintf(fo, " Pitstops=%d, Penalties=%d\n", vinfo.mNumPitstops, vinfo.mNumPenalties);

			// New version 2 stuff
			fprintf(fo, " IsPlayer=%d Control=%d InPits=%d LapStartET=%.3f\n", vinfo.mIsPlayer, vinfo.mControl, vinfo.mInPits, vinfo.mLapStartET);
			fprintf(fo, " Place=%d VehicleClass=%s\n", vinfo.mPlace, vinfo.mVehicleClass);
			fprintf(fo, " TimeBehindNext=%.3f LapsBehindNext=%d\n", vinfo.mTimeBehindNext, vinfo.mLapsBehindNext);
			fprintf(fo, " TimeBehindLeader=%.3f LapsBehindLeader=%d\n", vinfo.mTimeBehindLeader, vinfo.mLapsBehindLeader);
			fprintf(fo, " Pos=(%.3f,%.3f,%.3f)\n", vinfo.mPos.x, vinfo.mPos.y, vinfo.mPos.z);

			// Forward is roughly in the -z direction (although current pitch of car may cause some y-direction velocity)
			fprintf(fo, " LocalVel=(%.2f,%.2f,%.2f)\n", vinfo.mLocalVel.x, vinfo.mLocalVel.y, vinfo.mLocalVel.z);
			fprintf(fo, " LocalAccel=(%.1f,%.1f,%.1f)\n", vinfo.mLocalAccel.x, vinfo.mLocalAccel.y, vinfo.mLocalAccel.z);

			// Orientation matrix is left-handed
			fprintf(fo, " [%6.3f,%6.3f,%6.3f]\n", vinfo.mOri[0].x, vinfo.mOri[0].y, vinfo.mOri[0].z);
			fprintf(fo, " [%6.3f,%6.3f,%6.3f]\n", vinfo.mOri[1].x, vinfo.mOri[1].y, vinfo.mOri[1].z);
			fprintf(fo, " [%6.3f,%6.3f,%6.3f]\n", vinfo.mOri[2].x, vinfo.mOri[2].y, vinfo.mOri[2].z);
			fprintf(fo, " LocalRot=(%.3f,%.3f,%.3f)\n", vinfo.mLocalRot.x, vinfo.mLocalRot.y, vinfo.mLocalRot.z);
			fprintf(fo, " LocalRotAccel=(%.2f,%.2f,%.2f)\n", vinfo.mLocalRotAccel.x, vinfo.mLocalRotAccel.y, vinfo.mLocalRotAccel.z);
		}

		// Delimit sections
		fprintf(fo, "\n");

		// Close file
		fclose(fo);
	}
}


bool TelemetryBridgePlugin::RequestCommentary(CommentaryRequestInfoV01 &info)
{
	// COMMENT OUT TO ENABLE EXAMPLE
	return(false);

	// only if enabled, of course
	if (!mEnabled)
		return(false);

	// Note: function is called twice per second

	// Say green flag event for no particular reason every 20 seconds ...
	const float timeMod20 = fmodf(mET, 20.0f);
	if (timeMod20 > 19.0f)
	{
		strcpy(info.mName, "GreenFlag");
		info.mInput1 = 0.0;
		info.mInput2 = 0.0;
		info.mInput3 = 0.0;
		info.mSkipChecks = true;
		return(true);
	}

	return(false);
}

