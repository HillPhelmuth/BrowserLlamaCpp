import { GGMLQuantizationType, gguf } from "@huggingface/gguf";
console.log(gguf);
export async function getModelSpecs(url) {
	/*const { metadata, tensorInfos } = await gguf(url);*/
	
	const { metadata, tensorInfos } = await gguf(url);
	console.log(metadata);
	return JSON.parse(JSON.stringify(metadata, (key, value) =>
		typeof value === 'bigint'
		? value.toString()
		: value // return everything else unchanged
	));
}