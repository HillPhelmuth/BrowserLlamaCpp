import { LlamaCpp } from "./llama-st/llama.js";

let _app;
let _dotNet;
let _prompt = "";
export function init(dotNetObj) {
	_dotNet = dotNetObj;
}

const onModelLoaded = () => {

	_dotNet.invokeMethodAsync("ModelLoaded");
	_app.run({
		prompt: _prompt,
		ctx_size: 2048,
		temp: 0.8,
		top_k: 40,
		no_display_prompt: true,
	});
}

const onMessageChunk = (text) => {
	console.log(text);
	_dotNet.invokeMethodAsync("UpdateResult", text);

};

const onComplete = () => {
	console.log("model: completed");
	_dotNet.invokeMethodAsync("UpdateComplete");
};


export function loadAndExecuteModel(url, prompt) {

	_prompt = prompt;
	console.log(`model url: ${url} prompt: ${prompt}`);
	if (_app && _app.url === url) {
		onModelLoaded();
		return;
	}

	_app = new LlamaCpp(
		url,
		onModelLoaded,
		onMessageChunk,
		onComplete
	);

}
