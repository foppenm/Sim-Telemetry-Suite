#include "InternalsPlugin.hpp"

// This is used for the app to use the plugin for its intended purpose
class BridgePlugin : public InternalsPluginV07
{

public:

	// Constructor/destructor
	BridgePlugin() {
		senderSocket = 0;
	}
	~BridgePlugin() {}

	// These are the functions derived from base class InternalsPlugin
	// that can be implemented.
	void Startup(long version);  // game startup

	// GAME OUTPUT
	long WantsTelemetryUpdates() { return(1); } // CHANGE TO 1 TO ENABLE TELEMETRY EXAMPLE!
	void UpdateTelemetry(const TelemInfoV01 &info);

	// SCORING OUTPUT
	bool WantsScoringUpdates() { return(true); } // CHANGE TO TRUE TO ENABLE SCORING EXAMPLE!
	void UpdateScoring(const ScoringInfoV01 &info);

private:

	// socket variables
	int senderSocket; // socket to data
	struct sockaddr_in sadSender;
	const char *serverHost;
	u_short serverPort;

};