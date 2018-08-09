"use strict";

export enum Notifications {
	All = "all",
	Mentions = "mentions",
	None = "none"
}

export enum TraceLevel {
	Silent = "silent",
	Errors = "errors",
	Verbose = "verbose",
	Debug = "debug"
}

export interface Config {
	autoSignIn: boolean;
	email: string;
	notifications: Notifications;
	serverUrl: string;
	webAppUrl: string;
	team: string;
	traceLevel: TraceLevel;
	showHeadshots: boolean;
	reduceMotion: boolean;
}
