// uuid generator taken from: https://gist.github.com/jed/982883
const createTempId = a =>
	a
		? (a ^ ((Math.random() * 16) >> (a / 4))).toString(16)
		: ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, createTempId);

export const markStreamRead = streamId => (dispatch, getState, { api }) => {
	if (!streamId) return;
	api.markStreamRead(streamId);
	return dispatch({ type: "CLEAR_UMI", payload: streamId });
};

export const createPost = (streamId, parentPostId, text, codeBlocks, mentions, extra) => async (
	dispatch,
	getState,
	{ api }
) => {
	const { context, session } = getState();
	const pendingId = createTempId();
	dispatch({
		type: "ADD_PENDING_POST",
		payload: {
			id: pendingId,
			streamId,
			parentPostId,
			codeBlocks,
			text,
			commitHashWhenPosted: context.currentCommit,
			creatorId: session.userId,
			pending: true
		}
	});
	try {
		const post = await api.createPost({
			id: pendingId,
			parentPostId,
			streamId,
			text,
			codeBlocks,
			mentions,
			extra
		});
		// FIXME: this is for analytics purposes and the extension host should probably send the event
		dispatch({ type: "POST_CREATED", meta: { post, ...extra } });
		return dispatch({
			type: "RESOLVE_PENDING_POST",
			payload: { pendingId, post }
		});
	} catch (e) {
		return dispatch({ type: "PENDING_POST_FAILED", payload: pendingId });
	}
};

export const retryPost = () => {
	// TODO
};

export const cancelPost = id => ({ type: "CANCEL_PENDING_POST", payload: id });

export const createSystemPost = (streamId, parentPostId, text, seqNum) => async (
	dispatch,
	getState,
	{ http }
) => {
	const state = getState();
	const { session, context } = state;
	const pendingId = createTempId();

	const post = {
		id: pendingId,
		teamId: context.currentTeamId,
		timestamp: new Date().getTime(),
		createdAt: new Date().getTime(),
		creatorId: "codestream",
		parentPostId: parentPostId,
		streamId,
		seqNum,
		text
	};

	dispatch({ type: "ADD_POST", payload: post });
};

export const editPost = () => {
	// TODO
};

export const deletePost = () => {
	// TODO
};

// usage: setUserPreference(["favorites", "shoes", "wedges"], "red")
export const setUserPreference = (prefPath, value) => (dispatch, getState, { api }) => {
	const { session, context, users } = getState();
	let user = users[session.userId];
	if (!user) return;

	if (!user.preferences) user.preferences = {};

	// we walk down the existing user preference to set the value
	// and simultaneously create a new preference object to pass
	// to the API server
	let preferences = user.preferences;
	let newPreference = {};
	let newPreferencePointer = newPreference;
	while (prefPath.length > 1) {
		let part = prefPath.shift().replace(/\./g, "*");
		if (!preferences[part]) preferences[part] = {};
		preferences = preferences[part];
		newPreferencePointer[part] = {};
		newPreferencePointer = newPreferencePointer[part];
	}
	preferences[prefPath[0].replace(/\./g, "*")] = value;
	newPreferencePointer[prefPath[0].replace(/\./g, "*")] = value;

	console.log("Saving preferences: ", newPreference);
	api.saveUserPreference(newPreference);
	// dispatch(saveUser(normalize(user)));
};

export const createStream = attributes => async (dispatch, getState, { api }) => {
	const { context, session } = getState();

	const stream = {
		teamId: context.currentTeamId,
		type: attributes.type
	};
	if (attributes.type === "channel") {
		stream.name = attributes.name;
		stream.privacy = attributes.privacy;
	}
	if (attributes.memberIds) stream.memberIds = attributes.memberIds;
	if (attributes.purpose) stream.purpose = attributes.purpose;

	try {
		const returnStream = await api.createStream(stream);
		dispatch(setCurrentStream(returnStream._id));
		return returnStream;
	} catch (error) {}
};

export const setCurrentStream = streamId => async dispatch => {
	dispatch({ type: "SET_CURRENT_STREAM", payload: streamId });
};

export const removeUsersFromStream = (streamId, userIds) => async (dispatch, getState, { api }) => {
	const update = {
		$pull: { memberIds: userIds }
	};

	try {
		const returnStream = await api.updateStream(streamId, update);
		console.log("return stream: ", returnStream);
		// if (streams.length > 0) dispatch(saveStreams(normalize(streams)));
	} catch (error) {
		console.log("Error: ", error);
	}
};

export const addUsersToStream = (streamId, userIds) => async (dispatch, getState, { api }) => {
	const update = {
		$push: { memberIds: userIds }
	};

	try {
		const returnStream = await api.updateStream(streamId, update);
		console.log("return stream: ", returnStream);
		// if (streams.length > 0) dispatch(saveStreams(normalize(streams)));
	} catch (error) {
		console.log("Error: ", error);
	}
};

export const renameStream = (streamId, name) => async (dispatch, getState, { api }) => {
	const update = { name };

	try {
		const returnStream = await api.updateStream(streamId, update);
		console.log("return stream: ", returnStream);
		return returnStream;
		// if (streams.length > 0) dispatch(saveStreams(normalize(streams)));
	} catch (error) {
		console.log("Error: ", error);
	}
};
