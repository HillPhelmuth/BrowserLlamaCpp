
const cacheName = "llama-cpp-wasm-cache";
async function openCacheStorage() {
	return await window.caches.open(cacheName);
}

function createRequest(url, method, body = "") {
	const requestInit = {
		method: method
	};

	if (body != "") {
		requestInit.body = body;
	}

	const request = new Request(url, requestInit);

	return request;
}

export async function store(url, method, body = "", responseString) {
	const llamaCache = await openCacheStorage();
	const request = createRequest(url, method, body);
	const response = new Response(responseString);
	await llamaCache.put(request, response);
}

export async function get(url, method, body = "") {
	const llamaCache = await openCacheStorage();
	const request = createRequest(url, method, body);
	const response = await llamaCache.match(request);

	if (response == undefined) {
		return "";
	}

	const result = await response.text();

	return result;
}
export async function getAllModels() {
	const llamaCache = await openCacheStorage();
	const result = await llamaCache.keys();
	console.log(result.map(x => x.url));
	return result.map(x => x.url);
}
export async function remove(url, method, body = "") {
	const llamaCache = await openCacheStorage();
	const request = createRequest(url, method, body);
	await llamaCache.delete(request);
}

export async function removeAll() {
	const llamaCache = await openCacheStorage();
	const requests = await llamaCache.keys();

	for (let i = 0; i < requests.length; i++) {
		await llamaCache.delete(requests[i]);
	}
}